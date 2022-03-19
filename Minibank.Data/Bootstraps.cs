using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core;
using Minibank.Core.Domains.Accounts.Repositories;
using Minibank.Core.Domains.MoneyTransfers.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Data.Accounts.Repositories;
using Minibank.Data.MoneyTransfers.Repositories;
using Minibank.Data.HttpClients.Models;
using Minibank.Data.Users.Repositories;
using System;


namespace Minibank.Data
{
    public static class Bootstraps
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IExchangeRateSource, ExchangeRateSource>(options =>
            {
                options.BaseAddress = new Uri(configuration["ExchangeRateSourseUri"]);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IMoneyTransferRepository, MoneyTransferRepository>();
            return services;
        }
    }
}
