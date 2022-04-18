using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication.Cookies;
using Minibank.Core.Domains;
using Minibank.Core.Domains.Accounts;
using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.Accounts.Services;
using Minibank.Core.Domains.Accounts.Validators;
using Minibank.Core.Domains.MoneyTransfers;
using Minibank.Core.Domains.MoneyTransfers.Repositories;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domains.Users.Services;
using Moq;
using Xunit;

namespace Minibank.Core.Tests;

public class AccountServiceTests
{

    private readonly Mock<IUserRepository> _fakeUserRepository;
    private readonly Mock<IUserService> _fakeUserService;
    private readonly Mock<IAccountRepository> _fakeAccountRepository;
    private readonly Mock<IUnitOfWork> _fakeUnitOfWork;
    private readonly Mock<ICurrencyConversion> _fakeCurrencyConversion;
    private readonly Mock<IExchangeRateSource> _fakeExchangeRateSource;
    private readonly Mock<IMoneyTransferRepository> _fakeMoneyTransferRepository;
    private readonly Mock<IValidator<Account>> _fakeCloseAccountValidator;
    private readonly AccountService _accountService;
    private readonly CancellationToken _fakeCancellationToken;

    public AccountServiceTests()
    {
        _fakeAccountRepository = new Mock<IAccountRepository>();
        _fakeUserRepository = new Mock<IUserRepository>();
        _fakeCancellationToken = new CancellationToken();
        _fakeUserService = new Mock<IUserService>();
        _fakeUnitOfWork = new Mock<IUnitOfWork>();
        _fakeCurrencyConversion = new Mock<ICurrencyConversion>();
        _fakeExchangeRateSource = new Mock<IExchangeRateSource>();
        _fakeCloseAccountValidator = new Mock<IValidator<Account>>();
        _fakeMoneyTransferRepository = new Mock<IMoneyTransferRepository>();
        _accountService = new AccountService(_fakeAccountRepository.Object, _fakeCurrencyConversion.Object,
            _fakeMoneyTransferRepository.Object, _fakeUserRepository.Object, _fakeUnitOfWork.Object,
            _fakeCloseAccountValidator.Object);
    }

    [Fact]
    public void Create_SuccessPath_ShouldCreateAccount()
    {
        var account = new Account
        {
            UserId = "someUserId",
            AmountOnAccount = 1000.0,
            Currency = CurrencyEnum.RUB
        };
        
        _fakeUserRepository
            .Setup(repository => repository.Exists(account.UserId, _fakeCancellationToken))
            .ReturnsAsync(true);

        _accountService.Create(account, _fakeCancellationToken);
        
        _fakeAccountRepository.Verify(obj => obj.Create(account, _fakeCancellationToken), Times.Once);
    }
    
    [Fact]
    public void Creat_NonExistentLUserId_ShouldThrowException()
    {
        var account = new Account
        {
            UserId = "someUserId",
            AmountOnAccount = 1000.0,
            Currency = CurrencyEnum.RUB
        };
        _fakeUserRepository
            .Setup(repository => repository.Exists(account.UserId, _fakeCancellationToken))
            .ReturnsAsync(false);

        var exception = Assert
            .ThrowsAsync<ObjectNotFoundException>(() => _accountService.Create(account, _fakeCancellationToken))
            .Result;
        
        Assert.Equal($"Пользователь id={account.UserId} не найден", exception.Message);
    }
    
    [Fact]
    public void Delete_SuccessPath_ShouldDeleteAccount()
    {
        _accountService.Delete("someId", _fakeCancellationToken);

        _fakeAccountRepository.Verify(obj => obj.Delete("someId", _fakeCancellationToken), Times.Once);
    }

    [Fact]
    public void GetById_SuccessPath_ReturnAccount()
    {
        var account = new Account
        {
            UserId = "someUserId",
            AmountOnAccount = 1000.0,
            Currency = CurrencyEnum.RUB
        };
        _fakeAccountRepository.Setup(repository => repository.GetById("someId", _fakeCancellationToken))
            .ReturnsAsync(account);

        var detectedAccount = _accountService.GetById("someId", _fakeCancellationToken).Result;
        
        Assert.Equal(account, detectedAccount);
    }

    [Fact]
    public void GetAll_SuccessPath_ReturnAccounts()
    {
        var accounts = new List<Account>
        {
            new Account
            {
                UserId = "someUserId1",
                AmountOnAccount = 1000.0,
                Currency = CurrencyEnum.RUB
            },
            new Account
            {
                UserId = "someUserId2",
                AmountOnAccount = 1000.0,
                Currency = CurrencyEnum.RUB
            }
        };
        _fakeAccountRepository
            .Setup(repository => repository.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(accounts);

        var detectedAccounts = _accountService.GetAll(_fakeCancellationToken).Result;
        
        Assert.Equal(accounts, detectedAccounts);
    }

    [Fact]
    public void Close_SuccessPath_ShouldCloseAccount()
    {
        var account = new Account
        {
            UserId = "someUserId",
            AmountOnAccount = 0.0,
            Currency = CurrencyEnum.RUB
        };
        _fakeAccountRepository.Setup(repository => repository.GetById("someId", _fakeCancellationToken))
            .ReturnsAsync(account);

        _accountService.Close("someId", _fakeCancellationToken);
        
        _fakeAccountRepository.Verify(repository => repository.CloseAccount("someId", _fakeCancellationToken), Times.Once);
    }
    
    [Fact]
    public void CalculateCommission_BetweenOneUserAccounts_SuccesPath()
    {
        double amount = 100.0;
        string someAccountId = "someAccountId";
        var account = new Account
        {
            UserId = "someUserId",
            AmountOnAccount = 0.0,
            Currency = CurrencyEnum.RUB
        };
        _fakeAccountRepository.Setup(repository => repository.GetById(It.IsAny<string>(), _fakeCancellationToken))
            .ReturnsAsync(account);

        var calculatedCommission = _accountService
            .CalculateCommission(amount, someAccountId, someAccountId, _fakeCancellationToken).Result;

        Assert.Equal(0.0, calculatedCommission);
    }
    
    [Fact]
    public void CalculateCommission_BetweenTwoUserAccounts_SuccesPath()
    {
        double amount = 100.0;
        string someAccountId1 = "someAccountId1";
        string someAccountId2 = "someAccountId2";
        var account1 = new Account
        {
            UserId = "someUserId1",
            AmountOnAccount = 0.0,
            Currency = CurrencyEnum.RUB
        };
        var account2 = new Account
        {
            UserId = "someUserId2",
            AmountOnAccount = 0.0,
            Currency = CurrencyEnum.RUB
        };
        _fakeAccountRepository.Setup(repository => repository.GetById(someAccountId1, _fakeCancellationToken))
            .ReturnsAsync(account1);
        _fakeAccountRepository.Setup(repository => repository.GetById(someAccountId2, _fakeCancellationToken))
            .ReturnsAsync(account2);

        var calculatedCommission = _accountService
            .CalculateCommission(amount, someAccountId1, someAccountId2, _fakeCancellationToken).Result;

        Assert.Equal(2.0, calculatedCommission);
    }

    [Fact]
    public void TransferMoney_SuccessPath_ShouldMoneyTransferCreate()
    {
        var amount = 100.0;
        var fromAccountId = "someAccountId1";
        var toAccountId = "someAccountId2";
        var account1 = new Account
        {
            UserId = "someUserId1",
            AmountOnAccount = 200.0,
            Currency = CurrencyEnum.RUB,
            IsOpen = true
        };
        var account2 = new Account
        {
            UserId = "someUserId1",
            AmountOnAccount = 0.0,
            Currency = CurrencyEnum.RUB,
            IsOpen = true
        };
        _fakeAccountRepository.Setup(repository => repository.GetById(fromAccountId, _fakeCancellationToken))
            .ReturnsAsync(account1);    
        _fakeAccountRepository.Setup(repository => repository.GetById(toAccountId, _fakeCancellationToken))
            .ReturnsAsync(account2);

        _accountService.TransferMoney(amount, fromAccountId, toAccountId, _fakeCancellationToken);
        
        _fakeMoneyTransferRepository.Verify(moneyTransferRep => moneyTransferRep.Create(It.IsAny<MoneyTransfer>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public void TransferMoney_NegativeAmount_ShouldThrowException()
    {
        var amount = -100.0;
        var fromAccountId = "someAccountId1";
        var toAccountId = "someAccountId2";
        var account1 = new Account
        {
            UserId = "someUserId1",
            AmountOnAccount = 200.0,
            Currency = CurrencyEnum.RUB,
            IsOpen = true
        };
        var account2 = new Account
        {
            UserId = "someUserId2",
            AmountOnAccount = 0.0,
            Currency = CurrencyEnum.RUB,
            IsOpen = true
        };
        _fakeAccountRepository.Setup(repository => repository.GetById(fromAccountId, _fakeCancellationToken))
            .ReturnsAsync(account1);    
        _fakeAccountRepository.Setup(repository => repository.GetById(toAccountId, _fakeCancellationToken))
            .ReturnsAsync(account2);

        var exception = Assert
            .ThrowsAsync<ValidationException>(() => _accountService.TransferMoney(amount, fromAccountId, toAccountId, _fakeCancellationToken))
            .Result;
        
        Assert.Equal("Неправильна введена сумма перевода", exception.Message);
    }
    
    [Fact]
    public void TransferMoney_FromAccountClose_ShouldThrowException()
    {
        var amount = 100.0;
        var fromAccountId = "someAccountId1";
        var toAccountId = "someAccountId2";
        var account1 = new Account
        {
            UserId = "someUserId1",
            AmountOnAccount = 200.0,
            Currency = CurrencyEnum.RUB,
            IsOpen = false
        };
        var account2 = new Account
        {
            UserId = "someUserId2",
            AmountOnAccount = 0.0,
            Currency = CurrencyEnum.RUB,
            IsOpen = true
        };
        _fakeAccountRepository.Setup(repository => repository.GetById(fromAccountId, _fakeCancellationToken))
            .ReturnsAsync(account1);    
        _fakeAccountRepository.Setup(repository => repository.GetById(toAccountId, _fakeCancellationToken))
            .ReturnsAsync(account2);

        var exception = Assert
            .ThrowsAsync<ValidationException>(() => _accountService.TransferMoney(amount, fromAccountId, toAccountId, _fakeCancellationToken))
            .Result;
        
        Assert.Equal("Аккаунт отправителя закрыт", exception.Message);
    }
    
    [Fact]
    public void TransferMoney_ToAccountClose_ShouldThrowException()
    {
        var amount = 100.0;
        var fromAccountId = "someAccountId1";
        var toAccountId = "someAccountId2";
        var account1 = new Account
        {
            UserId = "someUserId1",
            AmountOnAccount = 200.0,
            Currency = CurrencyEnum.RUB,
            IsOpen = true
        };
        var account2 = new Account
        {
            UserId = "someUserId2",
            AmountOnAccount = 0.0,
            Currency = CurrencyEnum.RUB,
            IsOpen = false
        };
        _fakeAccountRepository.Setup(repository => repository.GetById(fromAccountId, _fakeCancellationToken))
            .ReturnsAsync(account1);    
        _fakeAccountRepository.Setup(repository => repository.GetById(toAccountId, _fakeCancellationToken))
            .ReturnsAsync(account2);

        var exception = Assert
            .ThrowsAsync<ValidationException>(() => _accountService.TransferMoney(amount, fromAccountId, toAccountId, _fakeCancellationToken))
            .Result;
        
        Assert.Equal("Аккаунт получателя закрыт", exception.Message);
    }
    
    [Fact]
    public void TransferMoney_InsufficienFunds_ShouldThrowException()
    {
        var amount = 100.0;
        var fromAccountId = "someAccountId1";
        var toAccountId = "someAccountId2";
        var account1 = new Account
        {
            UserId = "someUserId1",
            AmountOnAccount = 50.0,
            Currency = CurrencyEnum.RUB,
            IsOpen = true
        };
        var account2 = new Account
        {
            UserId = "someUserId2",
            AmountOnAccount = 0.0,
            Currency = CurrencyEnum.RUB,
            IsOpen = true
        };
        _fakeAccountRepository.Setup(repository => repository.GetById(fromAccountId, _fakeCancellationToken))
            .ReturnsAsync(account1);    
        _fakeAccountRepository.Setup(repository => repository.GetById(toAccountId, _fakeCancellationToken))
            .ReturnsAsync(account2);

        var exception = Assert
            .ThrowsAsync<ValidationException>(() => _accountService.TransferMoney(amount, fromAccountId, toAccountId, _fakeCancellationToken))
            .Result;
        
        Assert.Equal("Недостаточно средств", exception.Message);
    }
}
