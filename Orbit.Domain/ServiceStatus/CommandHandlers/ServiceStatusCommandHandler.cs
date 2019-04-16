using MediatR;
using Orbit.Domain.CommandHandlers;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Domain.ServiceStatus.Commands;
using Orbit.Domain.ServiceStatus.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Domain.ServiceStatus.CommandHandlers
{
    public class ServiceStatusCommandHandler : CommandHandler, IRequestHandler<CreateServiceStatusCommand, bool>, IRequestHandler<UpdateServiceStatusCommand, bool>, IRequestHandler<RemoveServiceStatusCommand, bool>
    {
        private readonly IRepository<Orbit.Domain.Game.Models.ServiceStatus> _repository;
        private readonly IMediatorHandler _bus;
        private readonly INotificationHandler<DomainNotification> _notifications;

        public ServiceStatusCommandHandler(IRepository<Orbit.Domain.Game.Models.ServiceStatus> repository,IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _repository = repository;
            _bus = bus;
            _notifications = notifications;
        }

        public Task<bool> Handle(CreateServiceStatusCommand message, CancellationToken cancellationToken)
        {
            if(!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var serviceStatus = new Orbit.Domain.Game.Models.ServiceStatus(message.Id,message.State,message.TimeStamp,message.Service);

            _repository.Add(serviceStatus);

            if(Commit())
            {
                _bus.RaiseEvent(new ServiceStatusCreatedEvent(serviceStatus.Id,serviceStatus.State,serviceStatus.TimeStamp,serviceStatus.Service));
            }
            Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateServiceStatusCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var existingServiceStatus = _repository.GetById(message.Id);

            if(existingServiceStatus == null)
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "The corresponding ServiceStatus does not exist anymore!"));
                return Task.FromResult(false);
            }

            var serviceStatus = new Orbit.Domain.Game.Models.ServiceStatus(message.Id, message.State, message.TimeStamp, message.Service);

            _repository.Update(serviceStatus);

            if(Commit())
            {
                _bus.RaiseEvent(new ServiceStatusUpdatedEvent(message.Id, message.State, message.TimeStamp, message.Service));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveServiceStatusCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var existingServiceStatus = _repository.GetById(message.Id);

            if (existingServiceStatus == null)
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "The corresponding ServiceStatus does not exist anymore!"));
                return Task.FromResult(false);
            }

            _repository.Remove(message.Id);

            if(Commit())
            {
                _bus.RaiseEvent(new ServiceStatusRemovedEvent(message.Id, existingServiceStatus.State, existingServiceStatus.TimeStamp, existingServiceStatus.Service));
            }
            return Task.FromResult(true);
        }
    }
}
