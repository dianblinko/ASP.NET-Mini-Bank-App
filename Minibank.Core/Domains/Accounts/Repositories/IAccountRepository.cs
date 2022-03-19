using System.Collections.Generic;

namespace Minibank.Core.Domains.Accounts.Repositories
{
    public interface IAccountRepository
    {
        Account GetUserAccounts(string id);
        IEnumerable<Account> GetAll();
        void Create(Account account);
        void Delete(string id);
        bool ContainsUserId(string userId);
        void CloseAccount(string id);
        void SubAmount(string id, double amount);
        void AddAmount(string id, double amount);
    }
}
