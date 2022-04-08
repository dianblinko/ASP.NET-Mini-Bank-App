using Minibank.Core.Domains.MoneyTransfers.Repositories;
using System.Collections.Generic;

namespace Minibank.Core.Domains.MoneyTransfers.Services
{
    public class MoneyTransferService : IMoneyTransferService
    {
        private readonly IMoneyTransferRepository _moneyTransferRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MoneyTransferService(IMoneyTransferRepository moneyTransferRepository,
            IUnitOfWork unitOfWork)
        {
            _moneyTransferRepository = moneyTransferRepository;
            _unitOfWork = unitOfWork;
        }

        public void Create(MoneyTransfer moneyTransfer)
        {
            _moneyTransferRepository.Create(moneyTransfer);
            _unitOfWork.SaveChanges();
        }
    }
}
