using Microsoft.AspNetCore.Mvc;
using Fundusze.Domain;
using Fundusze.Domain.Interfaces;
using Fundusze.Application.DTOs;
using Fundusze.Application.Mappers;

namespace Fundusze.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController: ControllerBase
    {
        private readonly ITransactionRepository _repository;

        public TransactionController(ITransactionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll()
        {
            var transactions = await _repository.GetAllAsync();
            return Ok(transactions.Select(TransactionMapper.ToDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> Get(int id)
        {
            var transaction = await _repository.GetByIdAsync(id);
            if (transaction == null) return NotFound();

            return Ok(TransactionMapper.ToDto(transaction));
        }

        [HttpPost]
        public async Task<ActionResult> CreateTransaction([FromBody] TransactionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var transaction = TransactionMapper.FromDto(dto);
            await _repository.AddAsync(transaction);
            
            return CreatedAtAction(nameof(Get), new { id = transaction.Id }, TransactionMapper.ToDto(transaction));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTransaction(int id, [FromBody] TransactionDto dto)
        {
            if (id != dto.Id) return BadRequest();
            if(!ModelState.IsValid) return BadRequest(ModelState);
            if(!await _repository.ExistsAsync(id)) return NotFound();


            var transaction = TransactionMapper.FromDto(dto);
            await _repository.UpdateAsync(transaction);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            var transaction = await _repository.GetByIdAsync(id);
            if (transaction == null) return NotFound();

            await _repository.DeleteAsync(transaction);
            return NoContent();
        }

    }
}
