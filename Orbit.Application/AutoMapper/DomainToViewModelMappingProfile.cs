using AutoMapper;
using Orbit.Application.ViewModels;
using Orbit.Domain.Game.Enums;
using Orbit.Domain.GameCharacter;
using Orbit.Domain.GameGuild;
using Orbit.Domain.Generic;
using Orbit.Domain.News;
using Orbit.Domain.PlayerLog;
using Orbit.Domain.Statistics;
using Orbit.Domain.Transaction;
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
            CreateMap<int, EServiceState>()
                .ConstructUsing(e => (EServiceState)e);
            CreateMap<EServiceState, int>()
                .ConstructUsing(e => (int)e);
            CreateMap<Domain.Game.Models.ServiceStatus, ServiceStatusViewModel>();
            //.ConstructUsing(c => new ServiceStatusViewModel(c.Id, c.Service, c.TimeStamp, (int)c.State));
            CreateMap<GenericObject, GenericObjectViewModel>();
            CreateMap<Character, CharacterAdminViewModel>();
            CreateMap<Character, CharacterViewModel>();
            CreateMap<StatisticsEntry, StatisticsEntryViewModel>();
            CreateMap<Orbit.Domain.Game.Transaction, TransactionViewModel>();
            CreateMap<Orbit.Domain.Game.Transaction, TransactionAdminViewModel>();
            CreateMap<Guild, GuildViewModel>();
        }
    }
}
