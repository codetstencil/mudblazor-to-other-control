namespace Application.Services
{
    public interface IAlbumViewService : IReadOnlyViewBaseService<AlbumViewDto>
    {
        Task<IEnumerable<AlbumViewDto>> SearchAsync(string searchTerm);
    }
}
