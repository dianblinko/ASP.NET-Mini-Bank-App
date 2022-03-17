using System;
using System.Collections.Generic;

namespace Minibank.Core.Domains.Accounts.Services
{
    public interface IAccountService
    {
        Account GetUserAccounts(string id);
        IEnumerable<Account> GetAll();
        void Create(Account account);
        void Update(Account account);
        void Delete(string id);
        void ToClose(String id);
        double CalculateCommission(double amount, string fromAccountId, string toAccountId);
        void TransferMoney(double amount, string fromAccountId, string toAccountId);
    }
}
