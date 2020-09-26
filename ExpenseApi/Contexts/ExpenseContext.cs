using System;
using ExpenseApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApi.Contexts
{
    public class ExpenseContext : DbContext
    {
        public ExpenseContext(DbContextOptions<ExpenseContext> options)
            : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Expense>().HasData(
                new Expense
                {
                    Id = 1,
                    TransactionDate = DateTime.Today.AddDays(-1),
                    Type = ExpenseType.Food,
                    Amount = 200,
                    Currency = "CHF",
                    Recipient = "Stefan"
                },
                new Expense
                {
                    Id = 2,
                    TransactionDate = DateTime.Today,
                    Type = ExpenseType.Drink,
                    Amount = 300,
                    Currency = "EUR",
                    Recipient = "Cezary"
                },
                new Expense
                {
                    Id = 3,
                    TransactionDate = DateTime.Today,
                    Type = ExpenseType.Other,
                    Amount = 100,
                    Currency = "USD",
                    Recipient = "Etienne"
                }
            );
        }
    }
}
