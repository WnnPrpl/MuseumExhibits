using Microsoft.EntityFrameworkCore.Storage;
using MuseumExhibits.Core.Abstractions;


namespace MuseumExhibits.Infrastructure.Data
{
    public class EfCoreTransaction : ITransaction
    {
        private readonly IDbContextTransaction _transaction;

        public EfCoreTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public Task CommitAsync() => _transaction.CommitAsync();
        public Task RollbackAsync() => _transaction.RollbackAsync();
        public async ValueTask DisposeAsync() => await _transaction.DisposeAsync();
    }
}
