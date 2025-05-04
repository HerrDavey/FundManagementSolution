using Fundusze.Domain;
using Fundusze.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Fundusze.Application.DTOs;
using Fundusze.Application.Mappers;

namespace Fundusze.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController: ControllerBase
    {
        private readonly IAssetRepository _repository;

        public AssetController(IAssetRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssets()
        {
            var assets = await _repository.GetAllAsync();
            return Ok(assets.Select(AssetMapper.ToDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssetDto>> GetAsset(int id)
        {
            var asset = await _repository.GetByIdAsync(id);
            if(asset == null)  return NotFound();

            return Ok(AssetMapper.ToDto(asset));
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsset([FromBody] AssetDto dto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var asset = AssetMapper.FromDto(dto);
            await _repository.AddAsync(asset);
            return CreatedAtAction(nameof(GetAsset), new {id = asset.Id}, AssetMapper.ToDto(asset));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsset (int id, [FromBody] AssetDto dto)
        {
            if (id != dto.Id) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _repository.ExistsAsync(id)) return NotFound();

            var asset = AssetMapper.FromDto(dto);
            await _repository.UpdateAsync(asset);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsset(int id)
        {
            var asset = await _repository.GetByIdAsync(id);
            if (asset == null) return NotFound();

            await _repository.DeleteAsync(asset);
            return NoContent();
        }

    }
}
