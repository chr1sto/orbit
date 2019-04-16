using AutoMapper;
using Orbit.Application.ViewModels;
using Orbit.Domain.GameAccount.Commands;
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
                .ConstructUsing(c => new CreateGameAccountCommand(c.Id, c.Account, c.Alias));
            CreateMap<GameAccountViewModel, UpdateGameAccountCommand>()
                .ConstructUsing(c => new UpdateGameAccountCommand(c.Id,c.Account, c.Alias));

            //ServiceStatus
            CreateMap<ServiceStatusViewModel, CreateServiceStatusCommand>()
                .ConstructUsing(c => new CreateServiceStatusCommand(c.Service, c.State));
            CreateMap<ServiceStatusViewModel, UpdateServiceStatusCommand>()
                .ConstructUsing(c => new UpdateServiceStatusCommand(c.Id,c.Service, c.State));
            CreateMap<ServiceStatusViewModel, RemoveServiceStatusCommand>()
                .ConstructUsing(c => new RemoveServiceStatusCommand(c.Id));
        }
    }
}
