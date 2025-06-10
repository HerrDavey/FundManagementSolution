using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Application.DTOs
{
    public class AggregatedAssetDto
    {
        public int AssetId { get; set; }
        public string? AssetName { get; set; }
        public int TotalQuantity { get; set; }
        public decimal CurrentValue { get; set; }
    }
}