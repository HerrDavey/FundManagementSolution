using Fundusze.Domain;
using Fundusze.Domain.Interfaces;
using Fundusze.Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fundusze.Infrastucture.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly FundsDbContext _context;

        public TransactionRepository(FundsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            _context.Transactions.Remove(transaction);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Transactions.AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Transactions
                .Include(p => p.Portfolio).ThenInclude(f => f.Fund) // <-- ZMIANA
                .Include(a => a.Asset)
                .ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(p => p.Portfolio).ThenInclude(f => f.Fund) // <-- ZMIANA
                .Include(a => a.Asset)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Transaction>> GetAllByPortfolioIdAsync(int portfolioId)
        {
            return await _context.Transactions
                .Where(t => t.PorfolioId == portfolioId)
                .Include(p => p.Portfolio).ThenInclude(f => f.Fund) // <-- ZMIANA
                .Include(a => a.Asset)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }
    }
}