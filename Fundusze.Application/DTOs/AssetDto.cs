using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Application.DTOs
{
    public class AssetDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string ISIN { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price has to be higher than zero!")]
        public decimal Price { get; set; }
    }
}
