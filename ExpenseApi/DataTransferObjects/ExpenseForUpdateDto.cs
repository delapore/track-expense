using System;
using System.ComponentModel.DataAnnotations;
using ExpenseApi.Models;

namespace ExpenseApi.DataTransferObjects
{
    public class ExpenseForUpdateDto
    {
        public DateTime TransactionDate { get; set; }

        public ExpenseType Type { get; set; }

        public double Amount { get; set; }

        [Required]
        [MaxLength(3)]
        public string Currency { get; set; }

        public string Recipient { get; set; }
    }
}
