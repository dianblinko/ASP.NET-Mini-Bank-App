using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.HistoryOfMoneyTransfers;
using Minibank.Core.Domains.HistoryOfMoneyTransfers.Services;
using Minibank.Core.Domains.Users.Services;
using System;
using System.Collections.Generic;

namespace Minibank.Core.Domains.Accounts.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserService _userService;
        private readonly ICurrencyСonversion _currencyСonversion;
        private readonly IHistoryOfMoneyTransferService _historyOfMoneyTransferService;

        public AccountService(IAccountRepository accountRepository, IUserService userService,
            ICurrencyСonversion currencyConversion, IHistoryOfMoneyTransferService historyOfMoneyTransferService)
        {
            _accountRepository = accountRepository;
            _userService = userService;
            _currencyСonversion = currencyConversion;
            _historyOfMoneyTransferService = historyOfMoneyTransferService;
        }
        void IAccountService.Create(Account account)
        {
            if (_userService.GetUser(account.UserId) != null)
            {
                if (new List<String> { "RUB", "USD", "EUR" }.Contains(account.Currency))
                {
                    _accountRepository.Create(account);
                }
                else
                {
                    throw new ValidationException("Заданый недопустимый курс счета");
                }
            }
            else
            {
                throw new ValidationException("Пользователя с таким id не существует");
            }

        }

        void IAccountService.Delete(string id)
        {
            _accountRepository.Delete(id);
        }

        Account IAccountService.GetUserAccounts(string id)
        {
            return _accountRepository.GetUserAccounts(id);
        }

        IEnumerable<Account> IAccountService.GetAll()
        {
            return _accountRepository.GetAll();
        }

        void IAccountService.Update(Account account)
        {
            _accountRepository.Update(account);
        }

        void IAccountService.ToClose(String id)
        {
            if (_accountRepository.GetUserAccounts(id).AmoumtOnAccount != 0)
            {
                throw new ValidationException("Нельзя закрыть аккаунт с деньгами на нем");
            }
            else
            {
                _accountRepository.ToCloseAccount(id);
            }
        }
        public double CalculateCommission(double amount, string fromAccountId, string toAccountId)
        {

            var fromAccountUserId = _accountRepository.GetUserAccounts(fromAccountId).UserId;
            var toAccountUserId = _accountRepository.GetUserAccounts(toAccountId).UserId;
            if (fromAccountUserId == toAccountUserId)
            {
                return 0.0;
            }
            else
            {
                double commission = amount * 0.02;
                return Math.Round(commission, 2);
            }
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

            //double resultAmount = amount - ((IAccountService)this).CalculateCommission(amount, fromAccountId, toAccountId);
            double resultAmount = amount - CalculateCommission(amount, fromAccountId, toAccountId);
            //double resultAmount = amount;
            if (fromAccountCurrency != toAccountCurrency)
            {
                resultAmount = _currencyСonversion.Converting(amount, fromAccountCurrency, toAccountCurrency);
            }
            resultAmount = Math.Round(resultAmount, 2);
            _accountRepository.SubAmount(fromAccountId, amount);
            _accountRepository.AddAmount(toAccountId, resultAmount);
            _historyOfMoneyTransferService.Create(new HistoryOfMoneyTransfer
            {
                Amount = resultAmount,
                Currency = toAccountCurrency,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            });


        }
    }
}
