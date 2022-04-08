using Minibank.Core;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Minibank.Data.Context;

namespace Minibank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MinibankContext _context;

        public UserRepository(MinibankContext context)
        {
            _context = context;
        }

        public void Create(User user)
        {
            var entity = new UserDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Login = user.Login,
                Email = user.Email
            };

            _context.Users.Add(entity);
        }

        public void Delete(string id)
        {
            var entity = _context.Users.FirstOrDefault(it => it.Id == id);
            
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Пользователь id={id} не найден");
            }

            _context.Users.Remove(entity);
        }

        public User GetUser(string id)
        {
            var entity = _context.Users
                .AsNoTracking()
                .FirstOrDefault(it => it.Id == id);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Пользователь id={id} не найден");
            }

            return new User
            {
                Id = entity.Id,
                Login = entity.Login,
                Email = entity.Email
            };
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users
                .AsNoTracking()
                .Select(it => new User()
                {
                    Id = it.Id,
                    Login = it.Login,
                    Email = it.Email
                });
        }

        public void Update(User user)
        {
            var entity = _context.Users.FirstOrDefault(it => it.Id == user.Id);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Пользователь id={user.Id} не найден");
            }

            entity.Login = user.Login;
            entity.Email = user.Email;
        }

        public bool Exists(string id)
        {
            return _context.Users
                .AsNoTracking()
                .Any(it => it.Id == id);
        }
    }
}
