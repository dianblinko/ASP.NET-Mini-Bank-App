using Minibank.Core;
using Minibank.Core.Domains.Users;
using Minibank.Core.Domains.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minibank.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {

        private static List<UserDbModel> _userStorage = new List<UserDbModel>();

        void IUserRepository.Create(User user)
        {
            var entity = new UserDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Login = user.Login,
                Email = user.Email
            };

            _userStorage.Add(entity);
        }

        void IUserRepository.Delete(string id)
        {
            var entity = _userStorage.FirstOrDefault(it => it.Id == id);

            if (entity != null)
            {
                _userStorage.Remove(entity);
            }
            else
            {
                throw new ValidationException("Пользователя с таким id не существует");
            }
        }

        User IUserRepository.GetUser(string id)
        {
            var entity = _userStorage.FirstOrDefault(it => it.Id == id);

            if (entity == null)
            {
                return null;
            }

            return new User
            {
                Id = entity.Id,
                Login = entity.Login,
                Email = entity.Email
            };
        }

        IEnumerable<User> IUserRepository.GetAll()
        {
            return _userStorage.Select(it => new User()
            {
                Id = it.Id,
                Login = it.Login,
                Email = it.Email
            });
        }

        void IUserRepository.Update(User user)
        {
            var entity = _userStorage.FirstOrDefault(it => it.Id == user.Id);

            if (entity != null)
            {
                entity.Login = user.Login;
                entity.Email = user.Email;
            }
            else
            {
                throw new ValidationException("Пользователя с таким id не существует");
            }
        }

    }
}
