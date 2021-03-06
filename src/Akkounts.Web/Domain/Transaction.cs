namespace Akkounts.Web.Domain
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string AccountNumber { get; set; }

        public enum TransactionType
        {
            //Type of transaction
            Debit,
            Credit
        }
    }
}