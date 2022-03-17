namespace Minibank.Web.Controllers.HistoryOfMoneyTransfer.Dto
{
    public class HistoryOfMoneyTransferDto
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
