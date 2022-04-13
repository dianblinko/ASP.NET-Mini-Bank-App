﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Minibank.Data.Context;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Minibank.Data.Migrations
{
    [DbContext(typeof(MinibankContext))]
    [Migration("20220405161319_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("Minibank.Data.Accounts.AccountDbModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<double>("AmoumtOnAccount")
                        .HasColumnType("double precision")
                        .HasColumnName("amount_on_account");

                    b.Property<DateTime?>("ClosingDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("closing_date");

                    b.Property<int>("Currency")
                        .HasColumnType("integer")
                        .HasColumnName("currency");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("boolean")
                        .HasColumnName("is_open");

                    b.Property<DateTime>("OpeningDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("opening_date");

                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("account");
                });

            modelBuilder.Entity("Minibank.Data.MoneyTransfers.MoneyTransferDbModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision")
                        .HasColumnName("amount");

                    b.Property<int>("Currency")
                        .HasColumnType("integer")
                        .HasColumnName("currency");

                    b.Property<string>("FromAccountId")
                        .HasColumnType("text")
                        .HasColumnName("from_account_id");

                    b.Property<string>("ToAccountId")
                        .HasColumnType("text")
                        .HasColumnName("to_account_id");

                    b.HasKey("Id");

                    b.ToTable("money_transfer");
                });

            modelBuilder.Entity("Minibank.Data.Users.UserDbModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Login")
                        .HasColumnType("text")
                        .HasColumnName("login");

                    b.HasKey("Id");

                    b.ToTable("user");
                });

            modelBuilder.Entity("Minibank.Data.Accounts.AccountDbModel", b =>
                {
                    b.HasOne("Minibank.Data.Users.UserDbModel", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}