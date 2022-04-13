using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Accounts.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetById(string id, CancellationToken cancellationToken);
        Task<List<Account>> GetAll(CancellationToken cancellationToken);
        Task Create(Account account, CancellationToken cancellationToken);
        Task Update(Account account, CancellationToken cancellationToken);
        Task Delete(string id, CancellationToken cancellationToken);
        Task<bool> ExistForUserId(string userId, CancellationToken cancellationToken);
        Task CloseAccount(string id, CancellationToken cancellationToken);
        Task<bool> Exists(string id, CancellationToken cancellationToken);
    }
}
