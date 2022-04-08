using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Accounts.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetById(string id);
        Task<List<Account>> GetAll();
        Task Create(Account account);
        Task Update(Account account);
        Task Delete(string id);
        Task<bool> ExistForUserId(string userId);
        Task CloseAccount(string id);
        Task<bool> Exists(string id);
    }
}
