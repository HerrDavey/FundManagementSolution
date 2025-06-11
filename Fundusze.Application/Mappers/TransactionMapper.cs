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
                // ZMIANA TUTAJ: Dodajemy nazwę funduszu
                PortfolioInfo = $"{transaction.Portfolio?.Fund?.Name} (Portfel #{transaction.PorfolioId})"
            };
        }

        // Ta metoda pozostaje bez zmian, używamy jej do aktualizacji
        public static Transaction FromDto(TransactionDto dto)
        {
            return new Transaction
            {
                PorfolioId = dto.PorfolioId,
                AssetId = dto.AssetId,
                TransactionDate = dto.TransactionDate,
                Quantity = dto.Quantity,
                Price = dto.Price,
                Type = Enum.TryParse(dto.Type, out TransactionType type) ? type : TransactionType.Buy
            };
        }

        // NOWA METODA specjalnie dla tworzenia transakcji
        public static Transaction FromDto(CreateTransactionDto dto)
        {
            return new Transaction
            {
                PorfolioId = dto.PorfolioId,
                AssetId = dto.AssetId,
                TransactionDate = dto.TransactionDate,
                Quantity = dto.Quantity,
                Price = dto.Price,
                Type = Enum.TryParse(dto.Type, out TransactionType type) ? type : TransactionType.Buy
            };
        }
    }
}