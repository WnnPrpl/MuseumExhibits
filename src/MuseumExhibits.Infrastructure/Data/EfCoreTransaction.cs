using Microsoft.EntityFrameworkCore.Storage;
using MuseumExhibits.Core.Abstractions;

namespace MuseumExhibits.Infrastructure.Data
{
    public class EfCoreTransaction(IDbContextTransaction transaction) : ITransaction
    {
        public Task CommitAsync() => transaction.CommitAsync();
        public Task RollbackAsync() => transaction.RollbackAsync();
        public async ValueTask DisposeAsync() => await transaction.DisposeAsync();
    }
}
