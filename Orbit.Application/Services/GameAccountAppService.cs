using AutoMapper;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.GameAccount.Commands;
using Orbit.Infra.Persistence.Repository.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using X.PagedList;

namespace Orbit.Application.Services
{
    public class GameAccountAppService : IGameAccountAppService
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IRepository<Orbit.Domain.Game.Models.GameAccount> _repository;
        private readonly IMediatorHandler _bus;

        public GameAccountAppService(IMapper mapper, IRepository<Orbit.Domain.Game.Models.GameAccount> repository, IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _mapper = mapper;
            _repository = repository;
            _eventStoreRepository = eventStoreRepository;
            _bus = bus;
        }

        public void Create(GameAccountViewModel gameAccountViewModel)
        {
            var createCommand = _mapper.Map<CreateGameAccountCommand>(gameAccountViewModel);
            _bus.SendCommand(createCommand);
        }

        public IPagedList<GameAccountViewModel> GetAll(Guid userId,bool onlyOwned, out int recordCount, int pageIndex = 0, int recordsPerPage = 10)
        {
            var gameAccounts = _repository.GetAll().Where(e => !onlyOwned || e.UserID == userId );
            var gameAccountViewModels = _mapper.ProjectTo<GameAccountViewModel>(gameAccounts);
            recordCount = gameAccountViewModels.Count();
            return new StaticPagedList<GameAccountViewModel>(gameAccountViewModels, pageIndex + 1, recordsPerPage, recordCount);
        }

        public void Update(GameAccountViewModel gameAccountViewModel)
        {
            var updateCommand = _mapper.Map<UpdateGameAccountCommand>(gameAccountViewModel);
            _bus.SendCommand(updateCommand);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
