using Minibank.Core;
using Minibank.Core.Domains.HistoryOfMoneyTransfers;
using Minibank.Core.Domains.HistoryOfMoneyTransfers.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Minibank.Data.HistoryOfMoneyTransfers.Repositories
{
    public class HistoryOfMoneyTransferRepository : IHistoryOfMoneyTransferRepository
    {
        private static List<HistoryOfMoneyTransferDbModel> historyOfMoneyTransferStorage = new List<HistoryOfMoneyTransferDbModel>();
        HistoryOfMoneyTransfer IHistoryOfMoneyTransferRepository.GetHistoryOfMoneyTransfer(string id)
        {
            var entity = historyOfMoneyTransferStorage.FirstOrDefault(it => it.Id == id);
            if (entity == null)
            {
                return null;
            }
            return new HistoryOfMoneyTransfer
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Currency = entity.Currency,
                FromAccountId = entity.FromAccountId,
                ToAccountId = entity.ToAccountId
            };
        }
        IEnumerable<HistoryOfMoneyTransfer> IHistoryOfMoneyTransferRepository.GetAll()
        {
            return historyOfMoneyTransferStorage.Select(it => new HistoryOfMoneyTransfer()
            {
                Id = it.Id,
                Amount = it.Amount,
                Currency = it.Currency,
                FromAccountId = it.FromAccountId,
                ToAccountId = it.ToAccountId
            });
        }
        void IHistoryOfMoneyTransferRepository.Create(HistoryOfMoneyTransfer historyOfMoneyTransfer)
        {
            var entity = new HistoryOfMoneyTransferDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Amount = historyOfMoneyTransfer.Amount,
                Currency = historyOfMoneyTransfer.Currency,
                FromAccountId = historyOfMoneyTransfer.FromAccountId,
                ToAccountId = historyOfMoneyTransfer.ToAccountId
            };

            historyOfMoneyTransferStorage.Add(entity);
        }
        void IHistoryOfMoneyTransferRepository.Update(HistoryOfMoneyTransfer historyOfMoneyTransfer)
        {
            var entity = historyOfMoneyTransferStorage.First(it => it.Id == historyOfMoneyTransfer.Id);

            if (entity != null)
            {
                entity.Id = historyOfMoneyTransfer.Id;
                entity.Amount = historyOfMoneyTransfer.Amount;
                entity.Currency = historyOfMoneyTransfer.Currency;
                entity.FromAccountId = historyOfMoneyTransfer.FromAccountId;
                entity.ToAccountId = historyOfMoneyTransfer.ToAccountId;
            }
            else
            {
                throw new ValidationException("Перевода с таким id не существует");
            }

        }
        void IHistoryOfMoneyTransferRepository.Delete(string id)
        {
            var entity = historyOfMoneyTransferStorage.FirstOrDefault(it => it.Id == id);

            if (entity != null)
            {
                historyOfMoneyTransferStorage.Remove(entity);
            }
            else
            {
                throw new ValidationException("Перевода с таким id не существует");
            }
        }
    }
}
