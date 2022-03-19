using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.MoneyTransfers;
using Minibank.Core.Domains.MoneyTransfers.Services;
using Minibank.Core.Domains.Users.Services;
using System;
using System.Collections.Generic;

[Flags] public enum permittedCurrencies
{
    RUB, USD, EUR
}

namespace Minibank.Core.Domains.Accounts.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserService _userService;
        private readonly ICurrencyСonversion _currencyСonversion;
        private readonly IMoneyTransferService _moneyTransferService;

        public AccountService(IAccountRepository accountRepository, IUserService userService,
            ICurrencyСonversion currencyConversion, IMoneyTransferService moneyTransferService)
        {
            _accountRepository = accountRepository;
            _userService = userService;
            _currencyСonversion = currencyConversion;
            _moneyTransferService = moneyTransferService;
        }
        public void Create(Account account)
        {
            if (_userService.GetUser(account.UserId) == null)
            {
                throw new ValidationException("Пользователя с таким id не существует");
            }


            if (!Enum.IsDefined(typeof(permittedCurrencies), account.Currency))     
            {
                throw new ValidationException("Задана недопустюмая валюта счета");
            }
            _accountRepository.Create(account);
        }

        public void Delete(string id)
        {
            _accountRepository.Delete(id);
        }

        public Account GetUserAccounts(string id)
        {
            return _accountRepository.GetUserAccounts(id);
        }

        public IEnumerable<Account> GetAll()
        {
            return _accountRepository.GetAll();
        }

        public void Close(String id)
        {
            if (_accountRepository.GetUserAccounts(id).AmoumtOnAccount != 0)
            {
                throw new ValidationException("Нельзя закрыть аккаунт с деньгами на нем");
            }
            _accountRepository.CloseAccount(id);
        }
        public double CalculateCommission(double amount, string fromAccountId, string toAccountId)
        {

            var fromAccountUserId = _accountRepository.GetUserAccounts(fromAccountId).UserId;
            var toAccountUserId = _accountRepository.GetUserAccounts(toAccountId).UserId;
            if (fromAccountUserId == toAccountUserId)
            {
                return 0.0;
            }
            double commission = amount * 0.02;
            return Math.Round(commission, 2);
        }
        public void TransferMoney(double amount, string fromAccountId, string toAccountId)
        {
            if (amount <= 0)
            {
                throw new ValidationException("Неправильна введена сумма перевода");
            }
            if (_accountRepository.GetUserAccounts(fromAccountId) == null)
            {
                throw new ValidationException("Неправильно введен id аккаунта отправителя");
            }
            if (_accountRepository.GetUserAccounts(toAccountId) == null)
            {
                throw new ValidationException("Неправильно введен id аккаунта получателя");
            }
            if (!_accountRepository.GetUserAccounts(fromAccountId).IsOpen)
            {
                throw new ValidationException("Аккаунт отправителя закрыт");
            }
            if (!_accountRepository.GetUserAccounts(toAccountId).IsOpen)
            {
                throw new ValidationException("Аккаунт получателя закрыт");
            }
            if (_accountRepository.GetUserAccounts(fromAccountId).AmoumtOnAccount < amount)
            {
                throw new ValidationException("Недостаточно средств");
            }

            var fromAccountCurrency = _accountRepository.GetUserAccounts(fromAccountId).Currency;
            var toAccountCurrency = _accountRepository.GetUserAccounts(toAccountId).Currency;

            double resultAmount = amount - CalculateCommission(amount, fromAccountId, toAccountId);
            if (fromAccountCurrency != toAccountCurrency)
            {
                resultAmount = _currencyСonversion.Converting(amount, fromAccountCurrency, toAccountCurrency);
            }
            resultAmount = Math.Round(resultAmount, 2);
            _accountRepository.SubAmount(fromAccountId, amount);
            _accountRepository.AddAmount(toAccountId, resultAmount);
            _moneyTransferService.Create(new MoneyTransfer
            {
                Amount = resultAmount,
                Currency = toAccountCurrency,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            });
        }
    }
}
