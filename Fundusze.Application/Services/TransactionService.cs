using Fundusze.Application.DTOs;
using Fundusze.Application.Mappers;
using Fundusze.Domain;
using Fundusze.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fundusze.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(IUnitOfWork unitOfWork, ILogger<TransactionService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Transaction> AddTransactionAndUpdatePortfolioAsync(CreateTransactionDto transactionDto)
        {
            _logger.LogInformation("Rozpoczynanie operacji dodania transakcji dla PortfolioId: {PortfolioId}", transactionDto.PorfolioId);

            var portfolio = await _unitOfWork.Portfolios.GetByIdAsync(transactionDto.PorfolioId);
            if (portfolio == null)
            {
                throw new KeyNotFoundException($"Portfolio with ID {transactionDto.PorfolioId} not found.");
            }

            var asset = await _unitOfWork.Assets.GetByIdAsync(transactionDto.AssetId);
            if (asset == null)
            {
                throw new KeyNotFoundException($"Asset with ID {transactionDto.AssetId} not found.");
            }

            if (transactionDto.Type == "Sell")
            {
                var existingTransactions = await _unitOfWork.Transactions.GetAllByPortfolioIdAsync(portfolio.Id);
                var currentAssetHolding = existingTransactions
                    .Where(t => t.AssetId == transactionDto.AssetId)
                    .Sum(t => t.Type == Domain.TransactionType.Buy ? t.Quantity : -t.Quantity);

                if (currentAssetHolding < transactionDto.Quantity)
                {
                    throw new InvalidOperationException($"Próba sprzedaży większej liczby aktywów ({transactionDto.Quantity}) niż jest w portfelu ({currentAssetHolding}).");
                }
            }

            var transactionValue = transactionDto.Quantity * transactionDto.Price;
            portfolio.NAV += (transactionDto.Type == "Buy" ? transactionValue : -transactionValue);

            var newTransaction = TransactionMapper.FromDto(transactionDto);

            newTransaction.Portfolio = portfolio;
            newTransaction.Asset = asset;

            await _unitOfWork.Transactions.AddAsync(newTransaction);

            // OSTATECZNA POPRAWKA: Jawnie informujemy UnitOfWork, że obiekt portfela
            // został zmodyfikowany i jego stan musi zostać zaktualizowany w bazie.
            await _unitOfWork.Portfolios.UpdateAsync(portfolio);

            // Ten krok zapisze teraz obie zmiany (nową transakcję i zaktualizowany portfel)
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Pomyślnie dodano transakcję i zaktualizowano portfel.");
            return newTransaction;
        }
    }
}