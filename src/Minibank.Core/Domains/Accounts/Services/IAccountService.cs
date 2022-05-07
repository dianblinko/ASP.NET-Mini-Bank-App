using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Accounts.Services
{
    public interface IAccountService
    {
        Task<Account> GetById(string id, CancellationToken cancellationToken);
        Task<List<Account>> GetAll(CancellationToken cancellationToken);
        Task Create(Account account, CancellationToken cancellationToken);
        Task Delete(string id, CancellationToken cancellationToken);
        Task Close(String id, CancellationToken cancellationToken);
        Task<double> CalculateCommission(double amount, string fromAccountId, string toAccountId, 
            CancellationToken cancellationToken);
        Task TransferMoney(double amount, string fromAccountId, string toAccountId, 
            CancellationToken cancellationToken);
    }
}
