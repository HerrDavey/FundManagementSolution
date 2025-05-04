using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fundusze.Domain;

namespace Fundusze.Domain.Interfaces
{
    public interface IAssetRepository
    {
        Task<IEnumerable<Asset>> GetAllAsync ();
        Task<Asset?> GetByIdAsync(int id);
        Task AddAsync (Asset asset);
        Task UpdateAsync (Asset asset);
        Task DeleteAsync (Asset asset);
        Task<bool> ExistsAsync(int id);
    }
}
