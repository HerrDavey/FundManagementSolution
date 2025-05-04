using Microsoft.AspNetCore.Mvc;
using Fundusze.Domain.Entities;
using Fundusze.Domain.Interfaces;
using Fundusze.Application.DTOs;
using Fundusze.Application.Mappers;
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
        public async Task<ActionResult<IEnumerable<FundDto>>> GetFunds()
        {
            var funds = await _repository.GetAllAsync();
            return Ok(funds.Select(FundMapper.ToDto));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<FundDto>> GetFund(int id)
        {
            var fund = await _repository.GetByIdAsync(id);
            if (fund == null) return NotFound();
            return Ok(FundMapper.ToDto(fund));
        }

        [HttpPost]
        public async Task<ActionResult> CreateFund([FromBody] FundDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var fund = FundMapper.FromDto(dto);
            await _repository.AddAsync(fund);

            return CreatedAtAction(nameof(GetFund), new { id = fund.Id }, FundMapper.ToDto(fund));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFund(int id, [FromBody] FundDto dto)
        {
            if (id != dto.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _repository.ExistsAsync(id)) return NotFound();

            var fund = FundMapper.FromDto(dto);
            await _repository.UpdateAsync(fund);

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
