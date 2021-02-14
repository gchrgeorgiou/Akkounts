﻿using System;

namespace Akkounts.Domain
{
    public class Transaction
    {
        //The transaction schema
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string AccountNumber { get; set; }
        public DateTime StartDate { get; set; }

        public enum TransactionType
        {
            Debit,
            Credit
        }
    }
}