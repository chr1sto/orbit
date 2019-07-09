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

        public IEnumerable<CharacterAdminViewModel> GetAllByGameAccount(string account, out int total, int index = 0, int count = 10, string searchText = "")
        {
            IQueryable<Character> query;
            if(!string.IsNullOrWhiteSpace(searchText))
            {
                query = from c in _repository.GetAll().Where(c => c.Account == account && c.Name.ToUpper().Contains(searchText.ToUpper()))
                        group c by c.PlayerId
                        into g
                        select g.OrderByDescending(e => e.UpdatedOn).FirstOrDefault();
            }
            else
            {
                query = from c in _repository.GetAll().Where(c => c.Account == account)
                        group c by c.PlayerId
                        into g
                        select g.OrderByDescending(e => e.UpdatedOn).FirstOrDefault();
            }

            total = query.Count();

            query = query.Skip(index * count).Take(count);

            //Hopefully this works again...
            return _mapper.ProjectTo<CharacterAdminViewModel>(query).AsEnumerable();
        }

        public IEnumerable<CharacterAdminViewModel> GetCurrent(out int total, int index = 0, int count = 10, string searchText = "")
        {
            IQueryable<Character> query;
            if (string.IsNullOrWhiteSpace(searchText))
            {
                query = from c in _repository.GetAll()
                            group c by c.PlayerId
                            into g
                            select g.OrderByDescending(e => e.UpdatedOn).FirstOrDefault();
            }
            else
            {
                query = from c in _repository.GetAll().Where(c => c.Name.ToUpper().Contains(searchText.ToUpper()))
                            group c by c.PlayerId
                            into g
                            select g.OrderByDescending(e => e.UpdatedOn).FirstOrDefault();
            }

            total = query.Count();

            query = query.Skip(index * count).Take(count);

            return _mapper.ProjectTo<CharacterAdminViewModel>(query).AsEnumerable();
        }

        public IEnumerable<CharacterViewModel> GetRanking(out int total, int index = 0, int count = 10, string orderBy = "", string filterJob = "")
        {
            Func<Character, int> func;
            switch (orderBy.ToUpper())
            {
                case "LEVEL":
                    func = x => x.Level;
                    break;
                case "BOSSKILLS":
                    func = x => x.BossKills;
                    break;
                case "GEARSCORE":
                default:
                    func = x => x.GearScore;
                    break;
            }

            IQueryable<Character> query =
                from c in _repository.GetAll()
                group c by c.PlayerId
                into g
                select g.OrderByDescending(e => e.UpdatedOn).FirstOrDefault();

            var ordered = query.Where(e => !e.IsStaff && !e.IsDeleted).OrderByDescending(func);
            //TODO: filter by job

            total = ordered.Count();

            var result = ordered.Skip(index * count).Take(count).Select(e => new CharacterViewModel() {
                BossKills = e.BossKills,
                Class = e.Class,
                Dexterity = e.Dexterity,
                GearScore = e.GearScore,
                Intelligence = e.Intelligence,
                Level = e.Level,
                Name = e.Name,
                PlayTime = e.PlayTime,
                Stamina = e.Stamina,
                Strength = e.Strength
            } );

            return result;
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
