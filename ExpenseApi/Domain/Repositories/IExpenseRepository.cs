using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseApi.Domain.Models;

namespace ExpenseApi.Domain.Repositories
{
    public interface IExpenseRepository
    {
        Task<List<Expense>> ListAsync();
        Task<Expense> DetailAsync(long expenseId);
        Task CreateAsync(Expense expense);
        Task UpdateAsync(Expense expense);
        Task DeleteAsync(Expense expense);
        bool Exists(long expenseId);
    }
}
