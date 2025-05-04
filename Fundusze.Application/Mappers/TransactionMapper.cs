using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                PortfolioId = transaction.PorfolioId,
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
                PorfolioId = dto.PortfolioId,
                AssetId = dto.AssetId,
                TransactionDate = dto.TransactionDate,
                Quantity = dto.Quantity,
                Price = dto.Price,
                Type = Enum.TryParse(dto.Type, out TransactionType type) ? type : TransactionType.Buy
            };
        }
    }
}
