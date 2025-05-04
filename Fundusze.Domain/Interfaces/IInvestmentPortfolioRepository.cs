using Fundusze.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Domain.Interfaces
{
    public interface IInvestmentPortfolioRepository
    {
        Task<IEnumerable<InvestmentPortfolio>> GetAllAsync();
        Task<InvestmentPortfolio?> GetByIdAsync(int id);
        Task AddAsync(InvestmentPortfolio portfolio);
        Task UpdateAsync(InvestmentPortfolio portfolio);
        Task DeleteAsync(InvestmentPortfolio portfolio);
        Task<bool> ExistsAsync(int id);
    }
}
