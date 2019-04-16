using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Infra.CrossCutting.Identity.Data
{
    internal class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var h = new HostingEnvironment
            {
                ContentRootPath = @"C:\Projekte\orbit\Orbit.Api"
            };
            var x = new ApplicationDbContext(h);
            return x;
        }
    }
}
