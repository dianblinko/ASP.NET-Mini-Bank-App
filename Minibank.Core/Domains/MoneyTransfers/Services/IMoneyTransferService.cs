using System.Collections.Generic;

namespace Minibank.Core.Domains.MoneyTransfers.Services
{
    public interface IMoneyTransferService
    {
        void Create(MoneyTransfer moneyTransfer);
    }
}
