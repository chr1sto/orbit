using AutoMapper;
using AutoMapper.QueryableExtensions;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.News;
using Orbit.Domain.News.Commands;
using Orbit.Infra.Persistence.Repository.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Orbit.Application.Services
{
    public class NewsAppService : INewsAppService
    {
        private readonly IMapper _mapper;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IRepository<NewsPost> _repository;
        private readonly IMediatorHandler _bus;

        public NewsAppService(IMapper mapper, IRepository<NewsPost> repository, IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _mapper = mapper;
            _repository = repository;
            _eventStoreRepository = eventStoreRepository;
            _bus = bus;
        }

        public void Create(NewsPostViewModel model)
        {
            var createCommand = _mapper.Map<CreateNewsPostCommand>(model);
            _bus.SendCommand(createCommand);
        }

        public void Remove(Guid id)
        {
            var removeCommand = _mapper.Map<RemoveNewsPostCommand>(id);
            _bus.SendCommand(removeCommand);
        }

        public void Update(NewsPostViewModel model)
        {
            var updateCommand = _mapper.Map<UpdateNewsPostCommand>(model);
            _bus.SendCommand(updateCommand);
        }

        public NewsPostViewModel GetSingle(Guid id)
        {
            var newsPost = _repository.GetById(id);
            if(newsPost == null)
            {
                return null;
            }

            var newsPostViewModel = _mapper.Map<NewsPostViewModel>(newsPost);
            return newsPostViewModel;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IEnumerable<NewsPostViewModel> GetAll(bool @public, out int recordCount, int pageIndex = 0, int recordsPerPage = 10)
        {
            var query = _repository.GetAll();
            recordCount = query.Where(e => e.Public == @public).Count();

            var newsPostViewModels = query.Where(e => e.Public == @public)
                .OrderBy(o => o.CreatedOn)
                .Skip(pageIndex * recordsPerPage)
                .Take(recordsPerPage);
            return _mapper.ProjectTo<NewsPostViewModel>(newsPostViewModels).AsEnumerable();
        }
    }
}
