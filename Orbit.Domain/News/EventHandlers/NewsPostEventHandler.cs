using MediatR;
using Orbit.Domain.News.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Domain.News.EventHandlers
{
    public class NewsPostEventHandler : INotificationHandler<NewsPostCreatedEvent>, INotificationHandler<NewsPostUpdatedEvent>, INotificationHandler<NewsPostRemovedEvent>
    {
        public Task Handle(NewsPostCreatedEvent notification, CancellationToken cancellationToken)
        {
            //TODO: Handle Discord Posts

            return Task.CompletedTask;
        }

        public Task Handle(NewsPostUpdatedEvent notification, CancellationToken cancellationToken)
        {
            //TODO: Handle Discord Posts

            return Task.CompletedTask;
        }

        public Task Handle(NewsPostRemovedEvent notification, CancellationToken cancellationToken)
        {
            //TODO: Handle Discord Posts

            return Task.CompletedTask;
        }
    }
}
