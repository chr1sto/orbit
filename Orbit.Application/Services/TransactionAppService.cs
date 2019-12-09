using AutoMapper;
using Orbit.Application.Interfaces;
using Orbit.Application.ViewModels;
using Orbit.Domain.Core.Bus;
using Orbit.Domain.Core.Interfaces;
using Orbit.Domain.Game;
using Orbit.Domain.Transaction.Commands;
using Orbit.Infra.Persistence.Repository.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbit.Application.Services
{
    public class TransactionAppService : ITransactionAppService
    {
        private readonly IRepository<Transaction> _repository;
        private readonly IMapper _mapper;
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IMediatorHandler _bus;

        public TransactionAppService(IRepository<Transaction> repository, IMapper mapper, IEventStoreRepository eventStoreRepository, IMediatorHandler bus)
        {
            _repository = repository;
            _mapper = mapper;
            _eventStoreRepository = eventStoreRepository;
            _bus = bus;
        }

        public void Add(Transaction transaction)
        {
            var createCommand = _mapper.Map<CreateTransactionCommand>(transaction);
            _bus.SendCommand(createCommand);
        }

        public bool DonateOrderExists(string orderId)
        {
            return _repository.GetAll().Where(x => x.Reason.Contains(orderId)).FirstOrDefault() != null;
        }

        public IEnumerable<TransactionViewModel> GetAllByUser(Guid userid, out int recordCount, int pageIndex = 0, int recordsPerPage = 20)
        {
            var query = _repository.GetAll().Where(x => x.UserId == userid);
            recordCount = query.Count();

            var transactions = query.OrderByDescending(o => o.Date).Skip(pageIndex * recordsPerPage).Take(recordsPerPage);

            return _mapper.ProjectTo<TransactionViewModel>(transactions).AsEnumerable();
        }

        public IEnumerable<TransactionAdminViewModel> GetAll(out int recordCount, int pageIndex = 0, int recordsPerPage = 0, Guid? userid = null, string currency = null, DateTime? from = null, DateTime? until = null, int minValue = int.MinValue, int maxValue = int.MaxValue, string status = null, string filter = null)
        {
            var query = _repository.GetAll().Where(x => x.Amount >= minValue && x.Amount <= maxValue);
            if (userid != null) query = query.Where(x => x.UserId == userid);
            if (!String.IsNullOrWhiteSpace(currency)) query = query.Where(x => x.Currency == currency);
            if (from != null && until != null) query = query.Where(x => x.Date >= from && x.Date <= until);
            if (!String.IsNullOrWhiteSpace(status)) query = query.Where(x => x.Status == status);
            if(!String.IsNullOrWhiteSpace(filter)) query = query.Where(x => x.AdditionalInfo.Contains(filter));
            recordCount = query.Count();

            var transactions = query.OrderByDescending(o => o.Date).Skip(pageIndex * recordsPerPage).Take(recordsPerPage);

            return _mapper.ProjectTo<TransactionAdminViewModel>(transactions).AsEnumerable();
        }

        public IEnumerable<TransactionViewModel> GetAllPendingForGame()
        {
            var transactions = _repository.GetAll().Where(x => x.Target.ToUpper() == "GAME" && x.Status.ToUpper() == "PENDING");
            return _mapper.ProjectTo<TransactionViewModel>(transactions).AsEnumerable();
        }

        public int GetBalance(Guid userId, string currency)
        {
            return _repository.GetAll().Where(x => x.UserId == userId && x.Currency.ToUpper() == currency.ToUpper() && x.Status != "FAILED").Sum(x => x.Amount);
        }

        public TransactionAdminViewModel GetById(Guid id)
        {
            return _mapper.Map<TransactionAdminViewModel>(_repository.GetById(id));
        }

        public TransactionViewModel GetLastVote(Guid userId, string ipAddress)
        {
            var transaction = _repository.GetAll().Where((x => (x.UserId == userId || x.IpAddress == ipAddress) && x.Target == "WEB")).OrderByDescending(x => x.Date).FirstOrDefault();
            return _mapper.Map<TransactionViewModel>(transaction);
        }

        public void Update(TransactionViewModel transactionViewModel)
        {
            var updateCommand = _mapper.Map<UpdateTransactionCommand>(transactionViewModel);
            _bus.SendCommand(updateCommand);
        }
    }
}
