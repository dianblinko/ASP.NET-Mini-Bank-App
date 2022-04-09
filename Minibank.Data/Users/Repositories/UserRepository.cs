using Minibank.Core;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task Create(User user, CancellationToken cancellationToken)
        {
            var entity = new UserDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Login = user.Login,
                Email = user.Email
            };

            await _context.Users.AddAsync(entity, cancellationToken);
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == id, 
                cancellationToken: cancellationToken);
            
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Пользователь id={id} не найден");
            }

            _context.Users.Remove(entity);
        }

        public async Task<User> GetUser(string id, CancellationToken cancellationToken)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(it => it.Id == id, cancellationToken: cancellationToken);
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

        public Task<List<User>> GetAll(CancellationToken cancellationToken)
        {
            return _context.Users
                .AsNoTracking()
                .Select(it => new User()
                {
                    Id = it.Id,
                    Login = it.Login,
                    Email = it.Email
                }).ToListAsync(cancellationToken);
        }

        public async Task Update(User user, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == user.Id, 
                cancellationToken: cancellationToken);
            if (entity == null)
            {
                throw new ObjectNotFoundException($"Пользователь id={user.Id} не найден");
            }

            entity.Login = user.Login;
            entity.Email = user.Email;
        }

        public Task<bool> Exists(string id, CancellationToken cancellationToken)
        {
            return _context.Users
                .AsNoTracking()
                .AnyAsync(it => it.Id == id, cancellationToken: cancellationToken);
        }
    }
}
