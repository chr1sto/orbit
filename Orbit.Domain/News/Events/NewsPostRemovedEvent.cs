using Orbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.News.Events
{
    public class NewsPostRemovedEvent : Event
    {
        public NewsPostRemovedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
