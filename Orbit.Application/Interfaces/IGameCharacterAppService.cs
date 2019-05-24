using Orbit.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.Interfaces
{
    /// <summary>
    /// Contains only administrative Functionality.
    /// </summary>
    public interface IGameCharacterAppService
    {
        void InsertNewEntries(IEnumerable<CharacterAdminViewModel> models);
        void ClearUntilRecent();
        IEnumerable<CharacterAdminViewModel> GetCurrent();
        IEnumerable<CharacterAdminViewModel> GetAllByGameAccount(string account);
    }
}
