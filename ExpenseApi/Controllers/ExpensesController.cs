using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseApi.Models;
using System;
using ExpenseApi.Repository;
using Microsoft.AspNetCore.Http;

namespace ExpenseApi.Controllers
{
    [Route("api/v1/expenses")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseRepository _repository;

        public ExpensesController(IExpenseRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/v1/expenses
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses() => await _repository.GetExpensesAsync();

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Expense>> PostExpense(Expense expense)
        {
            await _repository.InsertExpenseAsync(expense);
            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
        }

        // DELETE: api/v1/expenses/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
