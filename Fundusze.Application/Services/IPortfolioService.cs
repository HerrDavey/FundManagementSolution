using Fundusze.Application.DTOs;
using System.Threading.Tasks;

namespace Fundusze.Application.Services
{
    public interface IPortfolioService
    {
        Task<PortfolioDetailsDto> GetPortfolioDetailsAsync(int portfolioId);
    }
}