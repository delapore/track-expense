using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseApi.Models;

namespace ExpenseApi.Repository
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<Expense>> GetExpensesAsync();
        Task<Expense> GetExpenseByIdAsync(long expenseId);
        Task InsertExpenseAsync(Expense expense);
        Task UpdateExpenseAsync(Expense expense);
        Task DeleteExpenseAsync(Expense expense);
        bool ExpenseExists(long expenseId);
    }
}
