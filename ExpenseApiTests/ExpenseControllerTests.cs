using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseApi.Controllers;
using ExpenseApi.DataTransferObjects;
using ExpenseApi.Logger;
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
        private readonly ILoggerManager _loggerManager;
        private readonly IMapper _mapper;
        public ExpenseControllerTests()
        {
            _loggerManager = new Mock<ILoggerManager>().Object;

            var mockMapper = new Mock<IMapper>(MockBehavior.Strict);
            mockMapper.Setup(mapper => mapper.Map<IEnumerable<ExpenseDto>>(It.IsAny<IEnumerable<Expense>>()))
                .Returns(GetTestExpensesDto());
            mockMapper.Setup(mapper => mapper.Map<ExpenseDto>(It.IsAny<Expense>()))
                .Returns(GetTestExpenseDto());
            mockMapper.Setup(mapper => mapper.Map(It.IsAny<ExpenseForUpdateDto>(), It.IsAny<Expense>()))
                .Returns(GetTestExpense());
            mockMapper.Setup(mapper => mapper.Map<Expense>(It.IsAny<ExpenseForCreationDto>()))
                .Returns(GetTestExpense());

            _mapper = mockMapper.Object;
        }

        [Fact]
        public async Task GetExpensesTest()
        {
            var mockRepo = new Mock<IExpenseRepository>(MockBehavior.Strict);
            mockRepo.Setup(repo => repo.ListAsync())
                .ReturnsAsync(GetTestExpenses());

            var controller = new ExpensesController(mockRepo.Object, _loggerManager, _mapper);

            var result = await controller.GetExpenses();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<ExpenseDto>>(okResult.Value);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task GetExpenseFromIdTest()
        {
            var mockRepo = new Mock<IExpenseRepository>(MockBehavior.Strict);
            mockRepo.Setup(repo => repo.DetailAsync(It.IsAny<long>()))
                .ReturnsAsync(GetTestExpense());

            var controller = new ExpensesController(mockRepo.Object, _loggerManager, _mapper);

            var result = await controller.GetExpense(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<ExpenseDto>(okResult.Value);
            Assert.Equal(3, model.Id);
        }

        [Fact]
        public async Task GetExpenseFromNotFoundIdTest()
        {
            var mockRepo = new Mock<IExpenseRepository>(MockBehavior.Strict);
            mockRepo.Setup(repo => repo.DetailAsync(It.IsAny<long>()))
                .ReturnsAsync((Expense)null);

            var controller = new ExpensesController(mockRepo.Object, _loggerManager, _mapper);

            var result = await controller.GetExpense(1);

            Assert.IsType<NotFoundResult>(result.Result);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task PostExpenseTest()
        {
            var mockRepo = new Mock<IExpenseRepository>(MockBehavior.Strict);
            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Expense>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var controller = new ExpensesController(mockRepo.Object, _loggerManager, _mapper);

            var result = await controller.PostExpense(GetTestExpenseForCreationDto());

            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Null(actionResult.ControllerName);
            Assert.Equal("GetExpense", actionResult.ActionName);
            mockRepo.Verify();
        }

        [Fact]
        public async Task PutExpenseTest()
        {
            var mockRepo = new Mock<IExpenseRepository>(MockBehavior.Strict);
            mockRepo.Setup(repo => repo.DetailAsync(It.IsAny<long>()))
                .ReturnsAsync(GetTestExpense());
            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Expense>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var controller = new ExpensesController(mockRepo.Object, _loggerManager, _mapper);

            var expense = GetTestExpense();
            var result = await controller.PutExpense(3, GetTestExpenseForUpdateDto());

            var actionResult = Assert.IsType<NoContentResult>(result);
            mockRepo.Verify();
        }

        [Fact]
        public async Task PutExpenseNotFoundIdTest()
        {
            var mockRepo = new Mock<IExpenseRepository>(MockBehavior.Strict);
            mockRepo.Setup(repo => repo.DetailAsync(It.IsAny<long>()))
                .ReturnsAsync((Expense)null);

            var controller = new ExpensesController(mockRepo.Object, _loggerManager, _mapper);

            var result = await controller.PutExpense(3, GetTestExpenseForUpdateDto());

            var actionResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteExpenseTest()
        {
            var mockRepo = new Mock<IExpenseRepository>(MockBehavior.Strict);
            mockRepo.Setup(repo => repo.DeleteAsync(It.IsAny<Expense>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
            mockRepo.Setup(repo => repo.DetailAsync(It.IsAny<long>()))
                .ReturnsAsync(GetTestExpense());

            var controller = new ExpensesController(mockRepo.Object, _loggerManager, _mapper);

            var result = await controller.DeleteExpense(3);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<ExpenseDto>(okResult.Value);
            Assert.Equal(3, model.Id);
            mockRepo.Verify();
        }

        [Fact]
        public async Task DeleteExpenseNotFoundIdTest()
        {
            var mockRepo = new Mock<IExpenseRepository>(MockBehavior.Strict);
            mockRepo.Setup(repo => repo.DetailAsync(It.IsAny<long>()))
                .ReturnsAsync((Expense)null);

            var controller = new ExpensesController(mockRepo.Object, _loggerManager, _mapper);

            var result = await controller.DeleteExpense(3);

            Assert.IsType<NotFoundResult>(result.Result);
            Assert.Null(result.Value);
        }

        private IEnumerable<Expense> GetTestExpenses()
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

        private IEnumerable<ExpenseDto> GetTestExpensesDto()
            => GetTestExpenses().Select(e => new ExpenseDto
            {
                Id = e.Id,
                TransactionDate = e.TransactionDate,
                Type = e.Type,
                Amount = e.Amount,
                Currency = e.Currency,
                Recipient = e.Recipient
            });

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

        private ExpenseDto GetTestExpenseDto()
        {
            var expense = GetTestExpense();
            return new ExpenseDto
            {
                Id = expense.Id,
                TransactionDate = expense.TransactionDate,
                Type = expense.Type,
                Amount = expense.Amount,
                Currency = expense.Currency,
                Recipient = expense.Recipient
            };
        }

        private ExpenseForUpdateDto GetTestExpenseForUpdateDto()
        {
            var expense = GetTestExpense();
            return new ExpenseForUpdateDto
            {
                TransactionDate = expense.TransactionDate,
                Type = expense.Type,
                Amount = expense.Amount,
                Currency = expense.Currency,
                Recipient = expense.Recipient
            };
        }

        private ExpenseForCreationDto GetTestExpenseForCreationDto()
        {
            var expense = GetTestExpense();
            return new ExpenseForCreationDto
            {
                TransactionDate = expense.TransactionDate,
                Type = expense.Type,
                Amount = expense.Amount,
                Currency = expense.Currency,
                Recipient = expense.Recipient
            };
        }
    }
}
