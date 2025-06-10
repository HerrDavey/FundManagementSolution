using Fundusze.Domain.Entities;
using Fundusze.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fundusze.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestmentPortfolioController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvestmentPortfolioController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvestmentPortfolio>>> GetAll()
        {
            // Na razie zostawiamy zwracanie encji, zajmiemy się tym w następnym kroku
            return Ok(await _unitOfWork.Portfolios.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvestmentPortfolio>> Get(int id)
        {
            // Na razie zostawiamy zwracanie encji, zajmiemy się tym w następnym kroku
            var portfolio = await _unitOfWork.Portfolios.GetByIdAsync(id);
            if (portfolio == null) return NotFound();

            return Ok(portfolio);
        }

        [HttpPost]
        public async Task<ActionResult> CreateInvestmentPortfolio(InvestmentPortfolio portfolio)
        {
            await _unitOfWork.Portfolios.AddAsync(portfolio);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(Get), new { id = portfolio.Id }, portfolio);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInvestmentPortfolio(int id, InvestmentPortfolio updated)
        {
            if (id != updated.Id) return BadRequest("ID w URL nie zgadza się z ID w ciele żądania.");

            if (!await _unitOfWork.Portfolios.ExistsAsync(id)) return NotFound();

            await _unitOfWork.Portfolios.UpdateAsync(updated);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInvestmentPortfolio(int id)
        {
            var portfolio = await _unitOfWork.Portfolios.GetByIdAsync(id);
            if (portfolio == null) return NotFound();

            await _unitOfWork.Portfolios.DeleteAsync(portfolio);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}