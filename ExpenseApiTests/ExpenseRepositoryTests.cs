using System;
using System.Threading.Tasks;
using ExpenseApi.Models;
using ExpenseApi.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ExpenseApiTests
{
    public class ExpenseRepositoryTests
    {
        private ExpenseContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ExpenseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ExpenseContext(options);

            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task GetExpensesAsyncTest()
        {
            var repository = new ExpenseRepository(GetContext());

            var expenses = await repository.GetExpensesAsync();
            Assert.Equal(3, expenses.Count);
        }

        [Fact]
        public async Task GetExpenseByIdAsyncTest()
        {
            var repository = new ExpenseRepository(GetContext());

            var expense = await repository.GetExpenseByIdAsync(1);
            Assert.Equal("Stefan", expense.Recipient);
        }

        [Fact]
        public async Task InsertAndGetExpenseAsyncTest()
        {
            var repository = new ExpenseRepository(GetContext());

            var expense = new Expense
            {
                Id = 0,
                TransactionDate = DateTime.Today,
                Type = ExpenseType.Drink,
                Recipient = "test recipient",
                Amount = 123,
                Currency = "CHF"
            };
            await repository.InsertExpenseAsync(expense);

            Assert.NotEqual(0, expense.Id);

            var expenseFromDb = await repository.GetExpenseByIdAsync(expense.Id);

            Assert.Equal("test recipient", expenseFromDb.Recipient);
        }

        [Fact]
        public async Task UpdateAndGetExpenseAsyncTest()
        {
            var repository = new ExpenseRepository(GetContext());

            var expense = await repository.GetExpenseByIdAsync(1);

            Assert.Equal("Stefan", expense.Recipient);

            expense.Recipient = "Cezary";
            await repository.UpdateExpenseAsync(expense);

            var newExpense = await repository.GetExpenseByIdAsync(1);

            Assert.Equal("Cezary", newExpense.Recipient);
        }

        [Fact]
        public async Task DeleteAndExistsExpenseAsyncTest()
        {
            var repository = new ExpenseRepository(GetContext());

            Assert.True(repository.ExpenseExists(1));

            var expense = await repository.GetExpenseByIdAsync(1);
            await repository.DeleteExpenseAsync(expense);

            Assert.False(repository.ExpenseExists(1));
        }
    }
}
