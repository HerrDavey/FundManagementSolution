using Fundusze.Application.DTOs;
using Fundusze.Application.Mappers;
using Fundusze.Domain;
using Fundusze.Domain.Interfaces;
using Microsoft.Extensions.Logging; // <-- DODANA LINIA

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

            // Sprawdzamy czy aktywo istnieje (dobra praktyka)
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
                    // Dodajemy logikę zapobiegającą ujemnej wartości portfela
                    if (portfolio.NAV < transactionValue)
                    {
                        _logger.LogWarning("Próba sprzedaży aktywów o wartości większej niż NAV portfela.");
                        // Można rzucić wyjątkiem lub po prostu nie pozwolić na operację
                        throw new InvalidOperationException("Cannot sell assets worth more than the current portfolio NAV.");
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