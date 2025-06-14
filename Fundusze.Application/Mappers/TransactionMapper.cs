using Fundusze.Domain;
using Fundusze.Application.DTOs;

namespace Fundusze.Application.Mappers
{
    public static class TransactionMapper
    {
        public static TransactionDto ToDto(Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                PorfolioId = transaction.PorfolioId,
                AssetId = transaction.AssetId,
                TransactionDate = transaction.TransactionDate,
                Quantity = transaction.Quantity,
                Price = transaction.Price,
                Type = transaction.Type.ToString(),
                AssetName = transaction.Asset?.Name,
                // POPRAWKA TUTAJ: Używamy nowego, lepszego formatu
                PortfolioInfo = $"{transaction.Portfolio?.Fund?.Name} (Portfel #{transaction.PorfolioId})"
            };
        }

        public static Transaction FromDto(TransactionDto dto)
        {
            return new Transaction
            {
                PorfolioId = dto.PorfolioId,
                AssetId = dto.AssetId,
                TransactionDate = dto.TransactionDate,
                Quantity = dto.Quantity,
                Price = dto.Price,
                Type = Enum.TryParse<TransactionType>(dto.Type, out var type) ? type : TransactionType.Buy
            };
        }
        
        public static Transaction FromDto(CreateTransactionDto dto)
        {
            return new Transaction
            {
                PorfolioId = dto.PorfolioId,
                AssetId = dto.AssetId,
                TransactionDate = dto.TransactionDate,
                Quantity = dto.Quantity,
                Price = dto.Price,
                Type = Enum.TryParse<TransactionType>(dto.Type, out var type) ? type : TransactionType.Buy
            };
        }
    }
}