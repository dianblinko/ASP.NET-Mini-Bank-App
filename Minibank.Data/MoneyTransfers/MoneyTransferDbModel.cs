using Minibank.Core.Domains;

namespace Minibank.Data.MoneyTransfers
{
    public class MoneyTransferDbModel
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public CurrencyEnum Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
