namespace Application.Services
{
    public class ArtistService(HttpClient httpClient) : BaseHttpService(httpClient, ApiBaseUrl), IArtistService
    {
        private const string ApiBaseUrl = "api/Artists";

        public Task<IEnumerable<ArtistDto>> GetAllAsync()
            => base.GetAllAsync<ArtistDto>();

        public Task<ArtistDto?> GetByIdAsync(int id)
            => base.GetByIdAsync<ArtistDto>(id);

        public Task<PaginatedResult<ArtistDto>> GetPagedAsync(QueryParameters parameters)
            => base.GetPagedAsync<ArtistDto>(parameters);

        public Task<ArtistDto> CreateAsync(ArtistDto artistDto)
            => base.CreateAsync(artistDto);

        public Task<ArtistDto> UpdateAsync(ArtistDto artistDto)
            => base.UpdateAsync(artistDto.ArtistId, artistDto);

        public Task DeleteAsync(int id)
            => base.DeleteAsync(id);
    }
}
