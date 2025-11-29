using CoreBanking.Core.Interfaces;

namespace CoreBanking.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly BankingDbContext _context;

    public UnitOfWork(BankingDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
         await _context.SaveChangesWithOutboxAsync(cancellationToken);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}