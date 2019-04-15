using Orbit.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.News
{
    public class NewsPost : Entity
    {
        public NewsPost(Guid id, string caption, string content, string imageUrlSmallTile, string imageUrlBigTile, string imageUrlBanner, string forumPostUrl, bool @public, DateTime createdOn, Guid createdBy, string tags)
        {
            Id = id;
            Caption = caption;
            Content = content;
            ImageUrlSmallTile = imageUrlSmallTile;
            ImageUrlBigTile = imageUrlBigTile;
            ImageUrlBanner = imageUrlBanner;
            ForumPostUrl = forumPostUrl;
            Public = @public;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            Tags = tags;
        }

        protected NewsPost() { }

        public string Caption { get; private set; }
        public string Content { get; private set; }
        public string ImageUrlSmallTile { get; private set; }
        public string ImageUrlBigTile { get; private set; }
        public string ImageUrlBanner { get; private set; }
        public string ForumPostUrl { get; private set; }
        public string Tags { get; private set; }
        public bool Public { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public Guid CreatedBy { get; private set; }
    }
}
