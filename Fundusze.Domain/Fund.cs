using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Domain.Entities
{
    public class Fund
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Managed { get; set; } = string.Empty;
        public DateTime DataUtworzenia { get; set; }

        public ICollection<InvestmentPortfolio> Portfolios { get; set; } = new List<InvestmentPortfolio>();
    }
}
