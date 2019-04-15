using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Orbit.Application.Interfaces;
using Orbit.Application.Services;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Events;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Domain.Game.Events;
using Orbit.Domain.GameAccount.CommandHandlers;
using Orbit.Domain.GameAccount.Commands;
using Orbit.Domain.GameAccount.EventHandlers;
using Orbit.Domain.News;
using Orbit.Domain.News.CommandHandlers;
using Orbit.Domain.News.Commands;
using Orbit.Domain.News.EventHandlers;
using Orbit.Domain.News.Events;
using Orbit.Infra.CrossCutting.Bus;
using Orbit.Infra.CrossCutting.Identity.Authorization;
using Orbit.Infra.CrossCutting.Identity.Models;
using Orbit.Infra.Persistence.Context;
using Orbit.Infra.Persistence.EventSourcing;
using Orbit.Infra.Persistence.Repository;
using Orbit.Infra.Persistence.Repository.EventSourcing;
using Orbit.Infra.Persistence.UoW;

namespace Orbit.Infra.CrossCutting.IoC
{
    public class NativeInjectionBootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // ASP.NET Authorization Polices
            services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();

            // Application
            services.AddScoped<INewsAppService, NewsAppService>();
            services.AddScoped<IGameAccountAppService, GameAccountAppService>();
            services.AddScoped<IGameEventAppService, GameEventAppService>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<NewsPostCreatedEvent>, NewsPostEventHandler>();
            services.AddScoped<INotificationHandler<NewsPostUpdatedEvent>, NewsPostEventHandler>();
            services.AddScoped<INotificationHandler<NewsPostRemovedEvent>, NewsPostEventHandler>();
            services.AddScoped<INotificationHandler<GameAccountCreatedEvent>, GameAccountEventHandler>();
            services.AddScoped<INotificationHandler<GameAccountUpdatedEvent>, GameAccountEventHandler>();

            // Domain - Commands
            services.AddScoped<IRequestHandler<CreateNewsPostCommand, bool>, NewsPostCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateNewsPostCommand, bool>, NewsPostCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveNewsPostCommand, bool>, NewsPostCommandHandler>();
            services.AddScoped<IRequestHandler<CreateGameAccountCommand, bool>, GameAccountCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateGameAccountCommand, bool>, GameAccountCommandHandler>();

            // Infra - Data
            services.AddScoped<IRepository<NewsPost>,Repository<NewsPost>>();
            services.AddScoped<IRepository<Orbit.Domain.Game.Models.GameAccount>, Repository<Orbit.Domain.Game.Models.GameAccount>>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<OrbitContext>();

            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<EventStoreContext>();

            // Infra - Identity Services
            // TODO

            // Infra - Identity
            services.AddScoped<IUser, AspNetUser>();
        }

    }
}
