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
                // POPRAWKA TUTAJ
                PortfolioId = transaction.PortfolioId,
                AssetId = transaction.AssetId,
                TransactionDate = transaction.TransactionDate,
                Quantity = transaction.Quantity,
                Price = transaction.Price,
                Type = transaction.Type.ToString(),
            };
        }

        public static Transaction FromDto(TransactionDto dto)
        {
            return new Transaction
            {
                Id = dto.Id,
                // POPRAWKA TUTAJ
                PortfolioId = dto.PortfolioId,
                AssetId = dto.AssetId,
                TransactionDate = dto.TransactionDate,
                Quantity = dto.Quantity,
                Price = dto.Price,
                Type = Enum.TryParse(dto.Type, out TransactionType type) ? type : TransactionType.Buy
            };
        }
    }
}