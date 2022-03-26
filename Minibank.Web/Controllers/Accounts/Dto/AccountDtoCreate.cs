using Minibank.Core.Domains;

namespace Minibank.Web.Controllers.Accounts.Dto
{
    public class AccountDtoCreate
    {
        public string UserId { get; set; }
        public int AmoumtOnAccount { get; set; }
        public CurrencyEnum Currency { get; set; }
    }
}
