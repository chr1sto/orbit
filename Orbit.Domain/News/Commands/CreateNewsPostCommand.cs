using Orbit.Domain.News.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.News.Commands
{
    public class CreateNewsPostCommand : NewsPostCommand
    {
        public CreateNewsPostCommand(string caption, string content, string imageUrlSmallTile, string imageUrlBigTile, string imageUrlBanner, string forumPostUrl, string tags, Guid userId)
        {
            Caption = caption;
            Content = content;
            ImageUrlSmallTile = imageUrlSmallTile;
            ImageUrlBigTile = imageUrlBigTile;
            ImageUrlBanner = imageUrlBanner;
            ForumPostUrl = forumPostUrl;
            Tags = tags;
            UserId = userId;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateNewsPostValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
