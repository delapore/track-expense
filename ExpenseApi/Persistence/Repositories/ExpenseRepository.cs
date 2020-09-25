using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseApi.Domain.Models;
using ExpenseApi.Domain.Repositories;
using ExpenseApi.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApi.Persistence.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseContext _dbContext;

        public ExpenseRepository(ExpenseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteAsync(Expense expense)
        {
            _dbContext.Expenses.Remove(expense);
            await _dbContext.SaveChangesAsync();
        }

        public bool Exists(long expenseId)
        {
            return _dbContext.Expenses.Any(e => e.Id == expenseId);
        }

        public async Task<Expense> DetailAsync(long expenseId)
        {
            return await _dbContext.Expenses.FindAsync(expenseId);
        }

        public async Task<List<Expense>> ListAsync()
        {
            return await _dbContext.Expenses.ToListAsync();
        }

        public async Task CreateAsync(Expense expense)
        {
            _dbContext.Expenses.Add(expense);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Expense expense)
        {
            _dbContext.Entry(expense).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
