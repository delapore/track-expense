using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseApi.Models;
using System;
using ExpenseApi.Repository;

namespace ExpenseApi.Controllers
{
    [ApiController]
    [Route("api/v1/expenses")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseRepository _repository;

        public ExpensesController(IExpenseRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/v1/expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses() => await _repository.GetExpensesAsync();

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(long id)
        {
            var expense = await _repository.GetExpenseByIdAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }

        // PUT: api/v1/expenses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(long id, Expense expense)
        {
            if (id != expense.Id)
            {
                return ValidationProblem("Body id field is not the same as parameter id");
            }

            try
            {
                await _repository.UpdateExpenseAsync(expense);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/v1/expenses
        [HttpPost]
        public async Task<ActionResult<Expense>> PostExpense(Expense expense)
        {
            await _repository.InsertExpenseAsync(expense);
            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
        }

        // DELETE: api/v1/expenses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Expense>> DeleteExpense(long id)
        {
            var expense = await _repository.GetExpenseByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            await _repository.DeleteExpenseAsync(expense);

            return expense;
        }
    }
}
