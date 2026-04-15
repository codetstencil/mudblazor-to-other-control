namespace Application.Services
{
    /// <summary>
    /// Read-only base service interface for entities that should only support read operations
    /// </summary>
    /// <typeparam name="TDto">The DTO type</typeparam>
    public interface IReadOnlyBaseService<TDto>
    {
        Task<IEnumerable<TDto>> GetAllAsync();

        Task<TDto?> GetByIdAsync(int id);

        Task<PaginatedResult<TDto>> GetPagedAsync(QueryParameters parameters);
    }
} 
