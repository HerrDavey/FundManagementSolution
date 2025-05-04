using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fundusze.Domain.Entities;

namespace Fundusze.Domain.Interfaces
{
    public interface IFundRepository
    {
        Task<IEnumerable<Fund>> GetAllAsync();
        Task<Fund?> GetByIdAsync(int id);
        Task AddAsync(Fund fund);
        Task UpdateAsync(Fund fund);
        Task DeleteAsync(Fund Fund);
        Task<bool> ExistsAsync(int id);
    }
}
