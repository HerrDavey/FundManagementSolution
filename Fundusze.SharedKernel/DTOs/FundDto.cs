using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Application.DTOs
{
    public class FundDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Managed { get; set; } = string.Empty;

        [Required]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

    }
}
