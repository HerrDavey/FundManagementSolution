using System;
using System.ComponentModel.DataAnnotations;

namespace Fundusze.Application.DTOs
{
    public class CreateTransactionDto
    {
        [Required]
        public int PorfolioId { get; set; } 

        [Required]
        public int AssetId { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty;
    }
}