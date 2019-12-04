using AutoMapper;
using Newtonsoft.Json.Linq;
using Orbit.Application.Interfaces;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.PlayerLog.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.Services
{
    public class PlayerLogAppService : IPlayerLogAppService
    {

        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;

        public PlayerLogAppService(IMapper mapper, IMediatorHandler bus)
        {
            _mapper = mapper;
            _bus = bus;
        }

        public void Add(JObject @object)
        {
            var cmd = _mapper.Map<CreatePlayerLogCommand>(@object);
            _bus.SendCommand(cmd);
        }
    }
}
