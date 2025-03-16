// kyrs/Models/Transaction.cs
using System;

namespace kyrs.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string OperatorName { get; set; }
    }
}