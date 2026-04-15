namespace Core.Interfaces
{
    /// <summary>
    /// Repository interface for AlbumView with read-only operations
    /// </summary>
    public interface IAlbumViewRepository : IReadOnlyViewRepository<AlbumView>
    {
        Task<IEnumerable<AlbumView>> SearchAsync(string searchTerm);
    }
}
