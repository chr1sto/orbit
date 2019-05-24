using AutoMapper;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Statistics;
using Orbit.Domain.Statistics.Commands;
using Orbit.Infra.Persistence.Repository.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbit.Application.Services
{
    public class StatisticsAppService : IStatisticsAppService
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IRepository<StatisticsEntry> _repository;
        private readonly IMediatorHandler _bus;

        public StatisticsAppService(IMapper mapper, IRepository<StatisticsEntry> repository, IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _mapper = mapper;
            _repository = repository;
            _eventStoreRepository = eventStoreRepository;
            _bus = bus;
        }

        public void Create(StatisticsEntryViewModel model)
        {
            var createCommand = _mapper.Map<CreateStatisticsEntryCommand>(model);
            _bus.SendCommand(createCommand);
        }

        public IEnumerable<StatisticsEntryViewModel> Get(DateTime from, DateTime until, string statGroup, string statName = null, char interval = 'h')
        {
            var query = _repository.GetAll();
            //End currently is the same as Start...
            if(statName == null)
            {
                query = query.Where(s => s.Start > from && s.End < until && s.StatGroup == statGroup);
            }
            else
            {
                query = query.Where(s => s.Start > from && s.End < until && s.StatGroup == statGroup && s.StatName == statName);
            }

            //TODO Take average if type is number or date!

            switch(interval)
            {
                case 'm':
                    query = from r in query group r by new { year = r.Start.Year, month = r.Start.Month, day = r.Start.Day, hour = r.Start.Hour, minute = r.Start.Minute } into g select g.FirstOrDefault();
                    break;
                case 'd':
                    query = from r in query group r by new { year = r.Start.Year, month = r.Start.Month, day = r.Start.Day } into g select g.FirstOrDefault();
                    break;
                case 'M':
                    query = from r in query group r by new { year = r.Start.Year, month = r.Start.Month } into g select g.FirstOrDefault();
                    break;
                case 'h':
                default:
                    query = from r in query group r by new { year = r.Start.Year, month = r.Start.Month, day = r.Start.Day, hour = r.Start.Hour } into g select g.FirstOrDefault();
                    break;
            }

            //Hopefully this sh*t works...

            return _mapper.ProjectTo<StatisticsEntryViewModel>(query).AsEnumerable();
        }
    }
}
