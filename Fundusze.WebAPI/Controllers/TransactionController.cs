using Microsoft.AspNetCore.Mvc;
using Fundusze.Domain.Interfaces;
using Fundusze.Application.DTOs;
using Fundusze.Application.Mappers;
using Fundusze.Application.Services;
using Fundusze.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundusze.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionService _transactionService;

        public TransactionController(IUnitOfWork unitOfWork, ITransactionService transactionService)
        {
            _unitOfWork = unitOfWork;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll()
        {
            var transactions = await _unitOfWork.Transactions.GetAllAsync();
            return Ok(transactions.Select(TransactionMapper.ToDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> Get(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            if (transaction == null) return NotFound();

            return Ok(TransactionMapper.ToDto(transaction));
        }


        [HttpPost]
        public async Task<ActionResult<TransactionDto>> CreateTransaction([FromBody] CreateTransactionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var createdTransaction = await _transactionService.AddTransactionAndUpdatePortfolioAsync(dto);

                var fullTransaction = await _unitOfWork.Transactions.GetByIdAsync(createdTransaction.Id);
                var resultDto = TransactionMapper.ToDto(fullTransaction);

                return CreatedAtAction(nameof(Get), new { id = createdTransaction.Id }, resultDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTransaction(int id, [FromBody] TransactionDto dto)
        {
            if (id != dto.Id) return BadRequest("ID w URL nie zgadza się z ID w ciele żądania.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _transactionService.UpdateTransactionAndUpdatePortfolioAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            try
            {
                await _transactionService.DeleteTransactionAndUpdatePortfolioAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}