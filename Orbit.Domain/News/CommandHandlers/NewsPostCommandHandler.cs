using MediatR;
using Orbit.Domain.CommandHandlers;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Notifications;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.News.Commands;
using Orbit.Domain.News.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Domain.News.CommandHandlers
{
    public class NewsPostCommandHandler : CommandHandler, IRequestHandler<CreateNewsPostCommand, bool>, IRequestHandler<UpdateNewsPostCommand, bool>, IRequestHandler<RemoveNewsPostCommand, bool>
    {
        private readonly IRepository<NewsPost> _repository;
        private readonly IMediatorHandler _bus;
        private readonly IUser _user;

        public NewsPostCommandHandler(IUser user, IRepository<NewsPost> repository, IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _repository = repository;
            _bus = bus;
            _user = user;
        }

        public Task<bool> Handle(CreateNewsPostCommand message, CancellationToken cancellationToken)
        {
            if(!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var newsPost = new NewsPost(
                    Guid.NewGuid(),
                    message.Caption,
                    message.Content,
                    message.ImageUrlSmallTile,
                    message.ImageUrlBigTile,
                    message.ImageUrlBanner,
                    message.ForumPostUrl,
                    false,
                    DateTime.Now,
                    _user.Id,
                    message.Tags
                );

            _repository.Add(newsPost);

            if(Commit())
            {
                _bus.RaiseEvent(new NewsPostCreatedEvent(
                        newsPost.Id,
                        newsPost.Caption,
                        newsPost.Content,
                        newsPost.ImageUrlSmallTile,
                        newsPost.ImageUrlBigTile,
                        newsPost.ImageUrlBanner,
                        newsPost.ForumPostUrl,
                        newsPost.Tags,
                        newsPost.CreatedBy
                    ));
            }

            return Task.FromResult(true);
        }

        public async Task<bool> Handle(UpdateNewsPostCommand message, CancellationToken cancellationToken)
        {
            if(!message.IsValid())
            {
                NotifyValidationErrors(message);
                return false;
            }

            var newsPost = _repository.GetById(message.Id);

            if (newsPost == null)
            {
                return false;
            }

            newsPost.Caption = message.Caption;
            newsPost.Content = message.Content;
            newsPost.ForumPostUrl = message.ForumPostUrl;
            newsPost.ImageUrlBanner = message.ImageUrlBanner;
            newsPost.ImageUrlBigTile = message.ImageUrlBigTile;
            newsPost.ImageUrlSmallTile = message.ImageUrlSmallTile;
            newsPost.Public = message.Public;
            newsPost.Tags = message.Tags;

            _repository.Update(newsPost);

            if (Commit())
            {
                await _bus.RaiseEvent(new NewsPostUpdatedEvent(
                        newsPost.Id,
                        newsPost.Caption,
                        newsPost.Content,
                        newsPost.ImageUrlSmallTile,
                        newsPost.ImageUrlBigTile,
                        newsPost.ImageUrlBanner,
                        newsPost.ForumPostUrl,
                        newsPost.Tags,
                        newsPost.Public
                    ));
            }

            return true;
        }

        public Task<bool> Handle(RemoveNewsPostCommand message, CancellationToken cancellationToken)
        {
            if(!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var existingNewsPost = _repository.GetById(message.Id);

            if (existingNewsPost == null)
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "The corresponding NewsPost does not exist anymore!"));
                return Task.FromResult(false);
            }

            _repository.Remove(message.Id);

            if(Commit())
            {
                _bus.RaiseEvent(new NewsPostRemovedEvent(message.Id));
            }

            return Task.FromResult(true);
        }
    }
}
