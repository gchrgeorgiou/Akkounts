using System.Collections.Generic;

namespace Akkounts.Web.Domain.Abstract
{
    public interface TransactionRepository
    {
        void Add(Transaction t);
        //Repo
        IEnumerable<Transaction> GetAllBy(string account);
    }
}