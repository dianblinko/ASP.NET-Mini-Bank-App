using System;
using Minibank.Core.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;
using Minibank.Data.Users;

namespace Minibank.Data.Accounts
{
    public class AccountDbModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public UserDbModel User { get; set; }
        public double AmoumtOnAccount { get; set; }
        public CurrencyEnum Currency { get; set; }
        public bool IsOpen { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
    }

    internal class Map : IEntityTypeConfiguration<AccountDbModel>
    {
        public void Configure(EntityTypeBuilder<AccountDbModel> builder)
        {
            builder.ToTable("account");
            /*builder.HasOne(it => it.User)
                .WithMany(it => it.)
                //на 1:30 было*/
        }
    }
}
