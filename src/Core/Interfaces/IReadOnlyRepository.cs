namespace Core.Interfaces
{
    /// <summary>
    /// Read-only repository interface for entities that should only support read operations
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public interface IReadOnlyRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending);
    }

    /// <summary>
    /// Read-only repository interface for view entities that don't have unique IDs
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public interface IReadOnlyViewRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending);
    }
} 
