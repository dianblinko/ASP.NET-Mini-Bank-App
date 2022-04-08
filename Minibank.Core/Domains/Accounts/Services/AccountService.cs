using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.MoneyTransfers;
using Minibank.Core.Domains.MoneyTransfers.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Minibank.Core.Domains.Accounts.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrencyConversion _currencyConversion;
        private readonly IMoneyTransferRepository _moneyTransferRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IAccountRepository accountRepository, ICurrencyConversion currencyConversion, 
            IMoneyTransferRepository moneyTransferRepository, IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _currencyConversion = currencyConversion;
            _moneyTransferRepository = moneyTransferRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(Account account)
        {
            if (!await _userRepository.Exists(account.UserId))
            {
                throw new ObjectNotFoundException($"Пользователь id={account.UserId} не найден");
            }
            if (!Enum.IsDefined(typeof(CurrencyEnum), account.Currency))     
            {
                throw new ValidationException("Задана недопустюмая валюта счета");
            }

            await _accountRepository.Create(account);
            await _unitOfWork.SaveChanges();
        }

        public async Task Delete(string id)
        {
            await _accountRepository.Delete(id);
            await _unitOfWork.SaveChanges();
        }

        public Task<Account> GetById(string id)
        {
            return _accountRepository.GetById(id);
        }

        public Task<List<Account>> GetAll()
        {
            return _accountRepository.GetAll();
        }

        public async Task Close(string id)
        {
            var account = await _accountRepository.GetById(id);
            if (account.AmountOnAccount != 0)
            {
                throw new ValidationException("Нельзя закрыть аккаунт с деньгами на нем");
            }

            await _accountRepository.CloseAccount(id);
            await _unitOfWork.SaveChanges();
        }

        public async Task<double> CalculateCommission(double amount, string fromAccountId, string toAccountId)
        {
            var fromAccountUserId = (await _accountRepository.GetById(fromAccountId))
                .UserId;
            var toAccountUserId = (await _accountRepository.GetById(toAccountId))
                .UserId;
            if (fromAccountUserId == toAccountUserId)
            {
                return 0.0;
            }

            double commission = amount * 0.02;
            return Math.Round(commission, 2);
        }

        public async Task TransferMoney(double amount, string fromAccountId, string toAccountId)
        {
            Account fromAccount = await _accountRepository.GetById(fromAccountId);
            Account toAccount = await _accountRepository.GetById(toAccountId);
            if (amount <= 0)
            {
                throw new ValidationException("Неправильна введена сумма перевода");
            }
            if (!fromAccount.IsOpen)
            {
                throw new ValidationException("Аккаунт отправителя закрыт");
            }
            if (!toAccount.IsOpen)
            {
                throw new ValidationException("Аккаунт получателя закрыт");
            }
            if (fromAccount.AmountOnAccount < amount)
            {
                throw new ValidationException("Недостаточно средств");
            }

            var fromAccountCurrency = fromAccount.Currency; 
            var toAccountCurrency = toAccount.Currency;
            double resultAmount = amount - await CalculateCommission(amount, fromAccountId, toAccountId);

            if (fromAccountCurrency != toAccountCurrency)
            {
                resultAmount = await _currencyConversion.Converting(resultAmount, fromAccountCurrency, toAccountCurrency);
            }

            resultAmount = Math.Round(resultAmount, 2);

            fromAccount.AmountOnAccount -= amount;
            await _accountRepository.Update(fromAccount);

            toAccount.AmountOnAccount += resultAmount;
            await _accountRepository.Update(toAccount);

            await _moneyTransferRepository.Create(new MoneyTransfer
            {
                Amount = resultAmount,
                Currency = toAccountCurrency,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            });
            await _unitOfWork.SaveChanges();
        }
    }
}
