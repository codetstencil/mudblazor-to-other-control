namespace Infrastructure.Repositories
{
    public class AlbumViewRepository(ChinookManagerContext context) : IAlbumViewRepository
    {

        //GetAll
        public async Task<IEnumerable<AlbumView>> GetAllAsync()
        {
            return await context.AlbumViews.ToListAsync();
        }

        //Search
        public async Task<IEnumerable<AlbumView>> SearchAsync(string searchTerm)
        {
            return await context.AlbumViews
                .Where(av =>
                    av.Title.Contains(searchTerm)
                )
                .ToListAsync();
        }


        //GetPaged
        // Debug: RepositoryGetPaged.GenerateCode()
        public async Task<(IEnumerable<AlbumView> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending)
        {
            var query = context.AlbumViews
                .AsQueryable();

            // Apply Search Filtering
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a =>
                    a.Title.Contains(searchTerm)
                    );
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = sortColumn?.ToLower() switch
            {
                "title" => isDescending
                    ? query.OrderByDescending(a => a.Title)
                    : query.OrderBy(a => a.Title),
                _ => query.OrderBy(a => a.Title)  // Default sorting
            };

            // Apply pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

    }
}
