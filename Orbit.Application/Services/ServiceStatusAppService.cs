using AutoMapper;
using AutoMapper.QueryableExtensions;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Game.Enums;
using Orbit.Domain.Game.Models;
using Orbit.Domain.Generic;
using Orbit.Domain.ServiceStatus.Commands;
using Orbit.Infra.Persistence.Repository.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbit.Application.Services
{
    public class ServiceStatusAppService : IServiceStatusAppService
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IRepository<Orbit.Domain.Game.Models.ServiceStatus> _repository;
        private readonly IMediatorHandler _bus;

        public ServiceStatusAppService(IMapper mapper, IEventStoreRepository eventStoreRepository, IRepository<ServiceStatus> repository, IRepository<GenericObject> genericRepository, IMediatorHandler bus)
        {
            _mapper = mapper;
            _eventStoreRepository = eventStoreRepository;
            _repository = repository;
            _bus = bus;
        }

        public void Create(ServiceStatusViewModel model)
        {
            var createCommand = _mapper.Map<CreateServiceStatusCommand>(model);
            _bus.SendCommand(createCommand);
        }

        public IEnumerable<ServiceStatusViewModel> GetAll()
        {
            var serviceStates = _repository.GetAll();
            var serviceStatusViewModels = serviceStates.ProjectTo<ServiceStatusViewModel>();
            return serviceStatusViewModels.AsEnumerable();
        }

        public IEnumerable<ServiceStatusViewModel> GetRecent()
        {
            //fck automapper rn
            var serviceStates = 
                from r in _repository.GetAll()
                group r by r.Service 
                into g select g.OrderByDescending(e => e.TimeStamp).FirstOrDefault();
            var serviceStatusViewModels = from n in serviceStates select new ServiceStatusViewModel(n.Id,n.Service,n.TimeStamp,(int)n.State);
            return serviceStatusViewModels.AsEnumerable();
        }

        public IEnumerable<ServiceStatusViewModel> GetRecentPublic()
        {
            var serviceStates =
                from r in _repository.GetAll()
                group r by r.Service
                into g
                select g.OrderByDescending(e => e.TimeStamp).FirstOrDefault();
            EServiceState flag = EServiceState.Online;
            foreach (var item in serviceStates)
            {
                if (item.State != Domain.Game.Enums.EServiceState.Online || (DateTime.Now - item.TimeStamp) > new TimeSpan(0,5,0)) flag = EServiceState.Offline;
            }
            return new[] { new ServiceStatusViewModel(Guid.NewGuid(),"Server",DateTime.Now,(int)flag)};
        }

        public void Remove(Guid id)
        {
            var removeCommand = _mapper.Map<RemoveServiceStatusCommand>(id);
            _bus.SendCommand(removeCommand);
        }

        public void Update(ServiceStatusViewModel model)
        {
            var updateCommand = _mapper.Map<UpdateServiceStatusCommand>(model);
            _bus.SendCommand(updateCommand);
        }
    }
}
