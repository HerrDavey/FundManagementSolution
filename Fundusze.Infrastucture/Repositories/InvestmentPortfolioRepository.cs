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
    public class InvestmentPortfolioRepository: IInvestmentPortfolioRepository
    {
        private readonly FundsDbContext _context;

        public InvestmentPortfolioRepository(FundsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(InvestmentPortfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(InvestmentPortfolio portfolio)
        {
            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Portfolios.AnyAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<InvestmentPortfolio>> GetAllAsync()
        {
            return await _context.Portfolios.Include(x => x.Fund).ToListAsync();
        }

        public async Task<InvestmentPortfolio?> GetByIdAsync(int id)
        {
            return await _context.Portfolios.Include(x => x.Fund).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdateAsync(InvestmentPortfolio portfolio)
        {
            _context.Portfolios.Update(portfolio);
            await _context.SaveChangesAsync();
        }
    }
}
