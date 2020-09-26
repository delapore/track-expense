using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseApi.Domain.Models;
using ExpenseApi.Domain.Repositories;
using ExpenseApi.Domain.Services;

namespace ExpenseApi.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repository;

        public ExpenseService(IExpenseRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(Expense expense) => await _repository.CreateAsync(expense);
        public async Task DeleteAsync(Expense expense) => await _repository.DeleteAsync(expense);
        public async Task<Expense> DetailAsync(long expenseId) => await _repository.DetailAsync(expenseId);
        public bool Exists(long expenseId) => _repository.Exists(expenseId);
        public async Task<IEnumerable<Expense>> ListAsync() => await _repository.ListAsync();
        public async Task UpdateAsync(Expense expense) => await _repository.UpdateAsync(expense);
    }
}
