using Fundusze.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Fundusze.Application.DTOs;
using Fundusze.Application.Mappers;

namespace Fundusze.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssetController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssets()
        {
            var assets = await _unitOfWork.Assets.GetAllAsync();
            return Ok(assets.Select(AssetMapper.ToDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssetDto>> GetAsset(int id)
        {
            var asset = await _unitOfWork.Assets.GetByIdAsync(id);
            if (asset == null) return NotFound();

            return Ok(AssetMapper.ToDto(asset));
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsset([FromBody] AssetDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var asset = AssetMapper.FromDto(dto);
            await _unitOfWork.Assets.AddAsync(asset);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetAsset), new { id = asset.Id }, AssetMapper.ToDto(asset));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsset(int id, [FromBody] AssetDto dto)
        {
            if (id != dto.Id) return BadRequest("ID w URL nie zgadza się z ID w ciele żądania.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var asset = AssetMapper.FromDto(dto);
            await _unitOfWork.Assets.UpdateAsync(asset);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsset(int id)
        {
            var asset = await _unitOfWork.Assets.GetByIdAsync(id);
            if (asset == null) return NotFound();

            await _unitOfWork.Assets.DeleteAsync(asset);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}