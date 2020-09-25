using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseApi.Domain.Models
{
    public class Expense
    {
        public long Id { get; set; }

        public DateTime TransactionDate { get; set; }

        public ExpenseType Type { get; set; }

        public double Amount { get; set; }

        [Required]
        [MaxLength(3)]
        public string Currency { get; set; }

        public string Recipient { get; set; }
    }
}
