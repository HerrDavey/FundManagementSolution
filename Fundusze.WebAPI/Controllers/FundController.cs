using Microsoft.AspNetCore.Mvc;
using Fundusze.Domain.Entities;
using Fundusze.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Fundusze.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundController: ControllerBase
    {
        private readonly IFundRepository _repository;

        public FundController(IFundRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fund>>> GetFunds()
        {
            return Ok(await _repository.GetAllAsync());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Fund>> GetFund(int id)
        {
            var fund = await _repository.GetByIdAsync(id);
            if (fund == null) return NotFound();
            return Ok(fund);
        }

        [HttpPost]
        public async Task<ActionResult> CreateFund(Fund fund)
        {
            await _repository.AddAsync(fund);
            return CreatedAtAction(nameof(GetFund), new {id = fund.Id}, fund);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFund(int id, Fund updated)
        {
            if (id != updated.Id) return BadRequest();

            if (!await _repository.ExistsAsync(id)) return NotFound();

            await _repository.UpdateAsync(updated);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFund(int id)
        {
            var fund = await _repository.GetByIdAsync(id);
            if (fund == null) return NotFound();

            await _repository.DeleteAsync(fund);
            return NoContent();
        }

    }
}
