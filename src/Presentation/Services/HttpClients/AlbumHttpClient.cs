namespace Presentation.Services.HttpClients
{
    public class AlbumHttpClient(HttpClient httpClient, IAuthService authService)
        : BaseHttpClient(httpClient, authService, "api/albums"), IAlbumService
    {
        public async Task<IEnumerable<AlbumDto>> GetAllAsync() => 
            await base.GetAllAsync<AlbumDto>();

        public async Task<AlbumDto?> GetByIdAsync(int id) => 
            await base.GetByIdAsync<AlbumDto>(id);

        public async Task<PaginatedResult<AlbumDto>> GetPagedAsync(QueryParameters parameters) => 
            await base.GetPagedAsync<AlbumDto>(parameters);

        public async Task<AlbumDto> CreateAsync(AlbumDto albumDto) => 
            await base.CreateAsync(albumDto);

        public async Task<AlbumDto> UpdateAsync(AlbumDto albumDto) => 
            await base.UpdateAsync(albumDto, albumDto.AlbumId);

        public new async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id);
        }
    }
}
