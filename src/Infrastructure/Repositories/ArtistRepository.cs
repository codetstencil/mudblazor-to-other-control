namespace Infrastructure.Repositories
{
    public class ArtistRepository(ChinookManagerContext context) : IArtistRepository
    {

        //GetAll
        // Debug: RepositoryGetAll.GenerateCode()
        public async Task<IEnumerable<Artist>> GetAllAsync()
        {
            return await context.Artists
                .ToListAsync();
        }

        //GetById
        public async Task<Artist?> GetByIdAsync(int id)
        {
            return await context.Artists
                .FirstOrDefaultAsync(a => a.ArtistId == id);
        }

        //GetPaged
        // Debug: RepositoryGetPaged.GenerateCode()
        public async Task<(IEnumerable<Artist> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending)
        {
            var query = context.Artists
                .AsQueryable();

            // Apply Search Filtering
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a =>
                    a.Name.Contains(searchTerm)
                    );
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = sortColumn?.ToLower() switch
            {
                "name" => isDescending
                    ? query.OrderByDescending(a => a.Name)
                    : query.OrderBy(a => a.Name),
                _ => query.OrderBy(a => a.ArtistId)  // Default sorting
            };

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        //Create
        public async Task<Artist> AddAsync(Artist artist)
        {
            context.Artists.Add(artist);
            await context.SaveChangesAsync();
            return artist;
        }

        //Update
        public async Task<Artist> UpdateAsync(Artist artist)
        {
            try
            {
                var existingArtist = await context.Artists
                    .FirstOrDefaultAsync(a => a.ArtistId == artist.ArtistId);

                if (existingArtist == null)
                {
                    throw new Exception($"Artist with ID {artist.ArtistId} not found");
                }

                // Update the existing entity's properties
                context.Entry(existingArtist).CurrentValues.SetValues(artist);

                // Save changes
                await context.SaveChangesAsync();

                return existingArtist;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflict
                throw new Exception("The artist has been modified by another user. Please refresh and try again.", ex);
            }
        }

        //Delete
        public async Task DeleteAsync(int id)
        {
            var artist = await context.Artists.FindAsync(id);
            if (artist != null)
            {
                context.Artists.Remove(artist);
                await context.SaveChangesAsync();
            }
        }

    }
}
