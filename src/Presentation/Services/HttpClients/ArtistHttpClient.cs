namespace Presentation.Services.HttpClients
{
    public class ArtistHttpClient(HttpClient httpClient, IAuthService authService)
        : BaseHttpClient(httpClient, authService, "api/artists"), IArtistService
    {
        public async Task<IEnumerable<ArtistDto>> GetAllAsync() => 
            await base.GetAllAsync<ArtistDto>();

        public async Task<ArtistDto?> GetByIdAsync(int id) => 
            await base.GetByIdAsync<ArtistDto>(id);

        public async Task<PaginatedResult<ArtistDto>> GetPagedAsync(QueryParameters parameters) => 
            await base.GetPagedAsync<ArtistDto>(parameters);

        public async Task<ArtistDto> CreateAsync(ArtistDto artistDto) => 
            await base.CreateAsync(artistDto);

        public async Task<ArtistDto> UpdateAsync(ArtistDto artistDto) => 
            await base.UpdateAsync(artistDto, artistDto.ArtistId);

        public new async Task DeleteAsync(int id)
        {
            await base.DeleteAsync(id);
        }
    }
}
