using Orbit.Domain.Core.Interfaces;
using Orbit.Infra.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Infra.Persistence.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrbitContext _context;

        public UnitOfWork(OrbitContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
