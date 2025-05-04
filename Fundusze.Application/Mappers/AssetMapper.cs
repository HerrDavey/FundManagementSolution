using Fundusze.Domain;
using Fundusze.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Application.Mappers
{
    public static class AssetMapper
    {
        public static AssetDto ToDto(Asset asset)
        {
            return new AssetDto
            {
                Id = asset.Id,
                Name = asset.Name,
                Type = asset.Type,
                ISIN = asset.ISIN,
                Price = asset.Price,
            };
        }

        public static Asset FromDto(AssetDto dto)
        {
            return new Asset
            {
                Id = dto.Id,
                Name = dto.Name,
                Type = dto.Type,
                ISIN = dto.ISIN,
                Price = dto.Price,
            };
        }
    }
}
