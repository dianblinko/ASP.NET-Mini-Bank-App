using Minibank.Core.Domains.MoneyTransfers.Repositories;
using System.Collections.Generic;

namespace Minibank.Core.Domains.MoneyTransfers.Services
{
    public class MoneyTransferService : IMoneyTransferService
    {
        private readonly IMoneyTransferRepository _moneyTransferRepository;

        public MoneyTransferService(IMoneyTransferRepository moneyTransferRepository)
        {
            _moneyTransferRepository = moneyTransferRepository;
        }

        public void Create(MoneyTransfer moneyTransfer)
        {
            _moneyTransferRepository.Create(moneyTransfer);
        }
    }
}
