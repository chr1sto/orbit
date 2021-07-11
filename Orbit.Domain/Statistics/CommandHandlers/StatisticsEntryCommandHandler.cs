using MediatR;
using Orbit.Domain.CommandHandlers;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Domain.Statistics.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Domain.Statistics.CommandHandlers
{
    public class StatisticsEntryCommandHandler : CommandHandler, IRequestHandler<CreateStatisticsEntryCommand, bool>
    {
        private readonly IRepository<StatisticsEntry> _repository;
        private readonly IMediatorHandler _bus;
        private readonly IUser _user;

        public StatisticsEntryCommandHandler(IUser user, IRepository<StatisticsEntry> repository,IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _repository = repository;
            _bus = bus;
            _user = user;
        }

        public Task<bool> Handle(CreateStatisticsEntryCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }

            var statisticsEntry = new StatisticsEntry(
                request.Id,
                request.Start,
                request.End,
                request.StatGroup,
                request.StatName,
                request.ValueType,
                request.Value
                );

            _repository.Add(statisticsEntry);

            Commit();

            return Task.FromResult(true);
        }
    }
}
