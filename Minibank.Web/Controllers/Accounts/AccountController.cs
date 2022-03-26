using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains; 
using Minibank.Core.Domains.Accounts;
using Minibank.Core.Domains.Accounts.Services;
using Minibank.Web.Controllers.Accounts.Dto;
using System.Collections.Generic;
using System.Linq;

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
        public AccountDto GetUserAccounts(string id)
        {
            var model = _accountService.GetUserAccounts(id);

            return new AccountDto
            {
                Id = model.Id,
                UserId = model.UserId,
                AmoumtOnAccount = model.AmoumtOnAccount,
                Currency = model.Currency,
                IsOpen = model.IsOpen,
                OpeningDate = model.OpeningDate,
                ClosingDate = model.ClosingDate
            };
        }

        [HttpGet]
        public IEnumerable<AccountDto> GetAll()
        {
            return _accountService.GetAll()
                .Select(it => new AccountDto
                {
                    Id = it.Id,
                    UserId = it.UserId,
                    AmoumtOnAccount = it.AmoumtOnAccount,
                    Currency = it.Currency,
                    IsOpen = it.IsOpen,
                    OpeningDate = it.OpeningDate,
                    ClosingDate = it.ClosingDate
                });
        }

        [HttpPost]
        public void Create(AccountDtoCreate model)
        {
            _accountService.Create(new Account
            {
                UserId = model.UserId,
                AmoumtOnAccount = model.AmoumtOnAccount,
                Currency = model.Currency
            });
        }

        [HttpPut("close/{id}")]
        public void ToClose(string id)
        {
            _accountService.Close(id);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _accountService.Delete(id);
        }

        [HttpGet("calculateCommission")]
        public double CalculateCommission(double amount, string fromAccountId, string toAccountId)
        {
            return _accountService.CalculateCommission(amount, fromAccountId, toAccountId);
        }

        [HttpPut("transferMoney")]
        public void TransferMoney(double amount, string fromAccountId, string toAccountId)
        {
            _accountService.TransferMoney(amount, fromAccountId, toAccountId);
        }
    }
}
