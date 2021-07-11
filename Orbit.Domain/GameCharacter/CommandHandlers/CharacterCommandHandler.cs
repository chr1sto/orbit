using MediatR;
using Orbit.Domain.CommandHandlers;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Domain.GameCharacter.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var @char = _repository.GetAll().Where(e => e.PlayerId == request.PlayerId).FirstOrDefault();

            if(@char != null)
            {
                @char.UpdateId = request.UpdateId;
                @char.UpdatedOn =  request.UpdatedOn;
                @char.IsStaff =  request.IsStaff;
                @char.PlayerId =  request.PlayerId;
                @char.Account =  request.Account;
                @char.Name =  request.Name;
                @char.Class =  request.Class;
                @char.GearScore =  request.GearScore;
                @char.Level =  request.Level;
                @char.PlayTime =  request.PlayTime;
                @char.CreatedOn =  request.CreatedOn;
                @char.Strength =  request.Strength;
                @char.Dexterity =  request.Dexterity;
                @char.Stamina =  request.Stamina;
                @char.Intelligence =  request.Intelligence;
                @char.Perin =  request.Perin;
                @char.Penya =  request.Penya;
                @char.RedChips =  request.RedChips;
                @char.EuphresiaCoins =  request.EuphresiaCoins;
                @char.VotePoints =  request.VotePoints;
                @char.DonateCoins =  request.DonateCoins;
                @char.BossKills =  request.BossKills;
                @char.IsDeleted =  request.IsDeleted;

                _repository.Update(@char);
            }
            else
            {
                @char = new Character(
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
                request.Penya,
                request.RedChips,
                request.EuphresiaCoins,
                request.VotePoints,
                request.DonateCoins,
                request.BossKills,
                request.IsDeleted
                );

                _repository.Add(@char);
            }

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
