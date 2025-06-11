using Fundusze.Application.DTOs;
using Fundusze.Application.Mappers;
using Fundusze.Application.Services;
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
        private readonly IPortfolioService _portfolioService;

        public InvestmentPortfolioController(IUnitOfWork unitOfWork, IPortfolioService portfolioService)
        {
            _unitOfWork = unitOfWork;
            _portfolioService = portfolioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvestmentPortfolioDto>>> GetAll()
        {
            var portfolios = await _unitOfWork.Portfolios.GetAllAsync();
            var portfoliosDto = portfolios.Select(InvestmentPortfolioMapper.ToDto);
            return Ok(portfoliosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvestmentPortfolioDto>> Get(int id)
        {
            var portfolio = await _unitOfWork.Portfolios.GetByIdAsync(id);
            if (portfolio == null) return NotFound();

            return Ok(InvestmentPortfolioMapper.ToDto(portfolio));
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<PortfolioDetailsDto>> GetDetails(int id)
        {
            try
            {
                var details = await _portfolioService.GetPortfolioDetailsAsync(id);
                return Ok(details);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<InvestmentPortfolioDto>> CreateInvestmentPortfolio([FromBody] InvestmentPortfolioDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var portfolio = InvestmentPortfolioMapper.FromDto(dto);
            await _unitOfWork.Portfolios.AddAsync(portfolio);
            await _unitOfWork.CompleteAsync();

            var createdPortfolio = await _unitOfWork.Portfolios.GetByIdAsync(portfolio.Id);
            return CreatedAtAction(nameof(Get), new { id = portfolio.Id }, InvestmentPortfolioMapper.ToDto(createdPortfolio));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInvestmentPortfolio(int id, [FromBody] InvestmentPortfolioDto dto)
        {
            if (id != dto.Id) return BadRequest("ID w URL nie zgadza się z ID w ciele żądania.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var portfolio = InvestmentPortfolioMapper.FromDto(dto);
            await _unitOfWork.Portfolios.UpdateAsync(portfolio);
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