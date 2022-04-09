using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.Accounts;
using Minibank.Core.Domains.Accounts.Services;
using Minibank.Web.Controllers.Accounts.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Web.Controllers.Accounts
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{id}")]
        public async Task<AccountDto> GetUserAccount(string id, CancellationToken cancellationToken)
        {
            var model = await _accountService.GetById(id, cancellationToken);

            return new AccountDto
            {
                Id = model.Id,
                UserId = model.UserId,
                AmountOnAccount = model.AmountOnAccount,
                Currency = model.Currency,
                IsOpen = model.IsOpen,
                OpeningDate = model.OpeningDate,
                ClosingDate = model.ClosingDate
            };
        }

        [HttpGet]
        public async Task<List<AccountDto>> GetAll(CancellationToken cancellationToken)
        {
            return (await _accountService.GetAll(cancellationToken))
                .Select(it => new AccountDto
                {
                    Id = it.Id,
                    UserId = it.UserId,
                    AmountOnAccount = it.AmountOnAccount,
                    Currency = it.Currency,
                    IsOpen = it.IsOpen,
                    OpeningDate = it.OpeningDate,
                    ClosingDate = it.ClosingDate
                }).ToList();
        }

        [HttpPost]
        public Task Create(AccountDtoCreate model, CancellationToken cancellationToken)
        {
            return _accountService.Create(new Account
            {
                UserId = model.UserId,
                AmountOnAccount = model.AmountOnAccount,
                Currency = model.Currency
            }, cancellationToken);
        }

        [HttpPut("close/{id}")]
        public Task ToClose(string id, CancellationToken cancellationToken)
        {
            return _accountService.Close(id,cancellationToken);
        }

        [HttpDelete("{id}")]
        public Task Delete(string id, CancellationToken cancellationToken)
        { 
            return _accountService.Delete(id, cancellationToken);
        }

        [HttpGet("calculateCommission")]
        public Task<double> CalculateCommission(double amount, string fromAccountId, string toAccountId, 
            CancellationToken cancellationToken)
        {
            return _accountService.CalculateCommission(amount, fromAccountId, toAccountId, cancellationToken);
        }

        [HttpPut("transferMoney")]
        public Task TransferMoney(double amount, string fromAccountId, string toAccountId, 
            CancellationToken cancellationToken)
        {
            return _accountService.TransferMoney(amount, fromAccountId, toAccountId, cancellationToken);
        }
    }
}
