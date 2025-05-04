using Fundusze.Domain.Entities;
using Fundusze.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fundusze.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentPortfolioController: ControllerBase
    {
        private readonly IInvestmentPortfolioRepository _repository;

        public InvestmentPortfolioController(IInvestmentPortfolioRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvestmentPortfolio>>> GetAll()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvestmentPortfolio>> Get(int id)
        {
            var portfolio = await _repository.GetByIdAsync(id);
            if(portfolio == null) return NotFound();

            return Ok(portfolio);
        }

        [HttpPost]
        public async Task<ActionResult> CreateInvestmentPortfolio(InvestmentPortfolio portfolio)
        {
            await _repository.AddAsync(portfolio);
            return CreatedAtAction(nameof(Get), new { id = portfolio.Id }, portfolio);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInvestmentPortfolio(int id, InvestmentPortfolio updated)
        {
            if (id != updated.Id) return BadRequest();

            if (!await _repository.ExistsAsync(id)) return NotFound();

            await _repository.UpdateAsync(updated);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvestmentPortfolio(int id)
        {
            var portfolio = await _repository.GetByIdAsync(id);
            if (portfolio == null) return NotFound();

            await _repository.DeleteAsync(portfolio);
            return NoContent();
        }

    }
}
