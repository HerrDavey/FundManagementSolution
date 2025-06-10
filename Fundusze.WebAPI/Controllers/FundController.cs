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

        public FundController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FundDto>>> GetFunds()
        {
            var funds = await _unitOfWork.Funds.GetAllAsync();
            return Ok(funds.Select(FundMapper.ToDto));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<FundDto>> GetFund(int id)
        {
            var fund = await _unitOfWork.Funds.GetByIdAsync(id);
            if (fund == null) return NotFound();
            return Ok(FundMapper.ToDto(fund));
        }

        [HttpPost]
        public async Task<ActionResult> CreateFund([FromBody] FundDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var fund = FundMapper.FromDto(dto);
            await _unitOfWork.Funds.AddAsync(fund);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetFund), new { id = fund.Id }, FundMapper.ToDto(fund));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFund(int id, [FromBody] FundDto dto)
        {
            if (id != dto.Id) return BadRequest("ID w URL nie zgadza się z ID w ciele żądania.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var fund = FundMapper.FromDto(dto);
            await _unitOfWork.Funds.UpdateAsync(fund);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFund(int id)
        {
            var fund = await _unitOfWork.Funds.GetByIdAsync(id);
            if (fund == null) return NotFound();

            await _unitOfWork.Funds.DeleteAsync(fund);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}