using Orbit.Domain.Game.Models;
using Orbit.Game.Core.Data;
using Orbit.Game.Core.Interfaces;
using Orbit.Game.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbit.Game.Core.Services
{
    public class GameAccountService : IGameAccountService
    {
        private readonly AccountDbContext _accountDbContext;

        public GameAccountService(AccountDbContext accountDbContext)
        {
            _accountDbContext = accountDbContext;
        }

        public Task<bool> BanAccount(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateAccount(string accountId, string userId)
        {
            var account = new Account()
            {
                AccountId = accountId,
                Password = "",
                IsUse = 'T',
                Member = 'A',
                IdNo1 = string.Empty,
                IdNo2 = string.Empty,
                RealName = char.MinValue,
                Cash = 0,
                UserId = userId
            };

            var accountDetail = new AccountDetail()
            {
                AccountId = accountId,
                GameCode = "A000",
                Tester = '2',
                LoginAuthority = 'F',
                RegDate = DateTime.Now,
                BlockTime = DateTime.Now.AddDays(-1).ToString("yyyyMMdd"),
                EndTime = DateTime.Now.AddYears(10).ToString("yyyyMMdd"),
                WebTime = DateTime.Now.AddDays(-1).ToString("yyyyMMdd"),
                IsUse = 'T',
                Secession = null,
                Email = null
            };

            var playDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));

            var accountPlayTime = new AccountPlayTime()
            {
                AccountId = accountId,
                PlayDate = playDate,
                PlayTime = 0
            };

            _accountDbContext.Add(account);
            _accountDbContext.Add(accountDetail);
            _accountDbContext.Add(accountPlayTime);
            _accountDbContext.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<bool> DeleteAccount(string accountId)
        {
            throw new NotImplementedException();
        }
    }
}
