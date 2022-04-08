using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, 
            IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(User user)
        {
            await _userRepository.Create(user);
            await _unitOfWork.SaveChanges();
        }

        public async Task Delete(string id)
        {
            if (await _accountRepository.ExistForUserId(id))
            {
                throw new ValidationException("Нельзя удалить пользователя с привязанными аккаунтами");
            }

            await _userRepository.Delete(id);
            await _unitOfWork.SaveChanges();
        }

        public Task<User> GetUser(string id)
        {
            return _userRepository.GetUser(id);
        }

        public Task<List<User>> GetAll()
        {
            return _userRepository.GetAll();
        }

        public async Task Update(User user)
        {
            await _userRepository.Update(user);
            await _unitOfWork.SaveChanges();
        }
    }
}
