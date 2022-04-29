using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Minibank.Core.Domains;
using Minibank.Core.Domains.Accounts;
using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.Accounts.Services;
using Minibank.Core.Domains.MoneyTransfers;
using Minibank.Core.Domains.MoneyTransfers.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace Minibank.Core.Tests;

public class AccountServiceTests
{

    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICurrencyConversion> _currencyConversionMock;
    private readonly Mock<IMoneyTransferRepository> _moneyTransferRepositoryMock;
    private readonly Mock<IValidator<Account>> _closeAccountValidatorMock;
    private readonly AccountService _accountService;
    private readonly CancellationToken _cancellationToken;

    public AccountServiceTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _cancellationToken = new CancellationToken();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _currencyConversionMock = new Mock<ICurrencyConversion>();
        _closeAccountValidatorMock = new Mock<IValidator<Account>>();
        _moneyTransferRepositoryMock = new Mock<IMoneyTransferRepository>();
        _accountService = new AccountService(_accountRepositoryMock.Object, _currencyConversionMock.Object,
            _moneyTransferRepositoryMock.Object, _userRepositoryMock.Object, _unitOfWorkMock.Object,
            _closeAccountValidatorMock.Object);
    }

    [Fact]
    public async Task Create_SuccessPath_ShouldCreateAccount()
    {
        //ARRANGE
        var account = new Account
        {
            UserId = "someUserId",
            AmountOnAccount = 1000.0,
            Currency = CurrencyEnum.RUB
        };
        _userRepositoryMock
            .Setup(repository => repository.Exists(account.UserId, _cancellationToken))
            .Returns(Task.FromResult(true));

        //ACT
        await _accountService.Create(account, _cancellationToken);
        
        //ASSERT
        _accountRepositoryMock.Verify(obj => obj.Create(account, _cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(obj => obj.SaveChanges());
    }
    
    [Fact]
    public async Task Create_SuccessPath_CreateRightAccountFields()
    {
        //ARANGE
        var id = "someId";
        var amount = 1000.0;
        var currency = CurrencyEnum.RUB;
        var isOpen = true;
        DateTime? closingDate = null;
        var openingDate = DateTime.Now;
        var userId = "userId";
        var account = new Account
        {
            Id = id,
            UserId = userId,
            AmountOnAccount = amount,
            Currency = currency,
            IsOpen = isOpen,
            ClosingDate = closingDate,
            OpeningDate = openingDate,
        };
        _userRepositoryMock
            .Setup(repository => repository.Exists(account.UserId, _cancellationToken))
            .Returns(Task.FromResult(true));

        //ACT
        await _accountService.Create(account, _cancellationToken);
        
        //ASSERT
        _accountRepositoryMock.Verify(
            obj => obj.Create(
                It.Is<Account>(a =>
                    a.Id == id && a.UserId == userId && a.Currency == currency && Math.Abs(a.AmountOnAccount - amount) < 0.001 &&
                    a.IsOpen == isOpen && a.ClosingDate == closingDate && a.OpeningDate == openingDate),
                _cancellationToken), Times.Once);
    }
    
    [Fact]
    public async Task Create_NonExistentLUserId_ShouldThrowException()
    {
        //ARRANGE
        var account = new Account
        {
            UserId = "someUserId",
            AmountOnAccount = 1000.0,
            Currency = CurrencyEnum.RUB
        };
        _userRepositoryMock
            .Setup(repository => repository.Exists(account.UserId, _cancellationToken))
            .Returns(Task.FromResult(false));

        //ACT
        var exception = await Assert
            .ThrowsAsync<ObjectNotFoundException>(async () => await _accountService.Create(account, _cancellationToken));
        
        //ASSERT
        Assert.Equal($"Пользователь id={account.UserId} не найден", exception.Message);
    }
    
    [Fact]
    public async Task Delete_SuccessPath_ShouldDeleteAccount()
    {
        //ARRANGE
        var someId = "someId";
        
        //ACT
        await _accountService.Delete(someId, _cancellationToken);

        //ASSERT
        _accountRepositoryMock.Verify(obj => obj.Delete(someId, _cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(obj => obj.SaveChanges());
    }

    [Fact]
    public async Task GetById_SuccessPath_ReturnAccount()
    {
        //ARRANGE
        var someId = "someId";
        var account = new Account
        {
            UserId = "someUserId",
            AmountOnAccount = 1000.0,
            Currency = CurrencyEnum.RUB
        };
        _accountRepositoryMock.Setup(repository => repository.GetById(someId, _cancellationToken))
            .Returns(Task.FromResult(account));

        //ACT
        var detectedAccount = await _accountService.GetById(someId, _cancellationToken);
        
        //ASSERT
        Assert.Equal(account, detectedAccount);
    }
    
    [Fact]
    public async Task GetById_SuccessPath_ReturnRightAccountFields()
    {
        //ARRANGE
        var id = "someId";
        var amount = 1000.0;
        var currency = CurrencyEnum.RUB;
        var isOpen = true;
        DateTime? closingDate = null;
        var openingDate = DateTime.Now;
        var userId = "userId";
        var account = new Account
        {
            Id = id,
            UserId = userId,
            AmountOnAccount = amount,
            Currency = currency,
            IsOpen = isOpen,
            ClosingDate = closingDate,
            OpeningDate = openingDate,
        };
        _accountRepositoryMock.Setup(repository => repository.GetById(id, _cancellationToken))
            .Returns(Task.FromResult(account));

        //ACT
        var detectedAccount = await _accountService.GetById(id, _cancellationToken);
        
        //ASSERT
        Assert.True(account.Id == id && account.UserId == userId && account.Currency == currency && Math.Abs(account.AmountOnAccount - amount) < 0.001 &&
                    account.IsOpen == isOpen && account.ClosingDate == closingDate && account.OpeningDate == openingDate);
    }

    [Fact]
    public async Task GetAll_SuccessPath_ReturnAccounts()
    {
        //ARRANGE
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
        _accountRepositoryMock
            .Setup(repository => repository.GetAll(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(accounts));

        //ACT
        var detectedAccounts = await _accountService.GetAll(_cancellationToken);
        
        //ASSERT
        Assert.Equal(accounts, detectedAccounts);
    }

    [Fact]
    public async Task Close_SuccessPath_ShouldCloseAccount()
    {
        //ARRANGE
        var someId = "someId";
        var account = new Account
        {
            UserId = "someUserId",
            AmountOnAccount = 0.0,
            Currency = CurrencyEnum.RUB
        };
        _accountRepositoryMock.Setup(repository => repository.GetById(someId, _cancellationToken))
            .Returns(Task.FromResult(account));

        //ACT
        await _accountService.Close(someId, _cancellationToken);
        
        //ASSERT
        _accountRepositoryMock.Verify(repository => repository.CloseAccount(someId, _cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(obj => obj.SaveChanges());
    }

    [Fact]
    public async Task CalculateCommission_BetweenOneUserAccounts_SuccessPath()
    {
        //ARRANGE
        double amount = 100.0;
        string someAccountId = "someAccountId";
        var account = new Account
        {
            UserId = "someUserId",
            AmountOnAccount = 0.0,
            Currency = CurrencyEnum.RUB
        };
        _accountRepositoryMock.Setup(repository => repository.GetById(someAccountId, _cancellationToken))
            .Returns(Task.FromResult(account));

        //ACT
        var calculatedCommission = await _accountService
            .CalculateCommission(amount, someAccountId, someAccountId, _cancellationToken);

        //ASSERT
        Assert.Equal(0.0, calculatedCommission);
    }
    
    [Fact]
    public async Task CalculateCommission_BetweenTwoUserAccounts_SuccessPath()
    {
        //ARRANGE
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
        _accountRepositoryMock.Setup(repository => repository.GetById(someAccountId1, _cancellationToken))
            .Returns(Task.FromResult(account1));
        _accountRepositoryMock.Setup(repository => repository.GetById(someAccountId2, _cancellationToken))
            .Returns(Task.FromResult(account2));

        //ACT
        var calculatedCommission = await _accountService
            .CalculateCommission(amount, someAccountId1, someAccountId2, _cancellationToken);

        //ASSERT
        Assert.Equal(2.0, calculatedCommission);
    }

    [Fact]
    public async Task TransferMoney_SuccessPath_ShouldMoneyTransferCreate()
    {
        //ARRANGE
        var amount = 100.001;
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
        _accountRepositoryMock.Setup(repository => repository.GetById(fromAccountId, _cancellationToken))
            .Returns(Task.FromResult(account1));    
        _accountRepositoryMock.Setup(repository => repository.GetById(toAccountId, _cancellationToken))
            .Returns(Task.FromResult(account2));

        //ACT
        await _accountService.TransferMoney(amount, fromAccountId, toAccountId, _cancellationToken);

        //ASSERT
        amount = Math.Round(amount, 2);
        _moneyTransferRepositoryMock.Verify(
            moneyTransferRep => moneyTransferRep.Create(
                It.Is<MoneyTransfer>(mt =>
                    Math.Abs(mt.Amount - amount) < 0.001 && mt.Currency == account2.Currency && mt.FromAccountId == fromAccountId &&
                    mt.ToAccountId == toAccountId),
                _cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(obj => obj.SaveChanges());
    }
    
    [Fact]
    public async Task TransferMoney_SuccessPath_UpdateFromAccount()
    {
        //ARRANGE
        var amount = 100.001;
        var account1Amount = 200.0;
        var fromAccountId = "someAccountId1";
        var toAccountId = "someAccountId2";
        var account1 = new Account
        {
            Id = fromAccountId,
            UserId = "someUserId1",
            AmountOnAccount = account1Amount,
            Currency = CurrencyEnum.RUB,
            IsOpen = true
        };
        var account2 = new Account
        {
            Id = toAccountId,
            UserId = "someUserId1",
            AmountOnAccount = 0.0,
            Currency = CurrencyEnum.RUB,
            IsOpen = true
        };
        _accountRepositoryMock.Setup(repository => repository.GetById(fromAccountId, _cancellationToken))
            .Returns(Task.FromResult(account1));    
        _accountRepositoryMock.Setup(repository => repository.GetById(toAccountId, _cancellationToken))
            .Returns(Task.FromResult(account2));

        //ACT
        await _accountService.TransferMoney(amount, fromAccountId, toAccountId, _cancellationToken);

        //ASSERT
        amount = Math.Round(amount, 2);
        account1Amount -= amount;
        _accountRepositoryMock.Verify(rep => rep.Update(It.Is<Account>(a => a.Id == fromAccountId && Math.Abs(a.AmountOnAccount - account1Amount) < 0.001),_cancellationToken), Times.Once);
    }
    
    [Fact]
    public async Task TransferMoney_SuccessPath_UpdateToAccount()
    {
        //ARRANGE
        var amount = 100.001;
        var fromAccountId = "someAccountId1";
        var toAccountId = "someAccountId2";
        var account2Amount = 0.0;
        var account1 = new Account
        {
            Id = fromAccountId,
            UserId = "someUserId1",
            AmountOnAccount = 200.0,
            Currency = CurrencyEnum.RUB,
            IsOpen = true
        };
        var account2 = new Account
        {
            Id = toAccountId,
            UserId = "someUserId1",
            AmountOnAccount = 0.0,
            Currency = CurrencyEnum.RUB,
            IsOpen = true
        };
        _accountRepositoryMock.Setup(repository => repository.GetById(fromAccountId, _cancellationToken))
            .Returns(Task.FromResult(account1));    
        _accountRepositoryMock.Setup(repository => repository.GetById(toAccountId, _cancellationToken))
            .Returns(Task.FromResult(account2));

        //ACT
        await _accountService.TransferMoney(amount, fromAccountId, toAccountId, _cancellationToken);

        //ASSERT
        amount = Math.Round(amount, 2);
        account2Amount += amount;
        _accountRepositoryMock.Verify(rep => rep.Update(It.Is<Account>(a => a.Id == toAccountId && Math.Abs(a.AmountOnAccount - account2Amount) < 0.001),_cancellationToken), Times.Once);
    }
    
    [Fact]
    public async Task TransferMoney_NegativeAmount_ShouldThrowException()
    {
        //ARRANGE
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
        _accountRepositoryMock.Setup(repository => repository.GetById(fromAccountId, _cancellationToken))
            .Returns(Task.FromResult(account1));    
        _accountRepositoryMock.Setup(repository => repository.GetById(toAccountId, _cancellationToken))
            .Returns(Task.FromResult(account2));

        //ACT
        var exception = await Assert
            .ThrowsAsync<ValidationException>(() => _accountService.TransferMoney(amount, fromAccountId, toAccountId, _cancellationToken));
        
        //ASSERT
        Assert.Equal("Неправильна введена сумма перевода", exception.Message);
    }
    
    [Fact]
    public async Task TransferMoney_FromAccountClose_ShouldThrowException()
    {
        //ARRANGE
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
        _accountRepositoryMock.Setup(repository => repository.GetById(fromAccountId, _cancellationToken))
            .Returns(Task.FromResult(account1));    
        _accountRepositoryMock.Setup(repository => repository.GetById(toAccountId, _cancellationToken))
            .Returns(Task.FromResult(account2));

        //ACT
        var exception = await Assert
            .ThrowsAsync<ValidationException>(() => _accountService.TransferMoney(amount, fromAccountId, toAccountId, _cancellationToken));
        
        //ASSERT
        Assert.Equal("Аккаунт отправителя закрыт", exception.Message);
    }
    
    [Fact]
    public async Task TransferMoney_ToAccountClose_ShouldThrowException()
    {
        //ARRANGE
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
        _accountRepositoryMock.Setup(repository => repository.GetById(fromAccountId, _cancellationToken))
            .Returns(Task.FromResult(account1));    
        _accountRepositoryMock.Setup(repository => repository.GetById(toAccountId, _cancellationToken))
            .Returns(Task.FromResult(account2));

        //ACT
        var exception = await Assert
            .ThrowsAsync<ValidationException>(() => _accountService.TransferMoney(amount, fromAccountId, toAccountId, _cancellationToken));
        
        //ASSERT
        Assert.Equal("Аккаунт получателя закрыт", exception.Message);
    }
    
    [Fact]
    public async Task TransferMoney_InsufficientFunds_ShouldThrowException()
    {
        //ARRANGE
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
        _accountRepositoryMock.Setup(repository => repository.GetById(fromAccountId, _cancellationToken))
            .Returns(Task.FromResult(account1));    
        _accountRepositoryMock.Setup(repository => repository.GetById(toAccountId, _cancellationToken))
            .Returns(Task.FromResult(account2));

        //ACT
        var exception = await Assert
            .ThrowsAsync<ValidationException>(() => _accountService.TransferMoney(amount, fromAccountId, toAccountId, _cancellationToken));
        
        //ASSERT
        Assert.Equal("Недостаточно средств", exception.Message);
    }
}
