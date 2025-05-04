using Fundusze.Domain;
using Fundusze.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Asset>> GetAsset(int id)
        {
            var asset = await _repository.GetByIdAsync(id);
            if(asset == null)  return NotFound();

            return Ok(asset);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsset(Asset asset)
        {
            await _repository.AddAsync(asset);
            return CreatedAtAction(nameof(GetAsset), new {id = asset.Id}, asset);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsset (int id, Asset updated)
        {
            if (id != updated.Id) return BadRequest();

            if (!await _repository.ExistsAsync(id)) return NotFound();

            await _repository.UpdateAsync(updated);
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
