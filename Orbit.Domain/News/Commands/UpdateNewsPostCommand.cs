using Orbit.Domain.News.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.News.Commands
{
    public class UpdateNewsPostCommand : NewsPostCommand
    {
        public UpdateNewsPostCommand(Guid id, string caption, string content, string imageUrlSmallTile, string imageUrlBigTile, string imageUrlBanner, string forumPostUrl, string tags, Guid userId, bool publish)
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
            Public = publish;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateNewsPostValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
