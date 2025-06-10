using Fundusze.Domain.Interfaces;
using Fundusze.Infrastucture.Data;
using Fundusze.Infrastucture.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundusze.Infrastucture
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FundsDbContext _context;

        public IAssetRepository Assets { get; private set; }
        public IFundRepository Funds { get; private set; }
        public IInvestmentPortfolioRepository Portfolios { get; private set; }
        public ITransactionRepository Transactions { get; private set; }

        public UnitOfWork(FundsDbContext context)
        {
            _context = context;

            Assets = new AssetRepository(_context);
            Funds = new FundRepository(_context);
            Portfolios = new InvestmentPortfolioRepository(_context);
            Transactions = new TransactionRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}