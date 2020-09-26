using System;
using ExpenseApi.Models;

namespace ExpenseApi.DataTransferObjects
{
    public class ExpenseDto
    {
        public long Id { get; set; }

        public DateTime TransactionDate { get; set; }

        public ExpenseType Type { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public string Recipient { get; set; }
    }
}
