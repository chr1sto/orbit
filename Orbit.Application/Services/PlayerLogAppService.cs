using AutoMapper;
using Newtonsoft.Json.Linq;
using Orbit.Application.Interfaces;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.PlayerLog;
using Orbit.Domain.PlayerLog.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbit.Application.Services
{
    public class PlayerLogAppService : IPlayerLogAppService
    {

        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        private readonly IRepository<PlayerLog> _repository;

        public PlayerLogAppService(IMapper mapper, IMediatorHandler bus, IRepository<PlayerLog> repository)
        {
            _mapper = mapper;
            _bus = bus;
            _repository = repository;
        }

        public void Add(JObject @object)
        {
            var cmd = _mapper.Map<CreatePlayerLogCommand>(@object);
            _bus.SendCommand(cmd);
        }

        public IEnumerable<PlayerLog> GetByUser(Guid userId, out int recordCount, int pageIndex = 0, int recordsPerPage = 10)
        {
            var query = _repository.GetAll().Where(x => x.UserId == userId).OrderByDescending(x => x.TimeStamp);
            recordCount = query.Count();
            return query.Skip(pageIndex * recordsPerPage).Take(recordsPerPage).ToList();
        }
    }
}
