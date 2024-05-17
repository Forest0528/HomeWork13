using BankingLibrary.Exceptions;
using System;
using System.Linq;

namespace BankingLibrary
{
    public class BankAccount
    {
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }
        public bool IsActive { get; private set; }

        public BankAccount(string accountNumber)
        {
            if (!IsValidAccountNumber(accountNumber))
            {
                throw new InvalidAccountNumberException("Account number must be exactly 16 digits.");
            }
            AccountNumber = accountNumber;
            Balance = 0;
            IsActive = true;
        }

        public void CloseAccount()
        {
            IsActive = false;
        }

        private bool IsValidAccountNumber(string accountNumber)
        {
            return accountNumber.Length == 16 && accountNumber.All(char.IsDigit);
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive.");
            Balance += amount;
        }

        public static bool operator >(BankAccount a, BankAccount b)
        {
            return a.Balance > b.Balance;
        }

        public static bool operator <(BankAccount a, BankAccount b)
        {
            return a.Balance < b.Balance;
        }
    }
}
