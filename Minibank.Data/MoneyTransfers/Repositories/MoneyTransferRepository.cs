using Minibank.Core;
using Minibank.Core.Domains.MoneyTransfers;
using Minibank.Core.Domains.MoneyTransfers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Minibank.Data.MoneyTransfers.Repositories
{
    public class MoneyTransferRepository : IMoneyTransferRepository
    {
        private static List<MoneyTransferDbModel> moneyTransferStorage = new List<MoneyTransferDbModel>();
        public void Create(MoneyTransfer moneyTransfer)
        {
            var entity = new MoneyTransferDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Amount = moneyTransfer.Amount,
                Currency = moneyTransfer.Currency,
                FromAccountId = moneyTransfer.FromAccountId,
                ToAccountId = moneyTransfer.ToAccountId
            };
            moneyTransferStorage.Add(entity);
        }
    }
}
