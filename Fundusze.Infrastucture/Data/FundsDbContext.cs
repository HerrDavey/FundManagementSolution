using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fundusze.Domain;
using Fundusze.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Fundusze.Infrastucture.Data
{
    public class FundsDbContext: DbContext
    {
        public FundsDbContext(DbContextOptions<FundsDbContext> options) : base(options) { }

        public DbSet<Fund> Funds { get; set; }
        public DbSet<InvestmentPortfolio> Portfolios { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Asset> Assets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Transaction>()
                .Property(t => t.Type)
                .HasConversion<string>();
        }
    }
}
