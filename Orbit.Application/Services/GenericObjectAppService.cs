using AutoMapper;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Generic;
using Orbit.Domain.Generic.Commands;
using Orbit.Infra.Persistence.Repository.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbit.Application.Services
{
    public class GenericObjectAppService : IGenericObjectAppService
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IRepository<GenericObject> _repository;
        private readonly IMediatorHandler _bus;

        public GenericObjectAppService(IMapper mapper, IRepository<GenericObject> repository, IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _mapper = mapper;
            _repository = repository;
            _eventStoreRepository = eventStoreRepository;
            _bus = bus;
        }

        public void Create(GenericObjectViewModel model)
        {
            var createCommand = _mapper.Map<CreateGenericObjectCommand>(model);
            _bus.SendCommand(createCommand);
        }

        public IEnumerable<GenericObjectViewModel> GetAll(string type, int amount = 5, bool mostRecent = true)
        {
            var result = _repository.GetAll()
                .Where(e => e.Visible && e.Type == type)
                .OrderBy(o => o.CreatedOn)
                .Take(amount);

            return _mapper.ProjectTo<GenericObjectViewModel>(result).AsEnumerable();
        }

        public GenericObjectViewModel GetSingle(Guid id)
        {
            var @object = _repository.GetById(id);
            if (@object == null) return null;

            return _mapper.Map<GenericObjectViewModel>(@object);
        }

        public void Remove(Guid id)
        {
            var removeCommand = _mapper.Map<RemoveGenericObjectCommand>(id);
            _bus.SendCommand(removeCommand);
        }

        public void Update(GenericObjectViewModel model)
        {
            var updateCommand = _mapper.Map<UpdateGenericObjectCommand>(model);
            _bus.SendCommand(updateCommand);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
