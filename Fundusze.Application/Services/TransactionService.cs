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

        public async Task<Transaction> AddTransactionAndUpdatePortfolioAsync(TransactionDto transactionDto)
        {
            _logger.LogInformation("Rozpoczynanie operacji dodania transakcji i aktualizacji portfela dla PortfolioId: {PortfolioId}", transactionDto.PortfolioId);

            var portfolio = await _unitOfWork.Portfolios.GetByIdAsync(transactionDto.PortfolioId);
            if (portfolio == null)
            {
                _logger.LogError("Nie znaleziono portfela o ID: {PortfolioId}", transactionDto.PortfolioId);
                throw new KeyNotFoundException($"Portfolio with ID {transactionDto.PortfolioId} not found.");
            }

            if (!await _unitOfWork.Assets.ExistsAsync(transactionDto.AssetId))
            {
                _logger.LogError("Nie znaleziono aktywa o ID: {AssetId}", transactionDto.AssetId);
                throw new KeyNotFoundException($"Asset with ID {transactionDto.AssetId} not found.");
            }

            var transactionValue = transactionDto.Quantity * transactionDto.Price;

            switch (transactionDto.Type)
            {
                case "Buy":
                    portfolio.NAV += transactionValue;
                    _logger.LogInformation("Zwiększono NAV portfela o {TransactionValue}", transactionValue);
                    break;

                case "Sell":
                    // NOWA LOGIKA BIZNESOWA - WERYFIKACJA STANU POSIADANIA
                    var existingTransactions = await _unitOfWork.Transactions.GetAllByPortfolioIdAsync(portfolio.Id);
                    var currentAssetHolding = existingTransactions
                        .Where(t => t.AssetId == transactionDto.AssetId)
                        .Sum(t => t.Type == Domain.TransactionType.Buy ? t.Quantity : -t.Quantity);

                    if (currentAssetHolding < transactionDto.Quantity)
                    {
                        _logger.LogError("Próba sprzedaży {Quantity} jednostek aktywa {AssetId}, podczas gdy w portfelu jest tylko {CurrentHolding}",
                            transactionDto.Quantity, transactionDto.AssetId, currentAssetHolding);
                        throw new InvalidOperationException($"Próba sprzedaży większej liczby aktywów ({transactionDto.Quantity}) niż jest w portfelu ({currentAssetHolding}).");
                    }

                    portfolio.NAV -= transactionValue;
                    _logger.LogInformation("Zmniejszono NAV portfela o {TransactionValue}", transactionValue);
                    break;

                default:
                    _logger.LogWarning("Nierozpoznany typ transakcji: {TransactionType}", transactionDto.Type);
                    break;
            }

            var newTransaction = TransactionMapper.FromDto(transactionDto);

            await _unitOfWork.Transactions.AddAsync(newTransaction);
            await _unitOfWork.Portfolios.UpdateAsync(portfolio);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Pomyślnie dodano transakcję i zaktualizowano portfel.");

            return newTransaction;
        }
    }
}