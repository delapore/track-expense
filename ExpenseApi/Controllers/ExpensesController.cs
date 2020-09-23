using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseApi.Models;
using System;
using System.Net;
using ExpenseApi.Repository;

namespace ExpenseApi.Controllers
{
    [Route("api/v1/expenses")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseRepository _repository;

        public ExpensesController(IExpenseRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));;
        }

        // GET: api/v1/expenses
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Expense>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {
            var items = await _repository.GetExpensesAsync();
            return Ok(items);
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Expense), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Expense>> GetExpense(long id)
        {
            var expense = await _repository.GetExpenseByIdAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        // PUT: api/v1/expenses/5
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> PutExpense(long id, Expense expense)
        {
            if (id != expense.Id)
            {
                return BadRequest();
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
        [ProducesResponseType(typeof(Expense), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Expense>> PostExpense(Expense expense)
        {
            await _repository.InsertExpenseAsync(expense);
            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
        }

        // DELETE: api/v1/expenses/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Expense), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Expense>> DeleteExpense(long id)
        {
            var expense = await _repository.GetExpenseByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            await _repository.DeleteExpenseAsync(expense);

            return Ok(expense);
        }
    }
}
