using AutoMapper;
using Orbit.Application.ViewModels;
using Orbit.Domain.Game.Enums;
using Orbit.Domain.GameAccount.Commands;
using Orbit.Domain.Generic.Commands;
using Orbit.Domain.News.Commands;
using Orbit.Domain.ServiceStatus.Commands;
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
                .ConstructUsing(c => new UpdateGenericObjectCommand(c.Id, c.CreatedOn, c.Type, c.ValueType, c.Value, true);
            CreateMap<Guid, RemoveGenericObjectCommand>()
                .ConstructUsing(c => new RemoveGenericObjectCommand(c));
        }
    }
}
