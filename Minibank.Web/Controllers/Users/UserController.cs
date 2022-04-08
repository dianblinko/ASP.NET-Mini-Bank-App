using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Services;
using Minibank.Web.Controllers.Users.Dto;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<UserDto> Get(string id)
        {
            var model = await _userService.GetUser(id);
            return new UserDto
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email
            };
        }

        [HttpGet]
        public async Task<List<UserDto>> GetAll()
        {
            return (await _userService.GetAll())
                .Select(it => new UserDto
                {
                    Id = it.Id,
                    Login = it.Login,
                    Email = it.Email
                }).ToList();
        }

        [HttpPost]
        public Task Create(UserDtoCreate model)
        {
            return _userService.Create(new User
            {
                Login = model.Login,
                Email = model.Email
            });
        }

        [HttpPut("{id}")]
        public Task Update(string id, UserDtoCreate model)
        {
            return _userService.Update(new User
            {
                Id = id,
                Login = model.Login,
                Email = model.Email
            });
        }

        [HttpDelete("{id}")]
        public Task Delete(string id)
        {
            return _userService.Delete(id);
        }
    }
}
