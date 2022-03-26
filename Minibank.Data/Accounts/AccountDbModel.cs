using System;
using Minibank.Core.Domains;

namespace Minibank.Data.Accounts
{
    public class AccountDbModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public double AmoumtOnAccount { get; set; }
        public CurrencyEnum Currency { get; set; }
        public bool IsOpen { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
    }
}
