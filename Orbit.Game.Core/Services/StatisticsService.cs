using Microsoft.Extensions.DependencyInjection;
using Orbit.Game.Core.Data;
using Orbit.Game.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbit.Game.Core.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public StatisticsService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public int GetPlayerCount(int channel)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CharacterDbContext>();
                return context.Characters.Where(x => x.MultiServer == channel).Count();
            }
        }

        public int GetTotalGold()
        {
            throw new NotImplementedException();
        }
    }
}
