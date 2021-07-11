using MediatR;
using Orbit.Domain.ServiceStatus.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Domain.ServiceStatus.EventHandlers
{
    public class ServiceStatusEventHandler : INotificationHandler<ServiceStatusCreatedEvent>, INotificationHandler<ServiceStatusUpdatedEvent>, INotificationHandler<ServiceStatusRemovedEvent>
    {
        public Task Handle(ServiceStatusCreatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(ServiceStatusUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(ServiceStatusRemovedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
