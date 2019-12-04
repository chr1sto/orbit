using AutoMapper;
using Orbit.Application.ViewModels;
using Orbit.Domain.Game.Enums;
using Orbit.Domain.GameAccount.Commands;
using Orbit.Domain.GameCharacter.Commands;
using Orbit.Domain.Generic.Commands;
using Orbit.Domain.News.Commands;
using Orbit.Domain.PlayerLog.Commands;
using Orbit.Domain.ServiceStatus.Commands;
using Orbit.Domain.Statistics.Commands;
using Orbit.Domain.Transaction;
using Orbit.Domain.Transaction.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            //NEWS
            CreateMap<NewsPostViewModel, CreateNewsPostCommand>()
                .ConstructUsing(c => new CreateNewsPostCommand(
                        c.Caption,
                        c.Content,
                        c.ImageUrlSmallTile,
                        c.ImageUrlBigTile,
                        c.ImageUrlBanner,
                        c.ForumPostUrl,
                        c.Tags,
                        Guid.Empty
                    ));
            CreateMap<NewsPostViewModel, UpdateNewsPostCommand>()
                .ConstructUsing(c => new UpdateNewsPostCommand(
                        c.Id,
                        c.Caption,
                        c.Content,
                        c.ImageUrlSmallTile,
                        c.ImageUrlBigTile,
                        c.ImageUrlBanner,
                        c.ForumPostUrl,
                        c.Tags,
                        Guid.Empty,
                        c.Public
                    ));
            CreateMap<Guid, RemoveNewsPostCommand>()
                .ConstructUsing(c => new RemoveNewsPostCommand(
                        c
                    ));

            //GameAccount
            CreateMap<GameAccountViewModel, CreateGameAccountCommand>()
                .ConstructUsing(c => new CreateGameAccountCommand(Guid.NewGuid(), c.Account, c.Alias));
            CreateMap<GameAccountViewModel, UpdateGameAccountCommand>()
                .ConstructUsing(c => new UpdateGameAccountCommand(c.Id,c.Account, c.Alias));

            //ServiceStatus
            CreateMap<ServiceStatusViewModel, CreateServiceStatusCommand>()
                .ConstructUsing(c => new CreateServiceStatusCommand(c.Service, (EServiceState)c.State));
            CreateMap<ServiceStatusViewModel, UpdateServiceStatusCommand>()
                .ConstructUsing(c => new UpdateServiceStatusCommand(c.Id,c.Service, (EServiceState)c.State));
            CreateMap<ServiceStatusViewModel, RemoveServiceStatusCommand>()
                .ConstructUsing(c => new RemoveServiceStatusCommand(c.Id));

            //GenericObject
            CreateMap<GenericObjectViewModel, CreateGenericObjectCommand>()
                .ConstructUsing(c => new CreateGenericObjectCommand(c.CreatedOn, c.Type, c.ValueType, c.Value, true));
            CreateMap<GenericObjectViewModel, UpdateGenericObjectCommand>()
                .ConstructUsing(c => new UpdateGenericObjectCommand(c.Id, c.CreatedOn, c.Type, c.ValueType, c.Value, true));
            CreateMap<Guid, RemoveGenericObjectCommand>()
                .ConstructUsing(c => new RemoveGenericObjectCommand(c));

            //Character
            CreateMap<CharacterAdminViewModel, CreateCharacterCommand>()
                .ConstructUsing(c => new CreateCharacterCommand(
                                    c.UpdatedOn,
                                    c.UpdateId,
                                    c.IsStaff,
                                    c.PlayerId,
                                    c.Account,
                                    c.Name,
                                    c.Class,
                                    c.GearScore,
                                    c.Level,
                                    c.PlayTime,
                                    c.CreatedOn,
                                    c.Strength,
                                    c.Dexterity,
                                    c.Stamina,
                                    c.Intelligence,
                                    c.Perin,
                                    c.Penya,
                                    c.RedChips,
                                    c.EuphresiaCoins,
                                    c.VotePoints,
                                    c.DonateCoins,
                                    c.BossKills,
                                    c.IsDeleted
                            ));

            CreateMap<Guid, RemoveCharacterCommand>()
                .ConstructUsing(c => new RemoveCharacterCommand(c));

            //Statistics
            CreateMap<StatisticsEntryViewModel, CreateStatisticsEntryCommand>()
                .ConstructUsing(c => new CreateStatisticsEntryCommand(c.Start, c.End, c.StatGroup, c.StatName, c.ValueType, c.Value));

            CreateMap<Orbit.Domain.Game.Transaction, CreateTransactionCommand>()
                .ConstructUsing(c => new CreateTransactionCommand(Guid.NewGuid(), c.UserId, c.Date, c.Amount, c.Currency, c.IpAddress, c.RemoteAddress, c.Reason, c.Target,c.TargetInfo,c.Status));

            CreateMap<Orbit.Domain.Game.Transaction, UpdateTransactionCommand>()
                .ConstructUsing(c => new UpdateTransactionCommand(c.Id, c.UserId, c.Date, c.Amount, c.Currency, c.IpAddress, c.RemoteAddress, c.Reason, c.Target, c.TargetInfo, c.Status));

            CreateMap<TransactionViewModel, UpdateTransactionCommand>()
                .ConstructUsing(c => new UpdateTransactionCommand(c.Id, Guid.NewGuid(), c.Date, c.Amount, c.Currency, "", "", c.Reason, c.Target, c.TargetInfo, c.Status));

            CreateMap<Newtonsoft.Json.Linq.JObject, CreatePlayerLogCommand>()
                .ConstructUsing(c => new CreatePlayerLogCommand(Newtonsoft.Json.JsonConvert.SerializeObject(c)));
        }
    }
}
