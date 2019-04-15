using AutoMapper;
using Orbit.Application.ViewModels;
using Orbit.Domain.News;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<NewsPost, NewsPostViewModel>();
            CreateMap<Domain.Game.Models.GameAccount, GameAccountViewModel>();
        }
    }
}
