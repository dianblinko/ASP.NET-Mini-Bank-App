using Minibank.Core;
using Minibank.Core.Domains.Accounts;
using Minibank.Core.Domains.Accounts.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Minibank.Data.Context;

namespace Minibank.Data.Accounts.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MinibankContext _context;

        public AccountRepository(MinibankContext context)
        {
            _context = context;
        }
        public Account GetById(string id)
        {
            var entity = _context.Accounts
                .AsNoTracking()
                .FirstOrDefault(it => it.Id == id);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Аккаунт id={id} не найден");
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
            return _context.Accounts
                .AsNoTracking()
                .Select(it => new Account()
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
            _context.Accounts.Add(entity);
        }
        public void Update(Account account)
        {
            var entity = _context.Accounts.FirstOrDefault(it => it.Id == account.Id);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Аккаунт id={account.Id} не найден");
            }

            entity.UserId = account.UserId;
            entity.AmoumtOnAccount = account.AmoumtOnAccount;
            entity.Currency = account.Currency;
            entity.IsOpen = account.IsOpen;
            entity.ClosingDate = account.ClosingDate;
            entity.OpeningDate = account.OpeningDate;
        }

        public void Delete(string id)
        {
            var entity = _context.Accounts.FirstOrDefault(it => it.Id == id);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Аккаунт id={id} не найден");
            }

            _context.Accounts.Remove(entity);
        }

        public bool ExistForUserId(string userId)
        {
            return _context.Accounts.Any(it => it.UserId == userId);
        }

        public void CloseAccount(string id)
        {
            var entity = _context.Accounts.FirstOrDefault(it => it.Id == id);
            entity.IsOpen = false;
            entity.ClosingDate = DateTime.Now;
        }

        public void SubAmount(string id, double amount)
        {
            var entity = _context.Accounts.FirstOrDefault(it => it.Id == id);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Аккаунт id={id} не найден");
            }

            entity.AmoumtOnAccount -= amount;
        }

        public void AddAmount(string id, double amount)
        {
            var entity = _context.Accounts.FirstOrDefault(it => it.Id == id);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Аккаунт id={id} не найден");
            }

            entity.AmoumtOnAccount += amount;
        }

        public bool Exists(string id)
        {
            return _context.Accounts
                .AsNoTracking()
                .Any(it =>it.Id == id);
        }
    }
}
