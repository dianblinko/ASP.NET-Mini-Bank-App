using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Accounts.Services
{
    public interface IAccountService
    {
        Task<Account> GetById(string id);
        Task<List<Account>> GetAll();
        Task Create(Account account);
        Task Delete(string id);
        Task Close(String id);
        Task<double> CalculateCommission(double amount, string fromAccountId, string toAccountId);
        Task TransferMoney(double amount, string fromAccountId, string toAccountId);
    }
}
