using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseApi.Models;
using System;
using ExpenseApi.Repositories;
using ExpenseApi.Logger;
using System.Text.Json;
using AutoMapper;
using ExpenseApi.DataTransferObjects;

namespace ExpenseApi.Controllers
{
    [ApiController]
    [Route("api/v1/expenses")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ExpensesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IExpenseRepository _repository;
        private readonly IMapper _mapper;

        public ExpensesController(IExpenseRepository repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/v1/expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpenses()
        {
            try
            {
                var expenses = await _repository.ListAsync();
                var expensesDto = _mapper.Map<IEnumerable<ExpenseDto>>(expenses);

                _logger.LogInfo($"{nameof(GetExpenses)} : return Ok from database.");
                return Ok(expensesDto);
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
        public async Task<ActionResult<ExpenseDto>> GetExpense(long id)
        {
            try
            {
                var expense = await _repository.DetailAsync(id);

                if (expense == null)
                {
                    _logger.LogInfo($"{nameof(GetExpense)}({id}) : return NotFound from database");
                    return NotFound();
                }

                var expenseDto = _mapper.Map<ExpenseDto>(expense);

                _logger.LogInfo($"{nameof(GetExpense)}({id}) : return Ok from database");
                return Ok(expenseDto);
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
        public async Task<IActionResult> PutExpense(long id, ExpenseForUpdateDto expenseForUpdateDto)
        {
            try
            {
                var expense = await _repository.DetailAsync(id);
                if (expense == null)
                {
                    _logger.LogInfo($"{nameof(PutExpense)}({id}) : return NotFound from database");
                    return NotFound();
                }

                expense = _mapper.Map(expenseForUpdateDto, expense);
                await _repository.UpdateAsync(expense);

                _logger.LogInfo($"{nameof(PutExpense)}({id}) : return NoContent from database");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(PutExpense)}({id}) : something went wrong with {JsonSerializer.Serialize(expenseForUpdateDto)}" +
                    $"{Environment.NewLine}\t{ex.Message}");
                throw;
            }
        }

        // POST: api/v1/expenses
        [HttpPost]
        public async Task<ActionResult<ExpenseDto>> PostExpense(ExpenseForCreationDto expenseForCreationDto)
        {
            try
            {
                var expense = _mapper.Map<Expense>(expenseForCreationDto);
                await _repository.CreateAsync(expense);

                var expenseDto = _mapper.Map<ExpenseDto>(expense);

                _logger.LogInfo($"{nameof(PostExpense)} : return CreatedAtAction({nameof(GetExpense)}, {expenseDto.Id}) from database");
                return CreatedAtAction(nameof(GetExpense), new { id = expenseDto.Id }, expenseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(PostExpense)} : something went wrong with {JsonSerializer.Serialize(expenseForCreationDto)}" +
                    $"{Environment.NewLine}\t{ex.Message}");
                throw;
            }
        }

        // DELETE: api/v1/expenses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ExpenseDto>> DeleteExpense(long id)
        {
            try
            {
                var expense = await _repository.DetailAsync(id);
                if (expense == null)
                {
                    _logger.LogInfo($"{nameof(DeleteExpense)}({id}) : return NotFound from database");
                    return NotFound();
                }

                await _repository.DeleteAsync(expense);
                var expenseDto = _mapper.Map<ExpenseDto>(expense);

                _logger.LogInfo($"{nameof(DeleteExpense)}({id}) : return Ok from database");
                return Ok(expenseDto);
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
