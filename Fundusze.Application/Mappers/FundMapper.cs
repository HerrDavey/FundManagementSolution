using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fundusze.Domain.Entities;
using Fundusze.Application.DTOs;

namespace Fundusze.Application.Mappers
{
    public static class FundMapper
    {
        public static FundDto ToDto(Fund fund)
        {
            return new FundDto
            {
                Id = fund.Id,
                Name = fund.Name,
                Type = fund.Type,
                Managed = fund.Managed,
                CreatedDate = fund.CreatedDate

            };
        }

        public static Fund FromDto(FundDto dto)
        {
            return new Fund
            {
                Id = dto.Id,
                Name = dto.Name,
                Type = dto.Type,
                Managed = dto.Managed,
                CreatedDate = dto.CreatedDate ?? DateTime.Now,
            };
        }
    }
}
