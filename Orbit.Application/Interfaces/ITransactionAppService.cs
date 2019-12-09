using Orbit.Application.ViewModels;
using Orbit.Domain.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.Interfaces
{
    public interface ITransactionAppService
    {
        void Add(Orbit.Domain.Game.Transaction transaction);
        IEnumerable<TransactionViewModel> GetAllByUser(Guid userid, out int recordCount, int pageIndex = 0, int recordsPerPage = 20);
        TransactionViewModel GetById(Guid id);
        TransactionViewModel GetLastVote(Guid userId, string ipAddress);
        int GetBalance(Guid userId, string currency);
        IEnumerable<TransactionViewModel> GetAllPendingForGame();
        void Update(TransactionViewModel transactionViewModel);
        bool DonateOrderExists(string orderId);
        IEnumerable<TransactionAdminViewModel> GetAll(out int recordCount, int pageIndex = 0, int recordsPerPage = 0, Guid? userid = null, string currency = null, DateTime? from = null, DateTime? until = null, int minValue = int.MinValue, int maxValue = int.MaxValue, string status = null, string filter = null);
    }
}
