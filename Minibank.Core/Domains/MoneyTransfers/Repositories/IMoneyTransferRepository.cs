using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.MoneyTransfers.Repositories
{
    public interface IMoneyTransferRepository
    { 
        Task Create(MoneyTransfer moneyTransfer);
    }
}
