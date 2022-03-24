using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.MoneyTransfers;
using Minibank.Core.Domains.MoneyTransfers.Repositories;
using Minibank.Core.Domains.Users.Repositories;
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
        private readonly IUserRepository _userRepository;
        private readonly ICurrencyСonversion _currencyСonversion;
        private readonly IMoneyTransferRepository _moneyTransferRepository;

        public AccountService(IAccountRepository accountRepository, ICurrencyСonversion currencyConversion, 
            IMoneyTransferRepository moneyTransferRepository, IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _currencyСonversion = currencyConversion;
            _moneyTransferRepository = moneyTransferRepository;
            _userRepository = userRepository;
        }

        public void Create(Account account)
        {
            if (!_userRepository.Exists(account.UserId))
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
            if (!_accountRepository.Exists(id))
            {
                throw new ValidationException("Аккаунта с таким id не существует");
            }

            _accountRepository.Delete(id);
        }

        public Account GetUserAccounts(string id)
        {
            if (!_accountRepository.Exists(id))
            {
                throw new ValidationException("Аккаунта с таким id не существует");
            }

            return _accountRepository.GetUserAccounts(id);
        }

        public IEnumerable<Account> GetAll()
        {
            return _accountRepository.GetAll();
        }

        public void Close(String id)
        {
            Account account = GetUserAccounts(id);
            if (account == null)
            {
                throw new ValidationException("Аккаунта с таким id не существует");
            }
            if (account.AmoumtOnAccount != 0)
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
            Account fromAccount = _accountRepository.GetUserAccounts(fromAccountId);
            Account toAccount = _accountRepository.GetUserAccounts(toAccountId);
            if (amount <= 0)
            {
                throw new ValidationException("Неправильна введена сумма перевода");
            }
            if (fromAccount == null)
            {
                throw new ValidationException("Неправильно введен id аккаунта отправителя");
            }
            if (toAccount == null)
            {
                throw new ValidationException("Неправильно введен id аккаунта получателя");
            }
            if (!fromAccount.IsOpen)
            {
                throw new ValidationException("Аккаунт отправителя закрыт");
            }
            if (!toAccount.IsOpen)
            {
                throw new ValidationException("Аккаунт получателя закрыт");
            }
            if (fromAccount.AmoumtOnAccount < amount)
            {
                throw new ValidationException("Недостаточно средств");
            }

            var fromAccountCurrency = fromAccount.Currency;
            var toAccountCurrency = toAccount.Currency;
            double resultAmount = amount - CalculateCommission(amount, fromAccountId, toAccountId);

            if (fromAccountCurrency != toAccountCurrency)
            {
                resultAmount = _currencyСonversion.Converting(amount, fromAccountCurrency, toAccountCurrency);
            }

            resultAmount = Math.Round(resultAmount, 2);

            _accountRepository.SubAmount(fromAccountId, amount);
            _accountRepository.AddAmount(toAccountId, resultAmount);
            _moneyTransferRepository.Create(new MoneyTransfer
            {
                Amount = resultAmount,
                Currency = toAccountCurrency,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            });
        }
    }
}
