using MediatR;
using Orbit.Domain.CommandHandlers;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Domain.Game.Events;
using Orbit.Domain.GameAccount.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Domain.GameAccount.CommandHandlers
{
    public class GameAccountCommandHandler : CommandHandler, IRequestHandler<CreateGameAccountCommand,bool>,IRequestHandler<UpdateGameAccountCommand,bool>
    {
        private readonly IRepository<Orbit.Domain.Game.Models.GameAccount> _repository;
        private readonly IMediatorHandler _bus;
        private readonly IUser _user;

        public GameAccountCommandHandler(IUser user, IRepository<Orbit.Domain.Game.Models.GameAccount> repository,IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _repository = repository;
            _bus = bus;
            _user = user;
        }

        public Task<bool> Handle(CreateGameAccountCommand message, CancellationToken cancellationToken)
        {
            if(!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            string accountId = Guid.NewGuid().ToString().Replace("-", string.Empty).ToLower();
            var gameAccount = new Orbit.Domain.Game.Models.GameAccount(Guid.NewGuid(), _user.Id, accountId, message.Alias,message.Server);

            _repository.Add(gameAccount);

            if (Commit())
            {
                _bus.RaiseEvent(new GameAccountCreatedEvent(gameAccount.Id, gameAccount.UserID, gameAccount.Account, gameAccount.Alias, message.Server));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateGameAccountCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var existingGameAccount = _repository.GetById(message.ID);

            if(existingGameAccount == null)
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "The corresponding GameAccount does not exist!"));
                return Task.FromResult(false);
            }

            if(existingGameAccount.UserID != _user.Id)
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "You have no permission to perform this action."));
                return Task.FromResult(false);
            }

            existingGameAccount.Alias = message.Alias;
            _repository.Update(existingGameAccount);

            if(Commit())
            {
                _bus.RaiseEvent(new GameAccountUpdatedEvent(existingGameAccount.Id, existingGameAccount.UserID, existingGameAccount.Account, existingGameAccount.Alias));
            }

            return Task.FromResult(true);
        }
    }
}
