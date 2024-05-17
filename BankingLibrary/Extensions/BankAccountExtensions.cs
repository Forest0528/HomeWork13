namespace BankingLibrary.Extensions
{
    public static class BankAccountExtensions
    {
        public static void Deposit(this BankAccount account, decimal amount)
        {
            account.Deposit(amount);
        }
    }
}
