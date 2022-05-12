using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        Task<User> GetUser(string id, CancellationToken cancellationToken);
        Task<List<User>> GetAll(CancellationToken cancellationToken);
        Task Create(User user, CancellationToken cancellationToken);
        Task Update(User user, CancellationToken cancellationToken);
        Task Delete(string id, CancellationToken cancellationToken);
    }
}
