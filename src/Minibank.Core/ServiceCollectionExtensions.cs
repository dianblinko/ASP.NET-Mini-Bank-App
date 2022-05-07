using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core.Domains.Accounts.Services;
using Minibank.Core.Domains.MoneyTransfers.Services;
using Minibank.Core.Domains.Users.Services;

namespace Minibank.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrencyConversion, CurrencyConversion>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IMoneyTransferService, MoneyTransferService>();
            services.AddFluentValidation().AddValidatorsFromAssembly(typeof(AccountService).Assembly);
            return services;
        }
    }
}
