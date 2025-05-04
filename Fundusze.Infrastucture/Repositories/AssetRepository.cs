using Fundusze.Domain.Interfaces;
using Fundusze.Domain;
using Fundusze.Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Infrastucture.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly FundsDbContext _context;

        public AssetRepository(FundsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Asset asset)
        {
            await _context.Assets.AddAsync(asset);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Asset asset)
        {
            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Assets.AnyAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Asset>> GetAllAsync()
        {
            return await _context.Assets.ToListAsync();
        }

        public async Task<Asset?> GetByIdAsync(int id)
        {
            return await _context.Assets.FindAsync(id);
        }

        public async Task UpdateAsync(Asset asset)
        {
            _context.Assets.Update(asset);
            await _context.SaveChangesAsync();
        }
    }
}
