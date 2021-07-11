using MediatR;
using Orbit.Domain.CommandHandlers;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Domain.GameCharacter;
using Orbit.Domain.GameGuild.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbit.Domain.GameGuild.CommandHandlers
{
    public class GuildCommandHandler : CommandHandler, IRequestHandler<CreateOrUpdateGuildCommand,bool>, IRequestHandler<RemoveGuildCommand,bool>
    {
        private readonly IRepository<Guild> _guildRepository;
        private readonly IRepository<Character> _characterRepository;

        public GuildCommandHandler(IRepository<Guild> guildRepository, IRepository<Character> characterRepository, IUnitOfWork uow, IMediatorHandler bus, INotificationHandler<DomainNotification> notifications) : base(uow, bus, notifications)
        {
            _guildRepository = guildRepository;
            _characterRepository = characterRepository;
        }

        public Task<bool> Handle(RemoveGuildCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }

            _guildRepository.Remove(request.Id);
            _guildRepository.SaveChanges();

            if(!Commit())
            {
                //TODO log some errors
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(CreateOrUpdateGuildCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return Task.FromResult(false);
            }

            var @char = _characterRepository.GetAll().Where(x => x.PlayerId == request.PlayerId).FirstOrDefault();

            var guild = this._guildRepository.GetAll().Where(x => x.GuildId == request.GuildId).FirstOrDefault();
            if(guild == null)
            {
                guild = new Guild(Guid.NewGuid(), request.Name, request.GuildId, request.GuildScore, request.AverageGearScore, request.TotalGearScore, request.Level, request.CreatedOn);
                _guildRepository.Add(guild);
            }
            else
            {
                guild.Name = request.Name;
                guild.AverageGearScore = request.AverageGearScore;
                guild.GuildScore = request.GuildScore;
                guild.Leader = @char;
                guild.Level = request.Level;
                guild.TotalGearScore = request.TotalGearScore;

                _guildRepository.Update(guild);
            }

            _guildRepository.SaveChanges();

            if(!Commit())
            {
                //TODO log some errors
            }

            return Task.FromResult(true);
        }
    }
}
