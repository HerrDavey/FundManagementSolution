using Microsoft.AspNetCore.Mvc;
using Fundusze.Domain;
using Fundusze.Domain.Interfaces;


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
        public async Task<ActionResult<IEnumerable<Transaction>>> GetAll()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> Get(int id)
        {
            var transaction = await _repository.GetByIdAsync(id);
            if (transaction == null) return NotFound();

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTransaction(Transaction transaction)
        {
            await _repository.AddAsync(transaction);
            return CreatedAtAction(nameof(Get), new { id = transaction.Id }, transaction);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTransaction(int id, Transaction updated)
        {
            if (id != updated.Id) return BadRequest();

            if(!await _repository.ExistsAsync(id)) return NotFound();
            
            await _repository.UpdateAsync(updated);
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
