using Fundusze.Domain.Entities;

namespace Fundusze.Domain
{
    public enum TransactionType { Buy, Sell }

    public class Transaction
    {
        public int Id { get; set; }

        public int PorfolioId { get; set; }
        public InvestmentPortfolio? Portfolio { get; set; }

        public int AssetId { get; set; }
        public Asset? Asset { get; set; }

        public DateTime TransactionDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public TransactionType Type { get; set; }
    }
}