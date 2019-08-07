using MediatR;
using Orbit.Domain.CommandHandlers;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Domain.Transaction.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Domain.Transaction.CommandHandlers
{
    public class TransactionCommandHandler : CommandHandler, IRequestHandler<CreateTransactionCommand, bool>
    {
        private IRepository<Transaction> _repository;

        public TransactionCommandHandler(IRepository<Transaction> repository,IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _repository = repository;
        }

        public Task<bool> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }

            var transaction = new Transaction(request.Id, request.UserId, request.Date, request.Amount, request.Currency, request.IpAddress, request.RemoteAddress, request.Reason);

            _repository.Add(transaction);

            if(Commit())
            {
                //EVENTS?
            }

            return Task.FromResult(true);
        }
    }
}
