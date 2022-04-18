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
    private readonly Mock<IUserRepository> _fakeUserRepository;
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<IAccountRepository> _fakeAccountRepository;
    private readonly Mock<IUnitOfWork> _fakeUnitOfWork;
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _fakeAccountRepository = new Mock<IAccountRepository>();
        _fakeUserRepository = new Mock<IUserRepository>();
        _cancellationToken = new CancellationToken();
        _fakeUnitOfWork = new Mock<IUnitOfWork>();
        _userService = new UserService(_fakeUserRepository.Object, _fakeAccountRepository.Object,
            _fakeUnitOfWork.Object);
    }
        
    [Fact]
    public void Create_SuccessPath_ShouldCreateUser()
    {
        var user = new User
        {
            Login = "Artem",
            Email = "qwerty"
        };

        _userService.Create(user, _cancellationToken);
        
        _fakeUserRepository.Verify(obj => obj.Create(user, _cancellationToken), Times.Once);
    }
    
    [Fact]
    public void Delete_WithAccount_ShouldThrowException()
    {
        _fakeAccountRepository
            .Setup(repository => repository.ExistForUserId(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        var exception = Assert.ThrowsAsync<ValidationException>(() => _userService.Delete("someId", _cancellationToken)).Result;
        
        Assert.Equal("Нельзя удалить пользователя с привязанными аккаунтами", exception.Message);
    }
    
    [Fact]
    public void Delete_SuccessPath_ShouldDeleteUser()
    {
        _fakeAccountRepository
            .Setup(repository => repository.ExistForUserId(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        _userService.Delete("someId", _cancellationToken);

        _fakeUserRepository.Verify(obj => obj.Delete("someId", _cancellationToken), Times.Once);
    }
    
    [Fact]
    public void GetUser_SuccessPath_ReturnUser()
    {
        var user = new User
        {
            Login = "someLogin"
        };
        _fakeUserRepository
            .Setup(repository => repository.GetUser("someId", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        var expectedUser = _userService.GetUser("someId", _cancellationToken).Result;

        Assert.Equal(user, expectedUser);
    }
    
    [Fact]
    public void GetAll_SuccessPath_ReturnUsers()
    {
        var users = new List<User>
        {
            new User{ Login = "someLogin1"},
            new User{ Login = "someLogin2"}
        };
        _fakeUserRepository
            .Setup(repository => repository.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);
        
        var expectedUsers = _userService.GetAll(_cancellationToken).Result;

        Assert.Equal(users, expectedUsers);
    }
    
    [Fact]
    public void Update_SuccessPath_UpdateUser()
    {
        var user = new User
        {
            Login = "someLogin"
        };

        _userService.Update(user, _cancellationToken);

        _fakeUserRepository.Verify(obj => obj.Update(user, _cancellationToken), Times.Once);
    }
}
