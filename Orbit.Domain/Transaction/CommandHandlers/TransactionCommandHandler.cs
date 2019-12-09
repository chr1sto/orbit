using MediatR;
using Orbit.Domain.CommandHandlers;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Domain.Transaction.Commands;
using Orbit.Domain.Game;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Domain.Transaction.CommandHandlers
{
    public class TransactionCommandHandler : CommandHandler, IRequestHandler<CreateTransactionCommand, bool>, IRequestHandler<UpdateTransactionCommand,bool>
    {
        private IRepository<Orbit.Domain.Game.Transaction> _repository;
        private readonly IMediatorHandler _bus;

        public TransactionCommandHandler(IRepository<Orbit.Domain.Game.Transaction> repository,IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _repository = repository;
            _bus = bus;
        }

        public Task<bool> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }

            var transaction = new Orbit.Domain.Game.Transaction(request.Id, request.UserId, request.Date, request.Amount, request.Currency, request.IpAddress, request.RemoteAddress, request.Reason, request.Target, request.TargetInfo, request.Status, request.AdditionalInfo);

            _repository.Add(transaction);

            if(Commit())
            {
                //EVENTS?
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }

            var transaction = _repository.GetById(request.Id);

            if(transaction == null)
            {
                _bus.RaiseEvent(new DomainNotification(request.MessageType, "The corresponding Transaction does not exist!"));
                return Task.FromResult(false);
            }

            //The only thing that should be updated is the Status. As Transactions may NOT be changed.
            transaction.Status = request.Status;

            _repository.Update(transaction);

            if(Commit())
            {

            }

            return Task.FromResult(true);
        }
    }
}
