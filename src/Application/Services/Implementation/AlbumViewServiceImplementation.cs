namespace Application.Services.Implementation
{
    public class AlbumViewServiceImplementation(IAlbumViewRepository albumViewRepository)  : IAlbumViewService
    {
        private readonly IAlbumViewRepository _albumViewRepository = albumViewRepository ?? throw new ArgumentNullException(nameof(albumViewRepository));

        //GetAll
        public async Task<IEnumerable<AlbumViewDto>> GetAllAsync()
        {
            var albumViews = await _albumViewRepository.GetAllAsync();
            return albumViews.ToDtos();
        }

        //GetPaged
        public async Task<PaginatedResult<AlbumViewDto>> GetPagedAsync(QueryParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var result = await _albumViewRepository.GetPagedAsync(
                parameters.PageNumber,
                parameters.PageSize,
                parameters.SearchTerm,
                parameters.SortColumn,
                parameters.IsDescending);

            return new PaginatedResult<AlbumViewDto>
            {
                Items = result.Items.ToDtos(),
                TotalCount = result.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        //Search
        public async Task<IEnumerable<AlbumViewDto>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new ArgumentException("Search term cannot be null or empty", nameof(searchTerm));
            }

            var albumViews = await _albumViewRepository.SearchAsync(searchTerm);
            return albumViews.ToDtos();
        }
    }
}
