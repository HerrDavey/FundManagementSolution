using Fundusze.Application.DTOs;
using Fundusze.Application.Mappers;
using Fundusze.Domain;
using Fundusze.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
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
            _logger.LogInformation("Rozpoczynanie operacji dodania transakcji dla PorfolioId: {PorfolioId}", transactionDto.PorfolioId);

            var portfolio = await _unitOfWork.Portfolios.GetByIdAsync(transactionDto.PorfolioId);
            if (portfolio == null) { throw new KeyNotFoundException($"Portfolio with ID {transactionDto.PorfolioId} not found."); }

            var asset = await _unitOfWork.Assets.GetByIdAsync(transactionDto.AssetId);
            if (asset == null) { throw new KeyNotFoundException($"Asset with ID {transactionDto.AssetId} not found."); }

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
            await _unitOfWork.Portfolios.UpdateAsync(portfolio);

            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("Pomyślnie dodano transakcję i zaktualizowano portfel.");
            return newTransaction;
        }

        public async Task DeleteTransactionAndUpdatePortfolioAsync(int transactionId)
        {
            _logger.LogInformation("Rozpoczynanie operacji usunięcia transakcji o ID: {TransactionId}", transactionId);

            var transactionToDelete = await _unitOfWork.Transactions.GetByIdAsync(transactionId);
            if (transactionToDelete == null) { throw new KeyNotFoundException($"Transaction with ID {transactionId} not found."); }

            var portfolio = await _unitOfWork.Portfolios.GetByIdAsync(transactionToDelete.PorfolioId);
            if (portfolio == null) { throw new KeyNotFoundException($"Associated portfolio with ID {transactionToDelete.PorfolioId} not found."); }

            var transactionValue = transactionToDelete.Quantity * transactionToDelete.Price;
            portfolio.NAV -= (transactionToDelete.Type == TransactionType.Buy ? transactionValue : -transactionValue);
            _logger.LogInformation("Zaktualizowano NAV portfela o {TransactionValue}", -transactionValue);

            await _unitOfWork.Transactions.DeleteAsync(transactionToDelete);
            await _unitOfWork.Portfolios.UpdateAsync(portfolio);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Pomyślnie usunięto transakcję i zaktualizowano portfel.");
        }

        public async Task UpdateTransactionAndUpdatePortfolioAsync(TransactionDto transactionDto)
        {
            _logger.LogInformation("Rozpoczynanie operacji edycji transakcji o ID: {TransactionId}", transactionDto.Id);

            var originalTransaction = await _unitOfWork.Transactions.GetByIdAsync(transactionDto.Id);
            if (originalTransaction == null) { throw new KeyNotFoundException($"Transaction with ID {transactionDto.Id} not found to update."); }

            var portfolio = await _unitOfWork.Portfolios.GetByIdAsync(originalTransaction.PorfolioId);
            if (portfolio == null) { throw new KeyNotFoundException($"Associated portfolio with ID {originalTransaction.PorfolioId} not found."); }

            // Krok 1: Wycofaj efekt starej transakcji
            var originalValue = originalTransaction.Quantity * originalTransaction.Price;
            portfolio.NAV -= (originalTransaction.Type == TransactionType.Buy ? originalValue : -originalValue);

            // Krok 2: Zastosuj efekt nowej transakcji
            var updatedValue = transactionDto.Quantity * transactionDto.Price;
            portfolio.NAV += (Enum.Parse<TransactionType>(transactionDto.Type) == TransactionType.Buy ? updatedValue : -updatedValue);

            // Krok 3: Zaktualizuj samą transakcję
            originalTransaction.TransactionDate = transactionDto.TransactionDate;
            originalTransaction.Quantity = transactionDto.Quantity;
            originalTransaction.Price = transactionDto.Price;

            await _unitOfWork.Transactions.UpdateAsync(originalTransaction);
            await _unitOfWork.Portfolios.UpdateAsync(portfolio);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Pomyślnie zedytowano transakcję i zaktualizowano portfel.");
        }
    }
}