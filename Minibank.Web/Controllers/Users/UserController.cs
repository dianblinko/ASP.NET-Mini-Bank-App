using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Services;
using Minibank.Web.Controllers.Users.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Minibank.Web.Controllers.Users
{
    [ApiController]
    [Route("[controller]")]
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<UserDto> Get(string id, CancellationToken cancellationToken)
        {
            var model = await _userService.GetUser(id, cancellationToken);
            return new UserDto
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email
            };
        }

        [HttpGet]
        public async Task<List<UserDto>> GetAll(CancellationToken cancellationToken)
        {
            return (await _userService.GetAll(cancellationToken))
                .Select(it => new UserDto
                {
                    Id = it.Id,
                    Login = it.Login,
                    Email = it.Email
                }).ToList();
        }

        [HttpPost]
        public Task Create(UserDtoCreate model, CancellationToken cancellationToken)
        {
            return _userService.Create(new User
            {
                Login = model.Login,
                Email = model.Email
            },
                cancellationToken);
        }

        [HttpPut("{id}")]
        public Task Update(string id, UserDtoCreate model, CancellationToken cancellationToken)
        {
            return _userService.Update(new User
            {
                Id = id,
                Login = model.Login,
                Email = model.Email
            },
                cancellationToken);
        }

        [HttpDelete("{id}")]
        public Task Delete(string id, CancellationToken cancellationToken)
        {
            return _userService.Delete(id, cancellationToken);
        }
    }
}
