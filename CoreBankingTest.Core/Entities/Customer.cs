using CoreBankingTest.Core.Interfaces;
using CoreBankingTest.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBankingTest.Core.Entities
{
    public class Customer: ISoftDelete
    {
        public CustomerId CustomerId { get; private set; }
        
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateTime DateCreated { get; private set; } 
        private bool IsActive { get; set; }

        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public string? DeletedBy { get; private set; }

        public void SoftDelete(string deletedBy)
        {
            if (Accounts.Any(a => a.Balance.Amount > 0))
                throw new InvalidOperationException("Cannot delete customer with account balance");

            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }

        //Navigation Property for Accounts
        private readonly List<Account> _accounts = new();
        public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();

        public Customer (string firstName, string lastName, string email, string phoneNumber)
        {
            CustomerId = CustomerId.Create();
            FirstName = firstName ?? throw new ArgumentException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentException(nameof(lastName));
            Email = email ?? throw new ArgumentException(nameof(email));
            PhoneNumber = phoneNumber ?? throw new ArgumentException(nameof(phoneNumber));
            DateCreated = DateTime.UtcNow.AddDays(-30);
            IsActive = true;
        }

        public void UpdateContactInfo(string email, string phoneNumber)
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot update Inactive customer.");

            Email = email;
            PhoneNumber = phoneNumber;
        }

        public void Deactivate()
        {
            if(_accounts.Any(a => a.Balance.Amount > 0))
                throw new InvalidOperationException("Cannot deactivate customer with active accounts having balance.");
            IsActive = false;
        }

        internal void AddAccount(Account account)
        {
            _accounts.Add(account);
        }
    }
}
