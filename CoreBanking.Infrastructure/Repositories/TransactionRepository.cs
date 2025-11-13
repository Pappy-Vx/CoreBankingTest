using CoreBanking.Core.Entities;
using CoreBanking.Core.Enums;
using CoreBanking.Core.Interfaces;
using CoreBanking.Core.ValueObjects;
using CoreBanking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoreBanking.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly BankingDbContext _context;
    private readonly ILogger<TransactionRepository> _logger;


    public TransactionRepository(BankingDbContext context, ILogger<TransactionRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IUnitOfWork UnitOfWork => (IUnitOfWork)_context;
    //public IUnitOfWork UnitOfWork => _context;
    public async Task<Transaction> GetByIdAsync(TransactionId id, CancellationToken cancellationToken = default)
        {
            return await _context.Transactions
                .Include(t => t.Account)
                .ThenInclude(a => a.Customer)
                .FirstOrDefaultAsync(t => t.TransactionId == id, cancellationToken);
        }

    public async Task<List<Transaction>> GetTransactionsByAccountIdAsync(AccountId accountId, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Include(t => t.Account)
            .ThenInclude(a => a.Customer)
            .Where(t => t.Account.AccountId == accountId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAndDateRangeAsync(AccountId accountId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Where(t => t.AccountId == accountId &&
                       t.Timestamp >= startDate &&
                       t.Timestamp <= endDate)
            .OrderBy(t => t.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Transaction entity, CancellationToken cancellationToken = default)
    {
        await _context.Transactions.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Transaction> entities, CancellationToken cancellationToken = default)
    {
        await _context.Transactions.AddRangeAsync(entities, cancellationToken);
        _logger.LogDebug("Added {Count} transactions to the context", entities.Count());
    }

    public async Task UpdateAsync(Transaction entity, CancellationToken cancellationToken = default)
    {
        _context.Transactions.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Transaction entity, CancellationToken cancellationToken = default)
    {
        _context.Transactions.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }


    // NEW METHODS IMPLEMENTATION

    public async Task<List<Transaction>> GetTransactionsBeforeAsync(DateTime cutoffDate, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Include(t => t.Account)
            .ThenInclude(a => a.Customer)
            .Where(t => t.TransactionDate < cutoffDate &&
                    !t.IsArchived) // Only non-archived transactions
            .OrderBy(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Transaction>> GetRecentTransactionsByAccountAsync(AccountId accountId, DateTime sinceDate, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Include(t => t.Account)
            .ThenInclude(a => a.Customer)
            .Where(t => t.Account.AccountId == accountId &&
                    t.TransactionDate >= sinceDate &&
                    !t.IsArchived)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Include(t => t.Account)
            .ThenInclude(a => a.Customer)
            .Where(t => t.TransactionDate >= startDate &&
                    t.TransactionDate <= endDate &&
                    !t.IsArchived)
            .OrderBy(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Transaction>> GetTransactionsByAccountAndDateRangeAsync(AccountId accountId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Include(t => t.Account)
            .ThenInclude(a => a.Customer)
            .Where(t => t.Account.AccountId == accountId &&
                    t.TransactionDate >= startDate &&
                    t.TransactionDate <= endDate &&
                    !t.IsArchived)
            .OrderBy(t => t.TransactionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalTransactionsAmountByDateAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var startOfDay = date.Date;
        var endOfDay = date.Date.AddDays(1).AddTicks(-1);

        return await _context.Transactions
            .Where(t => t.TransactionDate >= startOfDay &&
                    t.TransactionDate <= endOfDay &&
                    t.Type == TransactionType.Deposit) // Or whatever type you need
            .SumAsync(t => t.Amount.Amount, cancellationToken);
    }

}