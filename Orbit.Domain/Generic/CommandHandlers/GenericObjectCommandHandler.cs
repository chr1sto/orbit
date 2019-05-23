using MediatR;
using Orbit.Domain.CommandHandlers;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Domain.Generic.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Domain.Generic.CommandHandlers
{
    public class GenericObjectCommandHandler : CommandHandler, IRequestHandler<CreateGenericObjectCommand, bool>, IRequestHandler<UpdateGenericObjectCommand, bool>, IRequestHandler<RemoveGenericObjectCommand, bool>
    {
        private readonly IRepository<GenericObject> _repository;
        private readonly IMediatorHandler _bus;
        private readonly IUser _user;

        public GenericObjectCommandHandler(IUser user, IRepository<GenericObject> repository,IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _repository = repository;
            _bus = bus;
            _user = user;
        }

        public Task<bool> Handle(CreateGenericObjectCommand request, CancellationToken cancellationToken)
        {
            if(!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }

            var genericObj = new GenericObject(Guid.NewGuid(), request.CreatedOn, request.Type, request.ValueType, request.Value, request.Visible);

            _repository.Add(genericObj);

            if(Commit())
            {
                //Should there be Events?
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateGenericObjectCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }

            var exists = _repository.Exists(request.Id);

            if (!exists)
            {
                _bus.RaiseEvent(new DomainNotification(request.MessageType, "The corresponding Object does not exist anymore!"));
                return Task.FromResult(false);
            }

            var genericObj = new GenericObject(request.Id, request.CreatedOn, request.Type, request.ValueType, request.Value, request.Visible);

            if (Commit())
            {
                //Should there be Events?
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveGenericObjectCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }

            var exists = _repository.Exists(request.Id);

            if (!exists)
            {
                _bus.RaiseEvent(new DomainNotification(request.MessageType, "The corresponding Object does not exist anymore!"));
                return Task.FromResult(false);
            }

            _repository.Remove(request.Id);

            if (Commit())
            {
                //Should there be Events?
            }

            return Task.FromResult(true);
        }
    }
}
