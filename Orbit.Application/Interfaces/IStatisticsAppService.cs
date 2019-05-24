using Orbit.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.Interfaces
{
    public interface IStatisticsAppService
    {
        void Create(StatisticsEntryViewModel model);
        IEnumerable<StatisticsEntryViewModel> Get(DateTime from, DateTime until, string statGroup, string statName = null, char interval = 'h');
    }
}
