using Minibank.Core.Domains;
using Minibank.Core.Domains.MoneyTransfers;
using Minibank.Core.Domains.MoneyTransfers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Minibank.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace Minibank.Data.MoneyTransfers.Repositories
{
    public class MoneyTransferRepository : IMoneyTransferRepository
    {
        private readonly MinibankContext _context;

        public MoneyTransferRepository(MinibankContext context)
        {
            _context = context;
        }
        public async Task Create(MoneyTransfer moneyTransfer)
        {
            var entity = new MoneyTransferDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Amount = moneyTransfer.Amount,
                Currency = moneyTransfer.Currency,
                FromAccountId = moneyTransfer.FromAccountId,
                ToAccountId = moneyTransfer.ToAccountId
            };
            await _context.MoneyTransfer.AddAsync(entity);
        }
    }
}
