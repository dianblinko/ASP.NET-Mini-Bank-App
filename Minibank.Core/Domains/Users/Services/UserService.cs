using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using System.Collections.Generic;
using System.Threading;
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

        public async Task Create(User user, CancellationToken cancellationToken)
        {
            await _userRepository.Create(user, cancellationToken);
            await _unitOfWork.SaveChanges();
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            if (await _accountRepository.ExistForUserId(id, cancellationToken))
            {
                throw new ValidationException("Нельзя удалить пользователя с привязанными аккаунтами");
            }

            await _userRepository.Delete(id, cancellationToken);
            await _unitOfWork.SaveChanges();
        }

        public Task<User> GetUser(string id, CancellationToken cancellationToken)
        {
            return _userRepository.GetUser(id, cancellationToken);
        }

        public Task<List<User>> GetAll(CancellationToken cancellationToken)
        {
            return _userRepository.GetAll(cancellationToken);
        }

        public async Task Update(User user, CancellationToken cancellationToken)
        {
            await _userRepository.Update(user, cancellationToken);
            await _unitOfWork.SaveChanges();
        }
    }
}
