using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fundusze.Domain.Entities;
using Fundusze.Domain.Interfaces;
using Fundusze.Infrastucture.Data;
using Microsoft.EntityFrameworkCore;


namespace Fundusze.Infrastucture.Repositories
{
    public class FundRepository : IFundRepository
    {
        private readonly FundsDbContext _context;

        public FundRepository(FundsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Fund fund)
        {
            await _context.Funds.AddAsync(fund);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Fund fund)
        {
            _context.Funds.Remove(fund);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Funds.AnyAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Fund>> GetAllAsync()
        {
            return await _context.Funds.ToListAsync();
        }

        public async Task<Fund?> GetByIdAsync(int id)
        {
            return await _context.Funds.FindAsync(id);
        }

        public async Task UpdateAsync(Fund fund)
        {
            _context.Funds.Update(fund);
            await _context.SaveChangesAsync();
        }
    }
}
