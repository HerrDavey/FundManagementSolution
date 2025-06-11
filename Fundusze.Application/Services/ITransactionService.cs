using Fundusze.Application.DTOs;
using Fundusze.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Application.Services
{
    public interface ITransactionService
    {
        Task<Transaction> AddTransactionAndUpdatePortfolioAsync(CreateTransactionDto transactionDto);
    }
}