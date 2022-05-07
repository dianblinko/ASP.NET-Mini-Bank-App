using Minibank.Core.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Minibank.Data.MoneyTransfers
{
    public class MoneyTransferDbModel
    {
        public string Id { get; set; }
        public double Amount { get; set; }
        public CurrencyEnum Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }

    internal class Map : IEntityTypeConfiguration<MoneyTransferDbModel>
    {
        public void Configure(EntityTypeBuilder<MoneyTransferDbModel> builder)
        {
            builder.ToTable("money_transfer");
        }
    }
}
