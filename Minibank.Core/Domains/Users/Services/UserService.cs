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
        void IUserService.Create(User user)
        {
            _userRepository.Create(user);
        }

        void IUserService.Delete(string id)
        {
            if (_accountRepository.ContainsUserId(id))
            {
                throw new ValidationException("Нельзя удалить пользователя с привязанными аккаунтами");
            }
            else
            {
                _userRepository.Delete(id);
            }
        }

        User IUserService.GetUser(string id)
        {
            return _userRepository.GetUser(id);
        }

        IEnumerable<User> IUserService.GetAll()
        {
            return _userRepository.GetAll();
        }

        void IUserService.Update(User user)
        {
            _userRepository.Update(user);
        }
    }
}
