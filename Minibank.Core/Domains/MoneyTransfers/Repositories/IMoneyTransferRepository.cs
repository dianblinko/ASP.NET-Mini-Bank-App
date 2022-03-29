using System.Collections.Generic;

namespace Minibank.Core.Domains.MoneyTransfers.Repositories
{
    public interface IMoneyTransferRepository
    { 
        void Create(MoneyTransfer moneyTransfer);
    }
}
