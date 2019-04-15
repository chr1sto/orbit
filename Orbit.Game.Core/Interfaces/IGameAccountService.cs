using Orbit.Domain.Game.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbit.Game.Core.Interfaces
{
    public interface IGameAccountService
    {
        Task<bool> BanAccount(string accountId);
        Task<bool> CreateAccount(string accountId);
        Task<bool> DeleteAccount(string accountId);
    }
}
