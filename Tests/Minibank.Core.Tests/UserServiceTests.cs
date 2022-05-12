using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domains.Users.Services;
using Moq;
using Xunit;

namespace Minibank.Core.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _cancellationToken = new CancellationToken();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userService = new UserService(_userRepositoryMock.Object, _accountRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }
        
    [Fact]
    public async Task Create_SuccessPath_ShouldCreateUser()
    {
        //ARRANGE
        var user = new User
        {
            Login = "Artem",
            Email = "qwerty"
        };
        
        //ACT
        await _userService.Create(user, _cancellationToken);
        
        //ASSERT
        _userRepositoryMock.Verify(obj => obj.Create(user, _cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(obj => obj.SaveChanges());
    }
    [Fact]
    public async Task Create_SuccessPath_RightUserFields()
    {
        //ARRANGE
        var login = "Artem";
        var email = "qwerty";
        var testUser = new User
        {
            Login = login,
            Email = email
        };
        
        //ACT
        await _userService.Create(testUser, _cancellationToken);
        
        //ASSERT
        _userRepositoryMock.Verify(
            obj => obj.Create(
                It.Is<User>(user =>
                    user.Id == null && user.Login == login && user.Email == email),
                _cancellationToken), Times.Once);
    }
    
    [Fact]
    public async Task Delete_WithAccount_ShouldThrowException()
    {
        //ARRANGE
        var someId = "someId";
        _accountRepositoryMock
            .Setup(repository => repository.ExistForUserId(someId, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));
        
        //ACT
        var exception = await Assert.ThrowsAsync<ValidationException>(async () => await _userService.Delete(someId, _cancellationToken));
        
        //ASSERT
        Assert.Equal("Нельзя удалить пользователя с привязанными аккаунтами", exception.Message);
    }
    
    [Fact]
    public async Task Delete_SuccessPath_ShouldDeleteUser()
    {
        //ARRANGE
        var someId = "someId";
        _accountRepositoryMock
            .Setup(repository => repository.ExistForUserId(someId, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(false));
        
        //ACT
        await _userService.Delete(someId, _cancellationToken);

        //ASSERT
        _userRepositoryMock.Verify(obj => obj.Delete(someId, _cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(obj => obj.SaveChanges());
    }
    
    [Fact]
    public async Task GetUser_SuccessPath_ReturnUser()
    {
        //ARRANGE
        var someId = "someId";
        var user = new User
        {
            Login = "someLogin"
        };
        _userRepositoryMock
            .Setup(repository => repository.GetUser(someId, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(user));
        
        //ACT
        var expectedUser = await _userService.GetUser(someId, _cancellationToken);

        //ASSERT
        Assert.Equal(user, expectedUser);
    }
    
    [Fact]
    public async Task GetUser_SuccessPath_ReturnRightUserFields()
    {
        //ARRANGE
        var someId = "someId";
        var login = "someLogin";
        var email = "someEmail";
        var user = new User
        {
            Id = someId,
            Login = login,
            Email = email
        };
        _userRepositoryMock
            .Setup(repository => repository.GetUser(someId, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(user));
        
        //ACT
        var expectedUser = await _userService.GetUser(someId, _cancellationToken);

        //ASSERT
        Assert.True(expectedUser.Id == someId && expectedUser.Login == login && expectedUser.Email == email);
    }
    
    [Fact]
    public async Task GetAll_SuccessPath_ReturnUsers()
    {
        //ARRANGE
        var users = new List<User>
        {
            new User{ Login = "someLogin1"},
            new User{ Login = "someLogin2"}
        };
        _userRepositoryMock
            .Setup(repository => repository.GetAll(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(users));
        
        //ACT
        var expectedUsers = await _userService.GetAll(_cancellationToken);

        //ASSERT
        Assert.Equal(users, expectedUsers);
    }
    
    [Fact]
    public async Task Update_SuccessPath_UpdateUser()
    {
        //ARRANGE
        var user = new User
        {
            Id = "someId",
            Login = "someLogin",
            Email = "someEmail"
        };

        //ACT
        await _userService.Update(user, _cancellationToken);

        //ASSERT
        _userRepositoryMock.Verify(obj => obj.Update(user, _cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(obj => obj.SaveChanges());
    }
    
    [Fact]
    public async Task Update_SuccessPath_UpdateRightUserFields()
    {
        //ARRANGE
        var someId = "someId";
        var login = "someLogin";
        var email = "someEmail";
        var user = new User
        {
            Id = someId,
            Login = login,
            Email = email
        };

        //ACT
        await _userService.Update(user, _cancellationToken);

        //ASSERT
        _userRepositoryMock.Verify(
            obj => obj.Update(It.Is<User>(user => user.Id == someId && user.Email == email && user.Login == login),
                _cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(obj => obj.SaveChanges());
    }
}
