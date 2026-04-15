namespace Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(int id);

        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending);
    }
}
