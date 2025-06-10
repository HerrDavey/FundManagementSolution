using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Application.DTOs
{
    public class InvestmentPortfolioDto
    {
        public int Id { get; set; }

        [Required]
        public int FundId { get; set; }

        // Dodajemy nazwę funduszu dla wygody frontendu
        public string? FundName { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal NAV { get; set; } // Net Asset Value

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}