﻿using CoreBankingTest.Core.Entities;
using CoreBankingTest.Core.Enums;
using CoreBankingTest.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBankingTest.Infrastructure.Data
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options)
            : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.CustomerId);
                entity.Property(c => c.CustomerId)
                    .HasConversion(customerId => customerId.Value,
                                value => CustomerId.Create(value));

                entity.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(c => c.LastName).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(255);
                entity.Property(c => c.PhoneNumber).HasMaxLength(20);

                // Customer has many Accounts
                entity.HasMany(c => c.Accounts)
                    .WithOne(a => a.Customer)
                    .HasForeignKey(a => a.CustomerId);
            });

            // Account configuration
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(a => a.AccountId);
                entity.Property(c => c.AccountId)
                    .HasConversion(AccountId => AccountId.Value,
                                value => CoreBankingTest.Core.ValueObjects.AccountId.Create(value));

                // Configure AccountNumber as owned type (Value Object)
                entity.OwnsOne(a => a.AccountNumber, an =>
                {
                    an.Property(a => a.Value)
                    .HasColumnName("AccountNumber")
                    .IsRequired()
                    .HasMaxLength(10);
                });

                // Configure Money as owned type (Value Object)
                entity.OwnsOne(a => a.Balance, money =>
                {
                    money.Property(m => m.Amount)
                        .HasColumnName("Amount")
                        .HasPrecision(18, 2);
                    money.Property(m => m.Currency)
                        .HasColumnName("Currency")
                        .HasMaxLength(3)
                        .HasDefaultValue(CurrencyType.NGN);
                });

                entity.Property(a => a.AccountType)
                    .HasConversion<string>()
                    .IsRequired();

                // Account has many Transactions
                entity.HasMany(a => a.Transactions)
                    .WithOne(t => t.Account)
                    .HasForeignKey(t => t.AccountId);

                // Ensure we don't accidentally load all transactions
                entity.Navigation(a => a.Transactions).AutoInclude(false);
            });

            // Transaction configuration
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.TransactionId);
                entity.Property(c => c.TransactionId)
                    .HasConversion(TransactionId => TransactionId.Value,
                                value => TransactionId.Create(value));

                // Configure Money as owned type
                entity.OwnsOne(t => t.Amount, money =>
                {
                    money.Property(m => m.Amount)
                        .HasColumnName("Amount")
                        .HasPrecision(18, 2);
                    money.Property(m => m.Currency)
                        .HasColumnName("Currency")
                        .HasMaxLength(3);
                });

                entity.Property(t => t.Type)
                    .HasConversion<string>()
                    .IsRequired();

                entity.Property(t => t.Description).HasMaxLength(500);
                entity.Property(t => t.Reference).HasMaxLength(50);
                entity.Property(t => t.Timestamp).IsRequired();
            });

            // Global query filter in DbContext - Automatically Exclude Deleted Records
            modelBuilder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Account>().HasQueryFilter(a => !a.IsDeleted);
            modelBuilder.Entity<Transaction>().HasQueryFilter(t => !t.Account.IsDeleted);

            // Account concurrency implementation
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(a => a.RowVersion)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            // Seed the DB
            modelBuilder.Entity<Customer>().HasData(new
            {
                CustomerId = CustomerId.Create(Guid.Parse("a1b2c3d4-1234-5678-9abc-123456789abc")),
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@email.com",
                PhoneNumber = "555-0101",
                DateCreated = DateTime.UtcNow.AddDays(-30),
                IsActive = true,
                IsDeleted = false
            }
            );

            modelBuilder.Entity<Account>().HasData(new
            {
                AccountId = CoreBankingTest.Core.ValueObjects.AccountId.Create(Guid.Parse("c3d4e5f6-3456-7890-cde1-345678901cde")),
                AccountType = AccountType.Savings, // EF handles enum conversion
                CustomerId = CustomerId.Create(Guid.Parse("a1b2c3d4-1234-5678-9abc-123456789abc")),
                Currency = CurrencyType.NGN,
                DateOpened = DateTime.UtcNow.AddDays(-20),
                IsActive = true,
                IsDeleted = false
            }
            );

            // Then configure the owned types separately
            modelBuilder.Entity<Account>().OwnsOne(a => a.AccountNumber).HasData(
                new
                {
                    AccountId = CoreBankingTest.Core.ValueObjects.AccountId.Create(Guid.Parse("c3d4e5f6-3456-7890-cde1-345678901cde")),
                    Value = "1000000001"
                }
            );

            modelBuilder.Entity<Account>().OwnsOne(a => a.Balance).HasData(
                new
                {
                    AccountId = CoreBankingTest.Core.ValueObjects.AccountId.Create(Guid.Parse("c3d4e5f6-3456-7890-cde1-345678901cde")),
                    Amount = 1500.00m,
                    Currency = CurrencyType.NGN
                }
            );




        }
    }
}
