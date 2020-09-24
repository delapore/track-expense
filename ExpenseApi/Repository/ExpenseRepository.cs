using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseApi.Repository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseContext _dbContext;

        public ExpenseRepository(ExpenseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteExpenseAsync(Expense expense)
        {
            _dbContext.Expenses.Remove(expense);
            await _dbContext.SaveChangesAsync();
        }

        public bool ExpenseExists(long expenseId)
        {
            return _dbContext.Expenses.Any(e => e.Id == expenseId);
        }

        public async Task<Expense> GetExpenseByIdAsync(long expenseId)
        {
            return await _dbContext.Expenses.FindAsync(expenseId);
        }

        public async Task<List<Expense>> GetExpensesAsync()
        {
            return await _dbContext.Expenses.ToListAsync();
        }

        public async Task InsertExpenseAsync(Expense expense)
        {
            _dbContext.Expenses.Add(expense);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            _dbContext.Entry(expense).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
