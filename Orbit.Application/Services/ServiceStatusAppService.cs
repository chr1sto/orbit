using AutoMapper;
using AutoMapper.QueryableExtensions;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Game.Models;
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

        public ServiceStatusAppService(IMapper mapper, IEventStoreRepository eventStoreRepository, IRepository<ServiceStatus> repository, IMediatorHandler bus)
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
            var serviceStates = from r in _repository.GetAll() group r by r.Service into g select g.OrderByDescending(e => e.TimeStamp).FirstOrDefault();
            var serviceStatusViewModels = _mapper.ProjectTo<ServiceStatusViewModel>(serviceStates);
            return serviceStatusViewModels.AsEnumerable();
        }

        public IEnumerable<ServiceStatusViewModel> GetRecentPublic()
        {
            //TODO: For Server Status on Main Page
            throw new NotImplementedException();
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
