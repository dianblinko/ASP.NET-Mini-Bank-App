using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.MoneyTransfers.Services
{
    public interface IMoneyTransferService
    {
        Task Create(MoneyTransfer moneyTransfer);
    }
}
