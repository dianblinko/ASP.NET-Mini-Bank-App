using FluentValidation;
using Minibank.Core.Domains.Users.Repositories;

namespace Minibank.Core.Domains.Accounts.Validators
{
    public class CloseAccountValidator : AbstractValidator<Account>
    {
        public CloseAccountValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.AmountOnAccount).Equal(0)
                .WithMessage("Нельзя закрыть аккаунт с деньгами на нем");
        }
    }
}