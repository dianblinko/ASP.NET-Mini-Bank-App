using Minibank.Core.Domains.MoneyTransfers;
using Minibank.Core.Domains.MoneyTransfers.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Minibank.Data.Context;


namespace Minibank.Data.MoneyTransfers.Repositories
{
    public class MoneyTransferRepository : IMoneyTransferRepository
    {
        private readonly MinibankContext _context;

        public MoneyTransferRepository(MinibankContext context)
        {
            _context = context;
        }
        public async Task Create(MoneyTransfer moneyTransfer, CancellationToken cancellationToken)
        {
            var entity = new MoneyTransferDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Amount = moneyTransfer.Amount,
                Currency = moneyTransfer.Currency,
                FromAccountId = moneyTransfer.FromAccountId,
                ToAccountId = moneyTransfer.ToAccountId
            };
            await _context.MoneyTransfer.AddAsync(entity, cancellationToken);
        }
    }
}
