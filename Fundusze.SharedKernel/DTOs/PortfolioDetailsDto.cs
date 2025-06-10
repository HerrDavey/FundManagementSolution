using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Application.DTOs
{
    public class PortfolioDetailsDto
    {
        public int Id { get; set; }
        public string? FundName { get; set; }
        public decimal NAV { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<AggregatedAssetDto> AggregatedAssets { get; set; } = new();
        public List<TransactionDto> Transactions { get; set; } = new();
    }
}