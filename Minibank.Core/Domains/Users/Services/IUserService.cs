using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        Task<User> GetUser(string id);
        Task<List<User>> GetAll();
        Task Create(User user);
        Task Update(User user);
        Task Delete(string id);
    }
}
