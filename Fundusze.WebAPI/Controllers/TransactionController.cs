using Microsoft.AspNetCore.Mvc;
using Fundusze.Domain.Interfaces;
using Fundusze.Application.DTOs;
using Fundusze.Application.Mappers;

namespace Fundusze.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public async Task<ActionResult> CreateTransaction([FromBody] TransactionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var transaction = TransactionMapper.FromDto(dto);
            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(Get), new { id = transaction.Id }, TransactionMapper.ToDto(transaction));
        }

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