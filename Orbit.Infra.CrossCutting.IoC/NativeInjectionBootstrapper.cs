using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Orbit.Application.Interfaces;
using Orbit.Application.Services;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Events;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Core.Notifications;
using Orbit.Domain.Game.Events;
using Orbit.Domain.GameAccount.CommandHandlers;
using Orbit.Domain.GameAccount.Commands;
using Orbit.Domain.GameAccount.EventHandlers;
using Orbit.Domain.GameCharacter;
using Orbit.Domain.GameCharacter.CommandHandlers;
using Orbit.Domain.GameCharacter.Commands;
using Orbit.Domain.Generic;
using Orbit.Domain.Generic.CommandHandlers;
using Orbit.Domain.Generic.Commands;
using Orbit.Domain.News;
using Orbit.Domain.News.CommandHandlers;
using Orbit.Domain.News.Commands;
using Orbit.Domain.News.EventHandlers;
using Orbit.Domain.News.Events;
using Orbit.Domain.PlayerLog;
using Orbit.Domain.PlayerLog.CommandHandlers;
using Orbit.Domain.PlayerLog.Commands;
using Orbit.Domain.ServiceStatus.CommandHandlers;
using Orbit.Domain.ServiceStatus.Commands;
using Orbit.Domain.ServiceStatus.EventHandlers;
using Orbit.Domain.ServiceStatus.Events;
using Orbit.Domain.Statistics;
using Orbit.Domain.Statistics.CommandHandlers;
using Orbit.Domain.Statistics.Commands;
using Orbit.Domain.Transaction;
using Orbit.Domain.Transaction.CommandHandlers;
using Orbit.Domain.Transaction.Commands;
using Orbit.Infra.CrossCutting.Bus;
using Orbit.Infra.CrossCutting.Identity.Authorization;
using Orbit.Infra.CrossCutting.Identity.Models;
using Orbit.Infra.CrossCutting.Identity.Services;
using Orbit.Infra.FileUpload.Interfaces;
using Orbit.Infra.FileUpload.Services;
using Orbit.Infra.Payments.PayPal.Interfaces;
using Orbit.Infra.Payments.PayPal.Services;
using Orbit.Infra.Persistence.Context;
using Orbit.Infra.Persistence.EventSourcing;
using Orbit.Infra.Persistence.Repository;
using Orbit.Infra.Persistence.Repository.EventSourcing;
using Orbit.Infra.Persistence.UoW;

namespace Orbit.Infra.CrossCutting.IoC
{
    public class NativeInjectionBootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddDbContext<OrbitContext>();

            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // ASP.NET Authorization Polices
            services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();

            // Application
            services.AddSingleton<IConcurrencyLockService, ConcurrencyLockService>();
            services.AddScoped<INewsAppService, NewsAppService>();
            services.AddScoped<IGameAccountAppService, GameAccountAppService>();
            services.AddScoped<IGameEventAppService, GameEventAppService>();
            services.AddScoped<IServiceStatusAppService, ServiceStatusAppService>();
            services.AddScoped<IGenericObjectAppService, GenericObjectAppService>();
            services.AddScoped<IStatisticsAppService, StatisticsAppService>();
            services.AddScoped<IGameCharacterAppService, GameCharacterAppService>();
            services.AddScoped<ITransactionAppService, TransactionAppService>();
            services.AddScoped<IPlayerLogAppService, PlayerLogAppService>();

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<NewsPostCreatedEvent>, NewsPostEventHandler>();
            services.AddScoped<INotificationHandler<NewsPostUpdatedEvent>, NewsPostEventHandler>();
            services.AddScoped<INotificationHandler<NewsPostRemovedEvent>, NewsPostEventHandler>();
            services.AddScoped<INotificationHandler<GameAccountCreatedEvent>, GameAccountEventHandler>();
            services.AddScoped<INotificationHandler<GameAccountUpdatedEvent>, GameAccountEventHandler>();
            services.AddScoped<INotificationHandler<ServiceStatusCreatedEvent>, ServiceStatusEventHandler>();
            services.AddScoped<INotificationHandler<ServiceStatusUpdatedEvent>, ServiceStatusEventHandler>();
            services.AddScoped<INotificationHandler<ServiceStatusRemovedEvent>, ServiceStatusEventHandler>();

            // Domain - Commands
            services.AddScoped<IRequestHandler<CreateNewsPostCommand, bool>, NewsPostCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateNewsPostCommand, bool>, NewsPostCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveNewsPostCommand, bool>, NewsPostCommandHandler>();
            services.AddScoped<IRequestHandler<CreateGameAccountCommand, bool>, GameAccountCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateGameAccountCommand, bool>, GameAccountCommandHandler>();
            services.AddScoped<IRequestHandler<CreateServiceStatusCommand, bool>, ServiceStatusCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateServiceStatusCommand, bool>, ServiceStatusCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveServiceStatusCommand, bool>, ServiceStatusCommandHandler>();
            services.AddScoped<IRequestHandler<CreateGenericObjectCommand, bool>, GenericObjectCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateGenericObjectCommand, bool>, GenericObjectCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveGenericObjectCommand, bool>, GenericObjectCommandHandler>();
            services.AddScoped<IRequestHandler<CreateStatisticsEntryCommand, bool>, StatisticsEntryCommandHandler>();
            services.AddScoped<IRequestHandler<CreateCharacterCommand, bool>, CharacterCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveCharacterCommand, bool>, CharacterCommandHandler>();
            services.AddScoped<IRequestHandler<CreateTransactionCommand, bool>, TransactionCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateTransactionCommand,bool>, TransactionCommandHandler>();
            services.AddScoped<IRequestHandler<CreatePlayerLogCommand, bool>, PlayerLogCommandHandlers>();

            // Infra - Data
            services.AddScoped<IRepository<NewsPost>,Repository<NewsPost>>();
            services.AddScoped<IRepository<Orbit.Domain.Game.Models.GameAccount>, Repository<Orbit.Domain.Game.Models.GameAccount>>();
            services.AddScoped<IRepository<Orbit.Domain.Game.Models.ServiceStatus>, Repository<Orbit.Domain.Game.Models.ServiceStatus>>();
            services.AddScoped<IRepository<GenericObject>, Repository<GenericObject>>();
            services.AddScoped<IRepository<StatisticsEntry>, Repository<StatisticsEntry>>();
            services.AddScoped<IRepository<Character>, Repository<Character>>();
            services.AddScoped<IRepository<Orbit.Domain.Game.Transaction>,Repository<Orbit.Domain.Game.Transaction>>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRepository<PlayerLog>, Repository<PlayerLog>>();

            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreRepository>();
            services.AddScoped<IEventStore, EventStore>();
            services.AddScoped<EventStoreContext>();

            // Infra - Identity Services
            services.AddTransient<IEmailSender, EmailSender>();

            // Infra - Identity
            services.AddScoped<IUser, AspNetUser>();

            // Infra - FileUpload
            services.AddSingleton<IFileUploadService, FileUploadService>();

            // Infra - PaymentProviders
            services.AddScoped<IPayPalService, PayPalService>();
        }

    }
}
