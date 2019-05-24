using MediatR;
using Orbit.Domain.CommandHandlers;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Domain.GameCharacter.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Domain.GameCharacter.CommandHandlers
{
    public class CharacterCommandHandler : CommandHandler, IRequestHandler<CreateCharacterCommand, bool>, IRequestHandler<RemoveCharacterCommand, bool>
    {
        private readonly IRepository<Character> _repository;
        private readonly IMediatorHandler _bus;
        private readonly IUser _user;

        public CharacterCommandHandler(IUser user, IRepository<Character> repository,IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _repository = repository;
            _bus = bus;
            _user = user;
        }

        public Task<bool> Handle(CreateCharacterCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }

            var @char = new Character(
                request.Id,
                request.UpdateId,
                request.UpdatedOn,
                request.IsStaff,
                request.PlayerId,
                request.Account,
                request.Name,
                request.Class,
                request.GearScore,
                request.Level,
                request.PlayTime,
                request.CreatedOn,
                request.Strength,
                request.Dexterity,
                request.Stamina,
                request.Intelligence,
                request.Perin,
                request.RedChips,
                request.EuphresiaCoins,
                request.VotePoints,
                request.DonateCoins,
                request.BossKills
                );

            _repository.Add(@char);

            Commit();

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveCharacterCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }

            var exists = _repository.Exists(request.Id);

            if (!exists)
            {
                _bus.RaiseEvent(new DomainNotification(request.MessageType, "The corresponding Character Entry does not exist anymore!"));
                return Task.FromResult(false);
            }

            _repository.Remove(request.Id);

            Commit();

            return Task.FromResult(true);
        }
    }
}
