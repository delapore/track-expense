using System;

namespace ExpenseApi.Models
{
    public class Expense
    {
        public long Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Amount { get; set; }
        public string Recipient { get; set; }
        public string Currency { get; set; }
        public ExpenseType Type { get; set; }
    }
}
