using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Infra.Persistence.Context
{
    internal class OrbitContextFactory : IDesignTimeDbContextFactory<OrbitContext>
    {
        public OrbitContext CreateDbContext(string[] args)
        {
            var h = new HostingEnvironment
            {
                ContentRootPath = @"C:\git\Euphresia\orbit\Orbit.Api"
            };
            var x = new OrbitContext(h);
            return x;
        }
    }
}
