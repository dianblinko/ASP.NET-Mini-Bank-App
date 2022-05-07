using System;

namespace Minibank.Core.Domains.Accounts
{
    public class Account
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public double AmountOnAccount { get; set; }
        public CurrencyEnum Currency { get; set; }
        public bool IsOpen { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
    }
}
