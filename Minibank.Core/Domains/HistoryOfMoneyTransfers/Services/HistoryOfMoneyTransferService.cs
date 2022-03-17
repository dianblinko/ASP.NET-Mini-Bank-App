using Minibank.Core.Domains.HistoryOfMoneyTransfers.Repositories;
using System.Collections.Generic;

namespace Minibank.Core.Domains.HistoryOfMoneyTransfers.Services
{
    public class HistoryOfMoneyTransferService : IHistoryOfMoneyTransferService
    {
        private readonly IHistoryOfMoneyTransferRepository _historyOfMoneyTransferRepository;

        public HistoryOfMoneyTransferService(IHistoryOfMoneyTransferRepository historyOfMoneyTransferRepository)
        {
            _historyOfMoneyTransferRepository = historyOfMoneyTransferRepository;
        }

        void IHistoryOfMoneyTransferService.Create(HistoryOfMoneyTransfer historyOfMoneyTransfer)
        {
            _historyOfMoneyTransferRepository.Create(historyOfMoneyTransfer);
        }

        void IHistoryOfMoneyTransferService.Delete(string id)
        {

            _historyOfMoneyTransferRepository.Delete(id);

        }

        HistoryOfMoneyTransfer IHistoryOfMoneyTransferService.GetHistoryOfMoneyTransfer(string id)
        {
            return _historyOfMoneyTransferRepository.GetHistoryOfMoneyTransfer(id);
        }

        IEnumerable<HistoryOfMoneyTransfer> IHistoryOfMoneyTransferService.GetAll()
        {
            return _historyOfMoneyTransferRepository.GetAll();
        }

        void IHistoryOfMoneyTransferService.Update(HistoryOfMoneyTransfer historyOfMoneyTransfer)
        {
            _historyOfMoneyTransferRepository.Update(historyOfMoneyTransfer);
        }
    }
}
