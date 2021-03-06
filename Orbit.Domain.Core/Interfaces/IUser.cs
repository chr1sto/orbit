using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Orbit.Domain.Core.Interfaces
{
    public interface IUser
    {
        string Name { get; }
        Guid Id { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
    }
}
