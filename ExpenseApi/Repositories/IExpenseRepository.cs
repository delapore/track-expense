using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseApi.Models;

namespace ExpenseApi.Repositories
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<Expense>> ListAsync();
        Task<Expense> DetailAsync(long expenseId);
        Task CreateAsync(Expense expense);
        Task UpdateAsync(Expense expense);
        Task DeleteAsync(Expense expense);
    }
}
