using System;
using Microsoft.EntityFrameworkCore;
using Minibank.Data.Accounts;
using Minibank.Data.Users;
using Minibank.Data.MoneyTransfers;
using Microsoft.EntityFrameworkCore.Design;

namespace Minibank.Data.Context
{
    public class MinibankContext : DbContext
    {
        public DbSet<UserDbModel> Users { get; set; }
        public DbSet<AccountDbModel> Accounts { get; set; }
        public DbSet<MoneyTransferDbModel> MoneyTransfer { get; set; }

        public MinibankContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MinibankContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql().UseSnakeCaseNamingConvention();
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }
    }
    
    public class Factory : IDesignTimeDbContextFactory<MinibankContext>
     {
         public MinibankContext CreateDbContext(string[] args)
         {
             var options = new DbContextOptionsBuilder()
                 .UseNpgsql("FakeConnectionStringOnlyForMigrations")
                 .Options;
             return new MinibankContext(options);
         }
     }
}
