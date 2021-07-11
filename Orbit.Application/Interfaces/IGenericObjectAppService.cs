using Orbit.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.Interfaces
{
    public interface IGenericObjectAppService
    {
        void Create(GenericObjectViewModel model);
        void Update(GenericObjectViewModel model);
        void Remove(Guid id);
        IEnumerable<GenericObjectViewModel> GetAll(string type,int amount = 5, bool mostRecent = true);
        GenericObjectViewModel GetSingle(Guid id);
    }
}
