
namespace MuseumExhibits.Core.Abstractions
{
    public interface ITransaction : IAsyncDisposable
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}
