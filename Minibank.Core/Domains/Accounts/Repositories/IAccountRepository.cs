﻿using System.Collections.Generic;

namespace Minibank.Core.Domains.Accounts.Repositories
{
    public interface IAccountRepository
    {
        Account GetById(string id);
        IEnumerable<Account> GetAll();
        void Create(Account account);
        void Update(Account account);
        void Delete(string id);
        bool ExistForUserId(string userId);
        void CloseAccount(string id);
        void SubAmount(string id, double amount);
        void AddAmount(string id, double amount);
        bool Exists(string id);
    }
}