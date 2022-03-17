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
        Account IAccountRepository.GetUserAccounts(string id)
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
        IEnumerable<Account> IAccountRepository.GetAll()
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
        void IAccountRepository.Create(Account account)
        {
            var entity = new AccountDbModel
            {
                Id = Guid.NewGuid().ToString(),
                UserId = account.UserId,
                AmoumtOnAccount = account.AmoumtOnAccount,
                Currency = account.Currency,
                IsOpen = true,
                OpeningDate = DateTime.Now,
                ClosingDate = DateTime.MinValue
            };

            _accountStorage.Add(entity);
        }
        void IAccountRepository.Update(Account account)
        {
            var entity = _accountStorage.First(it => it.Id == account.Id);

            if (entity != null)
            {
                entity.Id = account.Id;
                entity.UserId = account.UserId;
                entity.AmoumtOnAccount = account.AmoumtOnAccount;
                entity.Currency = account.Currency;
                entity.IsOpen = account.IsOpen;
                entity.OpeningDate = account.OpeningDate;
                entity.ClosingDate = account.ClosingDate;
            }
            else
            {
                throw new ValidationException("Аккаунта с таким id не существует");
            }

        }
        void IAccountRepository.Delete(string id)
        {
            var entity = _accountStorage.FirstOrDefault(it => it.Id == id);

            if (entity != null)
            {
                _accountStorage.Remove(entity);
            }
            else
            {
                throw new ValidationException("Аккаунта с таким id не сущесвует");
            }
        }
        bool IAccountRepository.ContainsUserId(string userId)
        {
            return _accountStorage.Any(it => it.UserId == userId);
        }
        void IAccountRepository.ToCloseAccount(string id)
        {
            var entity = _accountStorage.First(it => it.Id == id);

            if (entity != null)
            {
                entity.IsOpen = false;
                entity.ClosingDate = DateTime.Now;
            }
            else
            {
                throw new ValidationException("Аккаунта с таким id не сущесвует");
            }
        }

        public void SubAmount(string id, double amount)
        {
            var entity = _accountStorage.First(it => it.Id == id);

            if (entity != null)
            {
                entity.AmoumtOnAccount -= amount;
            }
            else
            {
                throw new ValidationException("Аккаунта с таким id не сущесвует");
            }
        }

        public void AddAmount(string id, double amount)
        {
            var entity = _accountStorage.First(it => it.Id == id);

            if (entity != null)
            {
                entity.AmoumtOnAccount += amount;
            }
            else
            {
                throw new ValidationException("Аккаунта с таким id не сущесвует");
            }
        }
    }
}
