namespace Application.Services
{
    public class AlbumService(HttpClient httpClient) : BaseHttpService(httpClient, ApiBaseUrl), IAlbumService
    {
        private const string ApiBaseUrl = "api/Albums";

        public Task<IEnumerable<AlbumDto>> GetAllAsync()
            => base.GetAllAsync<AlbumDto>();

        public Task<AlbumDto?> GetByIdAsync(int id)
            => base.GetByIdAsync<AlbumDto>(id);

        public Task<PaginatedResult<AlbumDto>> GetPagedAsync(QueryParameters parameters)
            => base.GetPagedAsync<AlbumDto>(parameters);

        public Task<AlbumDto> CreateAsync(AlbumDto albumDto)
            => base.CreateAsync(albumDto);

        public Task<AlbumDto> UpdateAsync(AlbumDto albumDto)
            => base.UpdateAsync(albumDto.AlbumId, albumDto);

        public Task DeleteAsync(int id)
            => base.DeleteAsync(id);
    }
}
