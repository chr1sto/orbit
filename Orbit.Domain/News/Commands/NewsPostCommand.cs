using Orbit.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.News.Commands
{
    public abstract class NewsPostCommand : Command
    {
        public Guid Id { get; protected set; }
        public string Caption { get; protected set; }
        public string Content { get; protected set; }
        public string ImageUrlSmallTile { get; protected set; }
        public string ImageUrlBigTile { get; protected set; }
        public string ImageUrlBanner { get; protected set; }
        public string ForumPostUrl { get; protected set; }
        public string Tags { get; protected set; }
        public Guid UserId { get; protected set; }
        public bool Public { get; protected set; }
    }
}
