using Orbit.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.Interfaces
{
    public interface IServiceStatusAppService
    {
        void Create(ServiceStatusViewModel model);
        void Update(ServiceStatusViewModel model);
        void Remove(Guid id);
        IEnumerable<ServiceStatusViewModel> GetAll();
        IEnumerable<ServiceStatusViewModel> GetRecent();
        IEnumerable<ServiceStatusViewModel> GetRecentPublic();

    }
}
