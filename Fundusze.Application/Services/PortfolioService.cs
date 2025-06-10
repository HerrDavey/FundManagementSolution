using Fundusze.Application.DTOs;
using Fundusze.Application.Mappers;
using Fundusze.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fundusze.Application.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PortfolioService> _logger;

        public PortfolioService(IUnitOfWork unitOfWork, ILogger<PortfolioService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<PortfolioDetailsDto> GetPortfolioDetailsAsync(int portfolioId)
        {
            _logger.LogInformation("Pobieranie szczegółów dla portfela o ID: {PortfolioId}", portfolioId);

            var portfolio = await _unitOfWork.Portfolios.GetByIdAsync(portfolioId);
            if (portfolio == null)
            {
                throw new KeyNotFoundException($"Portfolio with ID {portfolioId} not found.");
            }

            var transactions = await _unitOfWork.Transactions.GetAllByPortfolioIdAsync(portfolioId);

            var aggregatedAssets = transactions
                .GroupBy(t => t.Asset)
                .Select(g =>
                {
                    var asset = g.Key;
                    int totalQuantity = g.Sum(t => t.Type == Domain.TransactionType.Buy ? t.Quantity : -t.Quantity);

                    return new AggregatedAssetDto
                    {
                        AssetId = asset.Id,
                        AssetName = asset.Name,
                        TotalQuantity = totalQuantity,
                        CurrentValue = totalQuantity * asset.Price
                    };
                })
                .Where(a => a.TotalQuantity > 0) // Pokazuj tylko aktywa, które faktycznie posiadamy
                .ToList();

            var result = new PortfolioDetailsDto
            {
                Id = portfolio.Id,
                FundName = portfolio.Fund?.Name,
                NAV = portfolio.NAV,
                CreatedDate = portfolio.CreatedDate,
                Transactions = transactions.Select(TransactionMapper.ToDto).ToList(),
                AggregatedAssets = aggregatedAssets
            };

            return result;
        }
    }
}