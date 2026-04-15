namespace Application.Services.Implementation
{
    public class AlbumServiceImplementation(
        IAlbumRepository albumRepository,
        IValidator<AlbumDto> validator)
        : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository = albumRepository ?? throw new ArgumentNullException(nameof(albumRepository));
        private readonly IValidator<AlbumDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        //GetAll
        public async Task<IEnumerable<AlbumDto>> GetAllAsync()
        {
            var albums = await _albumRepository.GetAllAsync();
            return albums.ToDtos();
        }

        //GetById
        public async Task<AlbumDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var album = await _albumRepository.GetByIdAsync(id);
            return album?.ToDto();
        }

        //GetPaged
        public async Task<PaginatedResult<AlbumDto>> GetPagedAsync(QueryParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            var result = await _albumRepository.GetPagedAsync(
                parameters.PageNumber,
                parameters.PageSize,
                parameters.SearchTerm,
                parameters.SortColumn,
                parameters.IsDescending);

            return new PaginatedResult<AlbumDto>
            {
                Items = result.Items.ToDtos(),
                TotalCount = result.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        //Create
        public async Task<AlbumDto> CreateAsync(AlbumDto albumDto)
        {
            if (albumDto == null)
            {
                throw new ArgumentNullException(nameof(albumDto));
            }

            var validationResult = await _validator.ValidateAsync(albumDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var album = albumDto.ToEntity();
            var createdAlbum = await _albumRepository.AddAsync(album);
            return createdAlbum.ToDto();
        }

        //Update
        public async Task<AlbumDto> UpdateAsync(AlbumDto albumDto)
        {
            if (albumDto == null)
            {
                throw new ArgumentNullException(nameof(albumDto));
            }

            if (albumDto.AlbumId <= 0)
            {
                throw new ArgumentException("Album Id must be greater than 0", nameof(albumDto.AlbumId));
            }

            var validationResult = await _validator.ValidateAsync(albumDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var existingAlbum = await _albumRepository.GetByIdAsync(albumDto.AlbumId);
            if (existingAlbum == null)
            {
                throw new KeyNotFoundException($"Album with ID {albumDto.AlbumId} not found");
            }

            albumDto.MapTo(existingAlbum);
            var updatedAlbum = await _albumRepository.UpdateAsync(existingAlbum);
            return updatedAlbum.ToDto();        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0", nameof(id));
            }

            var album = await _albumRepository.GetByIdAsync(id);
            if (album == null)
            {
                throw new KeyNotFoundException($"Album with ID {id} not found");
            }

            await _albumRepository.DeleteAsync(id);
        }
    }
}

