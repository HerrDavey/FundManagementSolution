using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Domain.Entities
{
    public class InvestmentPortfolio
    {
        public int Id { get; set; }

        public int FundId { get; set; }
        public Fund Fund { get; set; }

        public decimal NAV { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
