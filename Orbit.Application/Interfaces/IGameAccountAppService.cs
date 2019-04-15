using Orbit.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace Orbit.Application.Interfaces
{
    public interface IGameAccountAppService
    {
        void Create(GameAccountViewModel gameAccountViewModel);
        void Update(GameAccountViewModel gameAccountViewModel);
        IPagedList<GameAccountViewModel> GetAll(Guid userId, bool onlyOwned, out int recordCount, int pageIndex = 0, int recordsPerPage = 10);
    }
}
