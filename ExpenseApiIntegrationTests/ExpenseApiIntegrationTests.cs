using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ExpenseApi.DataTransferObjects;
using ExpenseApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace ExpenseApiIntegrationTests
{
    public class ExpenseApiIntegrationTests : IClassFixture<CustomWebApplicationFactory<ExpenseApi.Startup>>
    {
        private readonly HttpClient _client;

        private readonly CustomWebApplicationFactory<ExpenseApi.Startup> _factory;

        public ExpenseApiIntegrationTests(CustomWebApplicationFactory<ExpenseApi.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task ListTest()
        {
            var response = await _client.GetAsync("/api/v1/expenses");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var expenses = JsonConvert.DeserializeObject<List<ExpenseDto>>(content);

            Assert.True(expenses.Count() > 0);
        }

        [Fact]
        public async Task DetailTest()
        {
            var response = await _client.GetAsync("/api/v1/expenses/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var expense = JsonConvert.DeserializeObject<ExpenseDto>(content);

            Assert.Equal(DateTime.Today.AddDays(-1), expense.TransactionDate);
            Assert.Equal(ExpenseType.Food, expense.Type);
            Assert.Equal(300, expense.Amount);
            Assert.Equal("CHF", expense.Currency);
            Assert.Equal("Alice", expense.Recipient);
        }

        [Fact]
        public async Task CreateTest()
        {
            var expenseForCreation = new ExpenseForCreationDto
            {
                TransactionDate = DateTime.Today,
                Type = ExpenseType.Food,
                Amount = 42.3,
                Currency = "LGB",
                Recipient = "Dan"
            };

            var content = new StringContent(JsonConvert.SerializeObject(expenseForCreation), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/v1/expenses", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var createdContent = await response.Content.ReadAsStringAsync();
            var expense = JsonConvert.DeserializeObject<ExpenseDto>(createdContent);

            Assert.Equal(DateTime.Today, expense.TransactionDate);
            Assert.Equal(ExpenseType.Food, expense.Type);
            Assert.Equal(42.3, expense.Amount);
            Assert.Equal("LGB", expense.Currency);
            Assert.Equal("Dan", expense.Recipient);

            var createdResponse = await _client.GetAsync($"/api/v1/expenses/{expense.Id}");
            Assert.Equal(HttpStatusCode.OK, createdResponse.StatusCode);

            var newContent = await createdResponse.Content.ReadAsStringAsync();
            var newExpense = JsonConvert.DeserializeObject<ExpenseDto>(newContent);

            Assert.Equal(DateTime.Today, newExpense.TransactionDate);
            Assert.Equal(ExpenseType.Food, newExpense.Type);
            Assert.Equal(42.3, newExpense.Amount);
            Assert.Equal("LGB", newExpense.Currency);
            Assert.Equal("Dan", newExpense.Recipient);
        }

        [Fact]
        public async Task UpdateTest()
        {
            var expenseForUpdate = new ExpenseForUpdateDto
            {
                TransactionDate = DateTime.Today.AddDays(1),
                Type = ExpenseType.Food,
                Amount = 42.3,
                Currency = "BRL",
                Recipient = "Eve"
            };

            var content = new StringContent(JsonConvert.SerializeObject(expenseForUpdate), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/v1/expenses/2", content);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var updatedResponse = await _client.GetAsync("/api/v1/expenses/2");
            Assert.Equal(HttpStatusCode.OK, updatedResponse.StatusCode);

            var updatedContent = await updatedResponse.Content.ReadAsStringAsync();
            var expense = JsonConvert.DeserializeObject<ExpenseDto>(updatedContent);

            Assert.Equal(DateTime.Today.AddDays(1), expense.TransactionDate);
            Assert.Equal(ExpenseType.Food, expense.Type);
            Assert.Equal(42.3, expense.Amount);
            Assert.Equal("BRL", expense.Currency);
            Assert.Equal("Eve", expense.Recipient);
        }

        [Fact]
        public async Task DeleteTest()
        {
            var response = await _client.DeleteAsync("/api/v1/expenses/3");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var deletedContent = await response.Content.ReadAsStringAsync();
            var expense = JsonConvert.DeserializeObject<ExpenseDto>(deletedContent);

            Assert.Equal(DateTime.Today, expense.TransactionDate);
            Assert.Equal(ExpenseType.Other, expense.Type);
            Assert.Equal(100, expense.Amount);
            Assert.Equal("USD", expense.Currency);
            Assert.Equal("Carol", expense.Recipient);

            var notFoundResponse = await _client.GetAsync("/api/v1/expenses/3");
            Assert.Equal(HttpStatusCode.NotFound, notFoundResponse.StatusCode);
        }
    }
}
