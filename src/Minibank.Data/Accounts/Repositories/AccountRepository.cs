using Minibank.Core;
using Minibank.Core.Domains.Accounts;
using Minibank.Core.Domains.Accounts.Repositories;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public async Task<Account> GetById(string id, CancellationToken cancellationToken)
        {
            var entity = await _context.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(it => it.Id == id, cancellationToken: cancellationToken);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Аккаунт id={id} не найден");
            }

            return new Account
            {
                Id = entity.Id,
                UserId = entity.UserId,
                AmountOnAccount = entity.AmountOnAccount,
                Currency = entity.Currency,
                IsOpen = entity.IsOpen,
                OpeningDate = entity.OpeningDate,
                ClosingDate = entity.ClosingDate
            };
        }

        public Task<List<Account>> GetAll(CancellationToken cancellationToken)
        {
            return _context.Accounts
                .AsNoTracking()
                .Select(it => new Account()
            {
                Id = it.Id,
                UserId = it.UserId,
                AmountOnAccount = it.AmountOnAccount,
                Currency = it.Currency,
                IsOpen = it.IsOpen,
                OpeningDate = it.OpeningDate,
                ClosingDate = it.ClosingDate
            }).ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task Create(Account account, CancellationToken cancellationToken)
        {
            var entity = new AccountDbModel
            {
                Id = Guid.NewGuid().ToString(),
                UserId = account.UserId,
                AmountOnAccount = account.AmountOnAccount,
                Currency = account.Currency, 
                IsOpen = true,
                OpeningDate = DateTime.Now,
                ClosingDate = null
            };
            await _context.Accounts.AddAsync(entity, cancellationToken);
        }
        public async Task Update(Account account, CancellationToken cancellationToken)
        {
            var entity = await _context.Accounts
                .FirstOrDefaultAsync(it => it.Id == account.Id, cancellationToken: cancellationToken);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Аккаунт id={account.Id} не найден");
            }

            entity.UserId = account.UserId;
            entity.AmountOnAccount = account.AmountOnAccount;
            entity.Currency = account.Currency;
            entity.IsOpen = account.IsOpen;
            entity.ClosingDate = account.ClosingDate;
            entity.OpeningDate = account.OpeningDate;
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            var entity = await _context.Accounts
                .FirstOrDefaultAsync(it => it.Id == id, cancellationToken: cancellationToken);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Аккаунт id={id} не найден");
            }

            _context.Accounts.Remove(entity);
        }

        public Task<bool> ExistForUserId(string userId, CancellationToken cancellationToken)
        {
            return _context.Accounts
                .AnyAsync(it => it.UserId == userId, cancellationToken: cancellationToken);
        }

        public async Task CloseAccount(string id, CancellationToken cancellationToken)
        {
            var entity = await _context.Accounts
                .FirstOrDefaultAsync(it => it.Id == id, cancellationToken: cancellationToken);
            entity.IsOpen = false;
            entity.ClosingDate = DateTime.Now;
        }

        public Task<bool> Exists(string id, CancellationToken cancellationToken)
        {
            return _context.Accounts
                .AsNoTracking()
                .AnyAsync(it =>it.Id == id, cancellationToken: cancellationToken);
        }
    }
}
