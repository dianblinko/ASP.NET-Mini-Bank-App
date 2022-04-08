using Minibank.Core.Domains.MoneyTransfers.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task Create(MoneyTransfer moneyTransfer)
        {
            await _moneyTransferRepository.Create(moneyTransfer);
            await _unitOfWork.SaveChanges();
        }
    }
}
