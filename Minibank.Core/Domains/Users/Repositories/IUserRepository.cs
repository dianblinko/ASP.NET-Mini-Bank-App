using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUser(string id);
        Task<List<User>> GetAll();
        Task Create(User user);
        Task Update(User user);
        Task Delete(string id);
        Task<bool> Exists(string id);
    }
}
