using System.Collections.Generic;

namespace Akkounts.Publisher.Model
{
    public class TransactionsFactory
    {
        //This is a fake transactions generator
        public IEnumerable<RandomTransaction> GetTransactions()
        {
            while (true)
                yield return new RandomTransaction();
        }
    }
}