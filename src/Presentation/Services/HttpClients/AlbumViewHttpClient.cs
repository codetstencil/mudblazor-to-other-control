namespace Presentation.Services.HttpClients
{
    public class AlbumViewHttpClient(HttpClient httpClient, IAuthService authService)
        : BaseHttpClient(httpClient, authService, "api/albumviews"), IAlbumViewService
    {
        public async Task<IEnumerable<AlbumViewDto>> GetAllAsync() => 
            await base.GetAllAsync<AlbumViewDto>();

        public async Task<PaginatedResult<AlbumViewDto>> GetPagedAsync(QueryParameters parameters) => 
            await base.GetPagedAsync<AlbumViewDto>(parameters);

        public async Task<IEnumerable<AlbumViewDto>> SearchAsync(string searchTerm) => 
            await base.SearchAsync<AlbumViewDto>(searchTerm);
    }
}
