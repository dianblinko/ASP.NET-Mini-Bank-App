﻿using System;
using System.Collections.Generic;

namespace Minibank.Core.Domains.Accounts.Services
{
    public interface IAccountService
    {
        Account GetById(string id);
        IEnumerable<Account> GetAll();
        void Create(Account account);
        void Delete(string id);
        void Close(String id);
        double CalculateCommission(double amount, string fromAccountId, string toAccountId);
        void TransferMoney(double amount, string fromAccountId, string toAccountId);
    }
}