using System.Collections.Generic;

namespace Minibank.Core.Domains.HistoryOfMoneyTransfers.Services
{
    public interface IHistoryOfMoneyTransferService
    {
        HistoryOfMoneyTransfer GetHistoryOfMoneyTransfer(string id);
        IEnumerable<HistoryOfMoneyTransfer> GetAll();
        void Create(HistoryOfMoneyTransfer historyOfMoneyTransfer);
        void Update(HistoryOfMoneyTransfer historyOfMoneyTransfer);
        void Delete(string id);
    }
}
