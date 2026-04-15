namespace Application.Services
{
    /// <summary>
    /// Read-only base service interface for view entities that don't have unique IDs
    /// </summary>
    /// <typeparam name="TDto">The DTO type</typeparam>
    public interface IReadOnlyViewBaseService<TDto>
    {
        Task<IEnumerable<TDto>> GetAllAsync();

        Task<PaginatedResult<TDto>> GetPagedAsync(QueryParameters parameters);
    }
} 
