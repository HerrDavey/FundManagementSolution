using Fundusze.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAssetRepository Assets { get; }
        IFundRepository Funds { get; }
        IInvestmentPortfolioRepository Portfolios { get; }
        ITransactionRepository Transactions { get; }

        Task<int> CompleteAsync();
    }
}