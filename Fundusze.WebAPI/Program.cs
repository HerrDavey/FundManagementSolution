using Fundusze.Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using Fundusze.Domain.Interfaces;
using Fundusze.Infrastucture.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Dodanie FundsDbContext
builder.Services.AddDbContext<FundsDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Rejestracja Repozytoriów
builder.Services.AddScoped<IFundRepository, FundRepository>();
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IInvestmentPortfolioRepository, InvestmentPortfolioRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
