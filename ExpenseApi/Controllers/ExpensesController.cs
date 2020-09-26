using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseApi.Models;
using System;
using ExpenseApi.Repositories;
using ExpenseApi.Logger;
using System.Text.Json;

namespace ExpenseApi.Controllers
{
    [ApiController]
    [Route("api/v1/expenses")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ExpensesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IExpenseRepository _repository;

        public ExpensesController(IExpenseRepository repository, ILoggerManager logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/v1/expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {
            try
            {
                var expenses = await _repository.ListAsync();

                _logger.LogInfo($"{nameof(GetExpenses)} : return Ok from database.");
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetExpenses)} : something went wrong{Environment.NewLine}" +
                    $"\t{ex.Message}");
                throw;
            }
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(long id)
        {
            try
            {
                var expense = await _repository.DetailAsync(id);

                if (expense == null)
                {
                    _logger.LogInfo($"{nameof(GetExpense)}({id}) : return NotFound from database");
                    return NotFound();
                }

                _logger.LogInfo($"{nameof(GetExpense)}({id}) : return Ok from database");
                return Ok(expense);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(GetExpense)}({id}) : something went wrong{Environment.NewLine}" +
                    $"\t{ex.Message}");
                throw;
            }
        }

        // PUT: api/v1/expenses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(long id, Expense expense)
        {
            try
            {
                if (id != expense.Id)
                {
                    var message = "Body id field is not the same as parameter id";

                    _logger.LogInfo($"{nameof(PutExpense)}({id}) : return ValidationProblem(\"{message}\") from datanase");
                    return ValidationProblem(message);
                }

                try
                {
                    await _repository.UpdateAsync(expense);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.Exists(id))
                    {
                        _logger.LogInfo($"{nameof(PutExpense)}({id}) : return NotFound from database");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                _logger.LogInfo($"{nameof(PutExpense)}({id}) : return NoContent from database");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(PutExpense)}({id}) : something went wrong with {JsonSerializer.Serialize(expense)}{Environment.NewLine}" +
                    $"\t{ex.Message}");
                throw;
            }
        }

        // POST: api/v1/expenses
        [HttpPost]
        public async Task<ActionResult<Expense>> PostExpense(Expense expense)
        {
            try
            {
                await _repository.CreateAsync(expense);

                _logger.LogInfo($"{nameof(PostExpense)} : return CreatedAtAction({nameof(GetExpense)}, {expense.Id}) from database");

                return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(PostExpense)} : something went wrong with {JsonSerializer.Serialize(expense)}{Environment.NewLine}" +
                    $"\t{ex.Message}");
                throw;
            }
        }

        // DELETE: api/v1/expenses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Expense>> DeleteExpense(long id)
        {
            try
            {
                var expense = await _repository.DetailAsync(id);
                if (expense == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(expense);

                return Ok(expense);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DeleteExpense)}({id}) : something went wrong{Environment.NewLine}" +
                    $"\t{ex.Message}");
                throw;
            }
        }
    }
}
