using Orbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.News.Events
{
    public class NewsPostCreatedEvent : Event
    {
        public NewsPostCreatedEvent(Guid id, string caption, string content, string imageUrlSmallTile, string imageUrlBigTile, string imageUrlBanner, string forumPostUrl, string tags, Guid userId)
        {
            Id = id;
            Caption = caption;
            Content = content;
            ImageUrlSmallTile = imageUrlSmallTile;
            ImageUrlBigTile = imageUrlBigTile;
            ImageUrlBanner = imageUrlBanner;
            ForumPostUrl = forumPostUrl;
            Tags = tags;
            UserId = userId;
        }

        public Guid Id { get; private set; }
        public string Caption { get; private set; }
        public string Content { get; private set; }
        public string ImageUrlSmallTile { get; private set; }
        public string ImageUrlBigTile { get; private set; }
        public string ImageUrlBanner { get; private set; }
        public string ForumPostUrl { get; private set; }
        public string Tags { get; private set; }
        public Guid UserId { get; private set; }
    }
}
