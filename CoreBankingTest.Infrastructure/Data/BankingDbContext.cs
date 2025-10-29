using CoreBankingTest.Core.Entities;
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
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customer Confguration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.CustomerId);
                entity.Property(c => c.CustomerId).HasConversion(customerId => customerId.Value,
                    value => new Core.ValueObjects.CustomerId(value));
                entity.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(c => c.LastName).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(200);
                entity.Property(c => c.PhoneNumber).HasMaxLength(20);

                // Customer has many Accounts
                entity.HasMany(c => c.Accounts)
                      .WithOne(a => a.Customer)
                      .HasForeignKey(a => a.CustomerId);
            });

            // Account Configuration
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(a => a.AccountId);
                // Configure AccountNumber as owned type (Value Object)

                // ✅ Configure AccountId value converter (Value Object to Guid)
                entity.Property(a => a.AccountId)
                      .HasConversion(
                          accountId => accountId.Value,
                          value => new Core.ValueObjects.AccountId(value));
                entity.OwnsOne(a => a.AccountNumber, an =>
                {
                    an.Property(a => a.Value).HasColumnName("AccountNumber").IsRequired().HasMaxLength(10);
                });

                entity.Property(a => a.RowVersion)
           .IsRowVersion()
           .IsConcurrencyToken();

                //configure Money as owned type (Value Object)
                entity.OwnsOne(a => a.Balance, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("BalanceAmount").HasPrecision(18,2);
                    money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).HasDefaultValue(CurrencyType.NGN);
                    
                });
                //entity.Property(a => a.Transactions).HasConversion<string>().IsRequired();

                ////Acount has many Transactions
                entity.HasMany(a => a.Transactions)
                     .WithOne(t => t.Account)
                      .HasForeignKey(t => t.AccountId);
                      //.Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);

                //// Ensure we don't accidetally load all transactions
                entity.Navigation(a => a.Transactions).AutoInclude(false);

                entity.Property(a => a.AccountType).HasConversion<string>().IsRequired();

                // Configure the backing field for Transactions navigation
                //var transactionsNav = entity.Metadata.FindNavigation(nameof(Account.Transactions));
                //transactionsNav?.SetPropertyAccessMode(PropertyAccessMode.Field);
                //transactionsNav?.SetField("_transactions");

                //// Account has many Transactions - configure relationship
                //entity.HasMany<Transaction>("_transactions")
                //      .WithOne(t => t.Account)
                //      .HasForeignKey(t => t.AccountId);

                //// Don't automatically load all transactions
                //entity.Navigation("_transactions").AutoInclude(false);
            });

            //transaction Configuration
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.TransactionId);
                // ✅ Configure TransactionId value converter (Value Object to Guid)
                entity.Property(t => t.TransactionId)
                      .HasConversion(
                          transactionId => transactionId.Value,
                          value => new Core.ValueObjects.TransactionId(value));
                //configure Money as owned type (Value Object)
                entity.OwnsOne(t => t.Amount, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("Amount").HasPrecision(18, 2);
                    money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3);
                });
                entity.Property(t => t.Type).HasConversion<string>().IsRequired();
                entity.Property(t => t.Timestamp).IsRequired();

                entity.Property(t => t.Description).HasMaxLength(500);
                entity.Property(t => t.Reference).HasMaxLength(50);
                entity.Property(t => t.Timestamp).IsRequired();
            });
            // Global query filter in DbContext - Automatically Exclude Deleted Records
            modelBuilder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Account>().HasQueryFilter(a => !a.IsDeleted);

           
            
            
            
            
            
            
            
            
            
            
            
            // Seed the DB
            modelBuilder.Entity<Customer>().HasData(new
            {
                CustomerId = new CustomerId (Guid.Parse("a1b2c3d4-1234-5678-9abc-123456789abc")),
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@email.com",
                PhoneNumber = "555-0101",
                DateCreated = DateTime.UtcNow.AddDays(-30),
                IsActive = true,
                IsDeleted = false
            }
            );
            var accountId = new Core.ValueObjects.AccountId(Guid.Parse("c3d4e5f6-3456-7890-cde1-345678901cde"));
            modelBuilder.Entity<Account>().HasData(new
            {
                AccountId = accountId,
                //AccountNumber = "1000000001", // maps to AccountNumber.Value
                AccountType = AccountType.Savings, // EF handles enum conversion
                CustomerId = new CustomerId (Guid.Parse("a1b2c3d4-1234-5678-9abc-123456789abc")),
                DateOpened = DateTime.UtcNow.AddDays(-20),
                IsActive = true,
                IsDeleted = false
            }
            );

            // 2. Seed owned types separately
            modelBuilder.Entity<Account>().OwnsOne(a => a.AccountNumber).HasData(new
            {
                AccountId = accountId, // Foreign key reference
                Value = "1000000001"
            });

            modelBuilder.Entity<Account>().OwnsOne(a => a.Balance).HasData(new
            {
                AccountId = accountId, // Foreign key reference
                Amount = 1500.00m,
                Currency = CurrencyType.NGN
            });
        }
    }
}
