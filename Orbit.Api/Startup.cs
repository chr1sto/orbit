using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orbit.Infra.CrossCutting.Identity.Data;
using Orbit.Infra.CrossCutting.Identity.Models;
using MediatR;
using Orbit.Api.Configurations;
using Orbit.Infra.CrossCutting.IoC;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;
using System.Linq;
using Orbit.Api.Hubs;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Orbit.Api
{
    public class Startup
    {
        private const string VERSION = "1";
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            /*
            if(env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            */
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });

                options.AddPolicy("DEV_ENV_SOCKET", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                    builder.WithOrigins("https://localhost:4200", "http://localhost:4200");
                });

                options.AddPolicy("PROD_ENV_SOCKET", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                    builder.WithOrigins("https://euphresia-flyff.com", "http://euphresia-flyff.com");
                });
            });

            services.AddMemoryCache();

            services.Configure<IISServerOptions>(options => options.AutomaticAuthentication = false);

            services.AddDbContext<ApplicationDbContext>();

            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc(options =>
            {
                options.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter());
                options.UseCentralRoutePrefix(new RouteAttribute($"v{VERSION}"));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAutoMapperSetup();

            //Todo: Identity Options

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecret"])),
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            //Todo: Policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.NameIdentifier);
                });
            });

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Orbit"
                });
            }
            );

            services.AddMediatR(typeof(Startup));


            services.AddSignalR();
            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // SOCKETS
            app.Map("/sock", b =>
            {
                if(env.IsDevelopment())
                {
                    b.UseCors("DEV_ENV_SOCKET");
                }
                else
                {
                    b.UseCors("PROD_ENV_SOCKET");
                }

                b.Use(async (context, next) =>
                {
                    await AuthQueryStringToHeader(context, next);
                });

                b.UseHttpsRedirection();
                b.UseAuthentication();
                b.UseSignalR(route =>
                {
                    route.MapHub<VoteHub>("/vote");
                });
            });

            //API
            app.Map("/api", b =>
            {
                b.UseCors();
                b.UseHttpsRedirection();
                b.UseAuthentication();
                b.UseMvc();
            });
            
            //SATIC FILES
            app.MapWhen(r => !r.Request.Path.Value.StartsWith("/swagger") && !r.Request.Path.Value.StartsWith("/api") && !r.Request.Path.Value.StartsWith("/sock"), b =>
            {
                b.UseCors();
                b.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Configuration["FILE_UPLOAD_ROOT_PATH"]),
                    RequestPath = new Microsoft.AspNetCore.Http.PathString("/static")
                }
                );
            });

            //DOCUMENTATION
            if (env.IsDevelopment())
            {
                app.UseSwagger(options => {
                    options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.BasePath = "/api";
                    });

                    options.PreSerializeFilters.Add((swaggerDoc, httpReq) => {
                        IDictionary<string, PathItem> paths = new Dictionary<string, PathItem>();
                        foreach (var path in swaggerDoc.Paths)
                        {
                            paths.Add(path.Key.Replace("/api", "/"), path.Value);
                        }
                        swaggerDoc.Paths = paths;
                    });
                });
                app.UseSwaggerUI(s =>
                {
                    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Orbit");
                });
            }

            EnsureRolesCreated(serviceProvider).Wait();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            NativeInjectionBootstrapper.RegisterServices(services);
        }

        private async Task EnsureRolesCreated(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = new string[] { "Developer", "Administrator", "Gamemaster", "User", "GameService" };
            IdentityResult result;
            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    result = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task AuthQueryStringToHeader(HttpContext context, Func<Task> next)
        {
            var token = context.Request.Query["access_token"];


            if (!string.IsNullOrWhiteSpace(token))
            {
                context.Request.Headers.Add("Authorization", "Bearer " + token);
            }

            await next?.Invoke();
        }
    }
}
