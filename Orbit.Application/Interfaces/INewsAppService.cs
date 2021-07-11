using Orbit.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Orbit.Application.Interfaces
{
    public interface INewsAppService
    {
        void Create(NewsPostViewModel model);
        void Update(NewsPostViewModel model);
        void Remove(Guid id);
        IEnumerable<NewsPostViewModel> GetAll(bool @public,out int recordCount, int pageIndex = 0, int recordsPerPage = 10);
        NewsPostViewModel GetSingle(Guid id);
    }
}
