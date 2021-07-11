using Orbit.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbit.Application.Interfaces
{
    public interface IStatisticsAppService
    {
        void Create(StatisticsEntryViewModel model);
        Task<IEnumerable<StatisticsEntryViewModel>> Get(DateTime from, DateTime until, string statGroup, string statName = null,int count = 15, char interval = 'h');
    }
}
