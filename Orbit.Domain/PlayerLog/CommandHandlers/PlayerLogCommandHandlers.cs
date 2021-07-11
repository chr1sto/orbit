using MediatR;
using Orbit.Domain.CommandHandlers;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Domain.PlayerLog.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Domain.PlayerLog.CommandHandlers
{
    public class PlayerLogCommandHandlers : CommandHandler, IRequestHandler<CreatePlayerLogCommand,bool>
    {
        private readonly IRepository<PlayerLog> _repository;
        private readonly IUser _user;
        public PlayerLogCommandHandlers(IRepository<PlayerLog> repository, IUser user,IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _repository = repository;
            _user = user;
        }

        public Task<bool> Handle(CreatePlayerLogCommand request, CancellationToken cancellationToken)

        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }

            var logEntry = new PlayerLog(request.Id, _user.Id, request.TimeStamp, request.Info, request.IpAddress);

            _repository.Add(logEntry);
            _repository.SaveChanges();

            if(!Commit())
            {
                //Shit Happened
            }

            return Task.FromResult(true);
        }
    }
}
