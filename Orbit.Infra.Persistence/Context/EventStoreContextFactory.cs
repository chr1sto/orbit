using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;

namespace Orbit.Infra.Persistence.Context
{
    internal class EventStoreContextFactory : IDesignTimeDbContextFactory<EventStoreContext>
    {
        public EventStoreContext CreateDbContext(string[] args)
        {
            var h = new HostingEnvironment
            {
                ContentRootPath = @"C:\Projekte\orbit\Orbit.Api"
            };
            var x = new EventStoreContext(h);
            return x;
        }
    }
}
