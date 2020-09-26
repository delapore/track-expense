using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseApi.Controllers;
using ExpenseApi.Models;
using ExpenseApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ExpenseApiTests
{
    public class ExpenseControllerTests
    {
        [Fact]
        public async Task GetExpensesTest()
        {
            var mockRepo = new Mock<IExpenseRepository>();
            mockRepo.Setup(repo => repo.ListAsync())
                .ReturnsAsync(GetTestExpenses());

            var controller = new ExpensesController(mockRepo.Object);

            var result = await controller.GetExpenses();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<Expense>>(okResult.Value);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task GetExpenseFromIdTest()
        {
            var mockRepo = new Mock<IExpenseRepository>();
            mockRepo.Setup(repo => repo.DetailAsync(It.IsAny<long>()))
                .ReturnsAsync(GetTestExpense());

            var controller = new ExpensesController(mockRepo.Object);

            var result = await controller.GetExpense(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<Expense>(okResult.Value);
            Assert.Equal(3, model.Id);
        }

        [Fact]
        public async Task GetExpenseFromNotFoundIdTest()
        {
            var mockRepo = new Mock<IExpenseRepository>();
            mockRepo.Setup(repo => repo.DetailAsync(It.IsAny<long>()))
                .ReturnsAsync((Expense)null);

            var controller = new ExpensesController(mockRepo.Object);

            var result = await controller.GetExpense(1);

            Assert.IsType<NotFoundResult>(result.Result);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task PostExpenseTest()
        {
            var mockRepo = new Mock<IExpenseRepository>();
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Expense>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var controller = new ExpensesController(mockRepo.Object);

            var result = await controller.PostExpense(GetTestExpense());

            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Null(actionResult.ControllerName);
            Assert.Equal("GetExpense", actionResult.ActionName);
            mockRepo.Verify();
        }

        [Fact]
        public async Task PutExpenseTest()
        {
            var mockRepo = new Mock<IExpenseRepository>();
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Expense>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var controller = new ExpensesController(mockRepo.Object);

            var result = await controller.PutExpense(3, GetTestExpense());

            var actionResult = Assert.IsType<NoContentResult>(result);
            mockRepo.Verify();
        }

        [Fact]
        public async Task PutExpenseWrongIdFromBodyTest()
        {
            var mockRepo = new Mock<IExpenseRepository>();

            var controller = new ExpensesController(mockRepo.Object);

            // ValidationProblem is returned but because ModelState is null, it throws a null ref exception
            var ex = await Assert.ThrowsAsync<NullReferenceException>(() => controller.PutExpense(1, GetTestExpense()));
            Assert.Contains("at Microsoft.AspNetCore.Mvc.ControllerBase.ValidationProblem", ex.StackTrace);
        }

        [Fact]
        public async Task PutExpenseNotFoundIdTest()
        {
            var mockRepo = new Mock<IExpenseRepository>();
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Expense>()))
                .Throws(new DbUpdateConcurrencyException());
            mockRepo.Setup(repo => repo.Exists(It.IsAny<long>()))
                .Returns(false);

            var controller = new ExpensesController(mockRepo.Object);

            var result = await controller.PutExpense(3, GetTestExpense());

            var actionResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteExpenseTest()
        {
            var mockRepo = new Mock<IExpenseRepository>();
            mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<Expense>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            mockRepo.Setup(repo => repo.DetailAsync(It.IsAny<long>()))
                .ReturnsAsync(GetTestExpense());

            var controller = new ExpensesController(mockRepo.Object);

            var result = await controller.DeleteExpense(3);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<Expense>(okResult.Value);
            Assert.Equal(3, model.Id);
            mockRepo.Verify();
        }

        [Fact]
        public async Task DeleteExpenseNotFoundIdTest()
        {
            var mockRepo = new Mock<IExpenseRepository>();
            mockRepo.Setup(repo => repo.DetailAsync(It.IsAny<long>()))
                .ReturnsAsync((Expense)null);

            var controller = new ExpensesController(mockRepo.Object);

            var result = await controller.DeleteExpense(3);

            Assert.IsType<NotFoundResult>(result.Result);
            Assert.Null(result.Value);
        }

        private List<Expense> GetTestExpenses()
        {
            var expenses = new List<Expense>();

            expenses.Add(new Expense
            {
                Id = 1,
                TransactionDate = new DateTime(2020, 9, 24),
                Type = ExpenseType.Food,
                Amount = 123.45,
                Currency = "EUR",
                Recipient = "Cezary"
            });

            expenses.Add(new Expense
            {
                Id = 2,
                TransactionDate = new DateTime(2020, 9, 23),
                Type = ExpenseType.Other,
                Amount = 100,
                Currency = "CHF",
                Recipient = "Manuel"
            });

            return expenses;
        }

        private Expense GetTestExpense()
        {
            return new Expense
            {
                Id = 3,
                TransactionDate = new DateTime(2020, 9, 24),
                Type = ExpenseType.Other,
                Amount = 123.45,
                Currency = "USD",
                Recipient = "Etienne"
            };
        }
    }
}
