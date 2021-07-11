using AutoMapper;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Game.Models;
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
        private readonly IRepository<GameAccount> _gameAccountRepo;
        private readonly IMediatorHandler _bus;

        public GameCharacterAppService(IMapper mapper, IRepository<Character> repository, IRepository<GameAccount> gameAccountRepo, IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _mapper = mapper;
            _repository = repository;
            _gameAccountRepo = gameAccountRepo;
            _eventStoreRepository = eventStoreRepository;
            _bus = bus;
        }

        public void ClearUntilRecent()
        {
            //TODO
            throw new NotImplementedException();
        }

        public IEnumerable<CharacterAdminViewModel> GetAllByGameAccount(string account, bool includeDeleted,out int total, int index = 0, int count = 10, string searchText = "")
        {
            IQueryable<Character> query;
            if(!string.IsNullOrWhiteSpace(searchText))
            {
                query = _repository.GetAll().Where(c => c.Account == account && c.Name.ToUpper().Contains(searchText.ToUpper()));
            }
            else
            {
                query = _repository.GetAll().Where(c => c.Account == account);
            }

            total = query.Count();

            if(!includeDeleted)
            {
                query = query.Where(c => c.IsDeleted == false);
            }

            query = query.Skip(index * count).Take(count);

            //Hopefully this works again...
            return _mapper.ProjectTo<CharacterAdminViewModel>(query).AsEnumerable();
        }

        public IEnumerable<CharacterAdminViewModel> GetCurrent(out int total, int index = 0, int count = 10, string searchText = "")
        {
            /*
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
            */

            IQueryable<Character> query;
            if (string.IsNullOrWhiteSpace(searchText))
            {
                query = _repository.GetAll();
            }
            else
            {
                query = _repository.GetAll().Where(c => c.Name.ToUpper().Contains(searchText.ToUpper()));
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

            Func<Character, bool> func2;
            if(filterJob == null)
            {
                func2 = x => true;
            }
            else
            {
                switch (filterJob.ToUpper())
                {
                    case "TEMPLAR":
                        func2 = x => x.Class == "32" || x.Class == "6" || x.Class == "16" || x.Class == "24" || x.Class == "1";
                        break;
                    case "SLAYER":
                        func2 = x => x.Class == "33" || x.Class == "7" || x.Class == "17" || x.Class == "25" || x.Class == "1";
                        break;
                    case "HARLEQUIN":
                        func2 = x => x.Class == "34" || x.Class == "8" || x.Class == "18" || x.Class == "26" || x.Class == "2";
                        break;
                    case "CRACKSHOOTER":
                        func2 = x => x.Class == "35" || x.Class == "9" || x.Class == "19" || x.Class == "27" || x.Class == "2";
                        break;
                    case "SERAPH":
                        func2 = x => x.Class == "36" || x.Class == "10" || x.Class == "20" || x.Class == "28" || x.Class == "3";
                        break;
                    case "FORCEMASTER":
                        func2 = x => x.Class == "37" || x.Class == "11" || x.Class == "21" || x.Class == "29" || x.Class == "3";
                        break;
                    case "MENTALIST":
                        func2 = x => x.Class == "38" || x.Class == "12" || x.Class == "22" || x.Class == "30" || x.Class == "4";
                        break;
                    case "ARCANIST":
                        func2 = x => x.Class == "39" || x.Class == "13" || x.Class == "23" || x.Class == "31" || x.Class == "4";
                        break;
                    default:
                        func2 = x => true;
                        break;

                }
            }
            

            var query = _repository.GetAll().Where(e => !e.IsStaff && !e.IsDeleted).Where(func2).OrderByDescending(func);

            total = query.Count();

            //TODO AUTOMAPPER
            var result = query.Skip(index * count).Take(count).Select(e => new CharacterViewModel() {
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

        public Guid GetWebIdFromCharName(string charName)
        {
            var character = _repository.GetAll().Where(x => x.Name == charName && x.IsDeleted == false).FirstOrDefault();
            if (character == null) return Guid.Empty;
            var gameAccount = _gameAccountRepo.GetAll().Where(x => x.Account == character.Account).FirstOrDefault();
            if(gameAccount == null) return Guid.Empty;
            return gameAccount.UserID;
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
