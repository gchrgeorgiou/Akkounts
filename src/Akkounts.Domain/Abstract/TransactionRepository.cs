namespace Akkounts.Domain.Abstract
{
    public interface TransactionRepository
    {
        void Add(Transaction t);
        //Balance
        Balance GetBalance(string account);
    }
    
}