using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseApi.Domain.Models;

namespace ExpenseApi.Domain.Services
{
    public interface IExpenseService
    {
        Task<List<Expense>> ListAsync();
        Task<Expense> DetailAsync(long expenseId);
        Task CreateAsync(Expense expense);
        Task UpdateAsync(Expense expense);
        Task DeleteAsync(Expense expense);
        bool Exists(long expenseId);
    }
}
