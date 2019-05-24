using AutoMapper;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.GameCharacter;
using Orbit.Domain.GameCharacter.Commands;
using Orbit.Infra.Persistence.Repository.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbit.Application.Services
{
    public class GameCharacterAppService : IGameCharacterAppService
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IRepository<Character> _repository;
        private readonly IMediatorHandler _bus;

        public GameCharacterAppService(IMapper mapper, IRepository<Character> repository, IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _mapper = mapper;
            _repository = repository;
            _eventStoreRepository = eventStoreRepository;
            _bus = bus;
        }

        public void ClearUntilRecent()
        {
            //TODO
            throw new NotImplementedException();
        }

        public IEnumerable<CharacterAdminViewModel> GetAllByGameAccount(string account)
        {
            //var chars = _repository.GetAll().Where(c => c.Account == account);
            var chars = from c in _repository.GetAll().Where(c => c.Account == account)
                        group c by c.PlayerId
                        into g
                        select g.OrderByDescending(e => e.UpdatedOn).FirstOrDefault();

            //Hopefully this works again...
            return _mapper.ProjectTo<CharacterAdminViewModel>(chars).AsEnumerable();
        }

        public IEnumerable<CharacterAdminViewModel> GetCurrent()
        {
            var chars = from c in _repository.GetAll()
                        group c by c.PlayerId
                        into g
                        select g.OrderByDescending(e => e.UpdatedOn).FirstOrDefault();

            return _mapper.ProjectTo<CharacterAdminViewModel>(chars).AsEnumerable();
        }

        public void InsertNewEntries(IEnumerable<CharacterAdminViewModel> models)
        {
            foreach(var item in models)
            {
                var createCommand = _mapper.Map<CreateCharacterCommand>(item);
                _bus.SendCommand(createCommand);
            }
        }
    }
}
