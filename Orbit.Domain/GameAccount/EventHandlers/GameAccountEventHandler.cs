using MediatR;
using Orbit.Domain.Game.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Domain.GameAccount.EventHandlers
{
    public class GameAccountEventHandler : INotificationHandler<GameAccountCreatedEvent>, INotificationHandler<GameAccountUpdatedEvent>
    {
        public Task Handle(GameAccountCreatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(GameAccountUpdatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
