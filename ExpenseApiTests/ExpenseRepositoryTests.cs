using System;
using System.Linq;
using System.Threading.Tasks;
using ExpenseApi.Models;
using ExpenseApi.Contexts;
using ExpenseApi.Repositories;
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
        public async Task ListAsyncTest()
        {
            var repository = new ExpenseRepository(GetContext());

            var expenses = await repository.ListAsync();
            Assert.Equal(3, expenses.Count());
        }

        [Fact]
        public async Task DetailAsyncTest()
        {
            var repository = new ExpenseRepository(GetContext());

            var expense = await repository.DetailAsync(1);
            Assert.Equal("Stefan", expense.Recipient);
        }

        [Fact]
        public async Task InsertAndDetailAsyncTest()
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
            await repository.CreateAsync(expense);

            Assert.NotEqual(0, expense.Id);

            var expenseFromDb = await repository.DetailAsync(expense.Id);

            Assert.Equal("test recipient", expenseFromDb.Recipient);
        }

        [Fact]
        public async Task UpdateAndDetailAsyncTest()
        {
            var repository = new ExpenseRepository(GetContext());

            var expense = await repository.DetailAsync(1);

            Assert.Equal("Stefan", expense.Recipient);

            expense.Recipient = "Cezary";
            await repository.UpdateAsync(expense);

            var newExpense = await repository.DetailAsync(1);

            Assert.Equal("Cezary", newExpense.Recipient);
        }

        [Fact]
        public async Task DeleteAndDetailAsyncTest()
        {
            var repository = new ExpenseRepository(GetContext());

            var expense = await repository.DetailAsync(1);
            Assert.NotNull(expense);

            await repository.DeleteAsync(expense);
            Assert.Null(await repository.DetailAsync(1));
        }
    }
}
