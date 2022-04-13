using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.MoneyTransfers;
using Minibank.Core.Domains.MoneyTransfers.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace Minibank.Core.Domains.Accounts.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrencyConversion _currencyConversion;
        private readonly IMoneyTransferRepository _moneyTransferRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Account> _closeAccountValidator;

        public AccountService(IAccountRepository accountRepository, ICurrencyConversion currencyConversion, 
            IMoneyTransferRepository moneyTransferRepository, IUserRepository userRepository,
            IUnitOfWork unitOfWork, IValidator<Account> closeAccountValidator)
        {
            _accountRepository = accountRepository;
            _currencyConversion = currencyConversion;
            _moneyTransferRepository = moneyTransferRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _closeAccountValidator = closeAccountValidator;
        }

        public async Task Create(Account account, CancellationToken cancellationToken)
        {
            if (!await _userRepository.Exists(account.UserId, cancellationToken))
            {
                throw new ObjectNotFoundException($"Пользователь id={account.UserId} не найден");
            }
            if (!Enum.IsDefined(typeof(CurrencyEnum), account.Currency))     
            {
                throw new ValidationException("Задана недопустюмая валюта счета");
            }

            await _accountRepository.Create(account, cancellationToken);
            await _unitOfWork.SaveChanges();
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _accountRepository.Delete(id, cancellationToken);
            await _unitOfWork.SaveChanges();
        }

        public Task<Account> GetById(string id, CancellationToken cancellationToken)
        {
            return _accountRepository.GetById(id, cancellationToken);
        }

        public Task<List<Account>> GetAll(CancellationToken cancellationToken)
        {
            return _accountRepository.GetAll(cancellationToken);
        }

        public async Task Close(string id, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetById(id, cancellationToken);
            await _closeAccountValidator.ValidateAndThrowAsync(account, cancellationToken);
            
            await _accountRepository.CloseAccount(id, cancellationToken);
            await _unitOfWork.SaveChanges();
        }

        public async Task<double> CalculateCommission(double amount, string fromAccountId, string toAccountId, 
            CancellationToken cancellationToken)
        {
            var fromAccountUser = await _accountRepository.GetById(fromAccountId, cancellationToken);
            var toAccountUser = await _accountRepository.GetById(toAccountId, cancellationToken);
            
            if (fromAccountUser.UserId == toAccountUser.UserId)
            {
                return 0.0;
            }

            double commission = amount * 0.02;
            return Math.Round(commission, 2);
        }

        public async Task TransferMoney(double amount, string fromAccountId, string toAccountId, 
            CancellationToken cancellationToken)
        {
            Account fromAccount = await _accountRepository.GetById(fromAccountId, cancellationToken);
            Account toAccount = await _accountRepository.GetById(toAccountId, cancellationToken);
            
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
                throw new ValidationException("Недостаточно средства");
            }
            
            var fromAccountCurrency = fromAccount.Currency; 
            var toAccountCurrency = toAccount.Currency;
            double resultAmount = amount - await CalculateCommission(amount, fromAccountId, toAccountId, cancellationToken);

            if (fromAccountCurrency != toAccountCurrency)
            {
                resultAmount = await _currencyConversion.Converting(resultAmount, fromAccountCurrency, toAccountCurrency);
            }

            resultAmount = Math.Round(resultAmount, 2);

            fromAccount.AmountOnAccount -= amount;
            await _accountRepository.Update(fromAccount, cancellationToken);

            toAccount.AmountOnAccount += resultAmount;
            await _accountRepository.Update(toAccount, cancellationToken);

            await _moneyTransferRepository.Create(new MoneyTransfer
            {
                Amount = resultAmount,
                Currency = toAccountCurrency,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId
            }, cancellationToken);
            await _unitOfWork.SaveChanges();
        }
    }
}
