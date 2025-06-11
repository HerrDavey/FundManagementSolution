using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Application.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }

        [Required]
        public int PorfolioId { get; set; }

        [Required]
        public int AssetId { get; set; }

        // Pola dodane dla czytelności w UI
        public string? AssetName { get; set; }
        public string? PortfolioInfo { get; set; }


        [Required]
        public DateTime TransactionDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity has to be higher than zero!")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price has to be higher than zero!")]
        public decimal Price { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty;
    }
}