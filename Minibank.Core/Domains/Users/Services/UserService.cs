using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using System.Collections.Generic;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;

        public UserService(IUserRepository userRepository, IAccountRepository accountRepository)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
        }

        public void Create(User user)
        {
            _userRepository.Create(user);
        }

        public void Delete(string id)
        {
            if (_accountRepository.ExistForUserId(id))
            {
                throw new ValidationException("Нельзя удалить пользователя с привязанными аккаунтами");
            }

            _userRepository.Delete(id);
        }

        public User GetUser(string id)
        {
            return _userRepository.GetUser(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public void Update(User user)
        {
            _userRepository.Update(user);
        }
    }
}
