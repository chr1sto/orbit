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
        IEnumerable<CharacterAdminViewModel> GetCurrent(out int total ,int index = 0, int count = 10, string searchText = "");
        IEnumerable<CharacterAdminViewModel> GetAllByGameAccount(string account, bool includeDeleted, out int total, int index = 0, int count = 10, string searchText = "");
        IEnumerable<CharacterViewModel> GetRanking(out int total, int index = 0, int count = 10, string orderBy = "", string filterJob = "");
        Guid GetWebIdFromCharName(string charName);
    }
}
