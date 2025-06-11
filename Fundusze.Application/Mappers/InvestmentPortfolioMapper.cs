using Fundusze.Application.DTOs;
using Fundusze.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Application.Mappers
{
    public static class InvestmentPortfolioMapper
    {
        public static InvestmentPortfolioDto ToDto(InvestmentPortfolio portfolio)
        {
            return new InvestmentPortfolioDto
            {
                Id = portfolio.Id,
                FundId = portfolio.FundId,
                FundName = portfolio.Fund?.Name,
                NAV = portfolio.NAV,
                CreatedDate = portfolio.CreatedDate
            };
        }

        public static InvestmentPortfolio FromDto(InvestmentPortfolioDto dto)
        {
            return new InvestmentPortfolio
            {
                Id = dto.Id,
                FundId = dto.FundId,
                NAV = dto.NAV,
                CreatedDate = dto.CreatedDate
            };
        }
    }
}