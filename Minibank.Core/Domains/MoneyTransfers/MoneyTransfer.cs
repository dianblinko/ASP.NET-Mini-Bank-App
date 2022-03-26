namespace Minibank.Core.Domains.MoneyTransfers
{
    public class MoneyTransfer
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public CurrencyEnum Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
