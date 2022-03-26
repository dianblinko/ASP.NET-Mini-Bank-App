using Minibank.Core;
using Minibank.Core.Domains.Accounts;
using Minibank.Core.Domains.Accounts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minibank.Data.Accounts.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private static List<AccountDbModel> _accountStorage = new List<AccountDbModel>();

        public Account GetUserAccounts(string id)
        {
            var entity = _accountStorage.FirstOrDefault(it => it.Id == id);
            if (entity == null)
            {
                return null;
            }

            return new Account
            {
                Id = entity.Id,
                UserId = entity.UserId,
                AmoumtOnAccount = entity.AmoumtOnAccount,
                Currency = entity.Currency,
                IsOpen = entity.IsOpen,
                OpeningDate = entity.OpeningDate,
                ClosingDate = entity.ClosingDate
            };
        }

        public IEnumerable<Account> GetAll()
        {
            return _accountStorage.Select(it => new Account()
            {
                Id = it.Id,
                UserId = it.UserId,
                AmoumtOnAccount = it.AmoumtOnAccount,
                Currency = it.Currency,
                IsOpen = it.IsOpen,
                OpeningDate = it.OpeningDate,
                ClosingDate = it.ClosingDate
            });
        }

        public void Create(Account account)
        {
            var entity = new AccountDbModel
            {
                Id = Guid.NewGuid().ToString(),
                UserId = account.UserId,
                AmoumtOnAccount = account.AmoumtOnAccount,
                Currency = account.Currency, 
                IsOpen = true,
                OpeningDate = DateTime.Now,
                ClosingDate = null
            };
            _accountStorage.Add(entity);
        }

        public void Delete(string id)
        {
            var entity = _accountStorage.FirstOrDefault(it => it.Id == id);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Аккаунт id={id} не найден");
            }

            _accountStorage.Remove(entity);
        }

        public bool ExistForUserId(string userId)
        {
            return _accountStorage.Any(it => it.UserId == userId);
        }

        public void CloseAccount(string id)
        {
            var entity = _accountStorage.FirstOrDefault(it => it.Id == id);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Аккаунт id={id} не найден");
            }

            entity.IsOpen = false;
            entity.ClosingDate = DateTime.Now;
        }

        public void SubAmount(string id, double amount)
        {
            var entity = _accountStorage.FirstOrDefault(it => it.Id == id);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Аккаунт id={id} не найден");
            }

            entity.AmoumtOnAccount -= amount;
        }

        public void AddAmount(string id, double amount)
        {
            var entity = _accountStorage.FirstOrDefault(it => it.Id == id);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Аккаунт id={id} не найден");
            }

            entity.AmoumtOnAccount += amount;
        }

        public bool Exists(string id)
        {
            var entity = _accountStorage.FirstOrDefault(it => it.Id == id);
            if (entity == null)
            {
                return false;
            }

            return true;
        }
    }
}
