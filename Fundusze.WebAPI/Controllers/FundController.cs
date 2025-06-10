using Microsoft.AspNetCore.Mvc;
using Fundusze.Domain.Interfaces;
using Fundusze.Application.DTOs;
using Fundusze.Application.Mappers;

namespace Fundusze.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FundController> _logger; // Dodajemy logger

        // Wstrzykujemy ILogger przez konstruktor
        public FundController(IUnitOfWork unitOfWork, ILogger<FundController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FundDto>>> GetFunds()
        {
            _logger.LogInformation("Pobieranie wszystkich funduszy.");
            try
            {
                var funds = await _unitOfWork.Funds.GetAllAsync();
                return Ok(funds.Select(FundMapper.ToDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Wystąpił błąd podczas pobierania funduszy.");
                return StatusCode(500, "Wystąpił wewnętrzny błąd serwera.");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<FundDto>> GetFund(int id)
        {
            _logger.LogInformation("Pobieranie funduszu o ID: {FundId}", id);
            var fund = await _unitOfWork.Funds.GetByIdAsync(id);
            if (fund == null)
            {
                _logger.LogWarning("Nie znaleziono funduszu o ID: {FundId}", id);
                return NotFound();
            }
            return Ok(FundMapper.ToDto(fund));
        }

        [HttpPost]
        public async Task<ActionResult> CreateFund([FromBody] FundDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("Tworzenie nowego funduszu o nazwie: {FundName}", dto.Name);
            var fund = FundMapper.FromDto(dto);
            await _unitOfWork.Funds.AddAsync(fund);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("Stworzono fundusz o ID: {FundId}", fund.Id);

            return CreatedAtAction(nameof(GetFund), new { id = fund.Id }, FundMapper.ToDto(fund));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFund(int id, [FromBody] FundDto dto)
        {
            if (id != dto.Id) return BadRequest("ID w URL nie zgadza się z ID w ciele żądania.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("Aktualizowanie funduszu o ID: {FundId}", id);
            var fund = FundMapper.FromDto(dto);
            await _unitOfWork.Funds.UpdateAsync(fund);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFund(int id)
        {
            _logger.LogInformation("Próba usunięcia funduszu o ID: {FundId}", id);
            var fund = await _unitOfWork.Funds.GetByIdAsync(id);
            if (fund == null)
            {
                _logger.LogWarning("Nie znaleziono funduszu do usunięcia o ID: {FundId}", id);
                return NotFound();
            }

            await _unitOfWork.Funds.DeleteAsync(fund);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("Usunięto fundusz o ID: {FundId}", id);

            return NoContent();
        }
    }
}