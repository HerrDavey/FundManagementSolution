using Microsoft.AspNetCore.Mvc;
using Fundusze.Domain.Interfaces;
using Fundusze.Application.DTOs;
using Fundusze.Application.Mappers;
using Fundusze.Application.Services; // <-- Nowy using
using Fundusze.Domain;

namespace Fundusze.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionService _transactionService; // <-- Nowy serwis

        // Wstrzykujemy ITransactionService zamiast tylko IUnitOfWork
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
        public async Task<ActionResult<TransactionDto>> CreateTransaction([FromBody] TransactionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                // Zamiast logiki w kontrolerze, wywołujemy jedną metodę z serwisu
                var createdTransaction = await _transactionService.AddTransactionAndUpdatePortfolioAsync(dto);

                // Pobieramy pełne dane (z Assetem) dla odpowiedzi
                var resultDto = TransactionMapper.ToDto(await _unitOfWork.Transactions.GetByIdAsync(createdTransaction.Id));

                return CreatedAtAction(nameof(Get), new { id = createdTransaction.Id }, resultDto);
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

        // Metody Update i Delete pozostają bez zmian (na razie)
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTransaction(int id, [FromBody] TransactionDto dto)
        {
            if (id != dto.Id) return BadRequest("ID w URL nie zgadza się z ID w ciele żądania.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var transaction = TransactionMapper.FromDto(dto);
            await _unitOfWork.Transactions.UpdateAsync(transaction);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            if (transaction == null) return NotFound();

            await _unitOfWork.Transactions.DeleteAsync(transaction);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}