namespace Application.Services
{
    public interface IBaseService<TDto>
    {
        Task<IEnumerable<TDto>> GetAllAsync();

        Task<TDto?> GetByIdAsync(int id);

        Task<TDto> CreateAsync(TDto dto);

        Task<TDto> UpdateAsync(TDto dto);

        Task DeleteAsync(int id);

        Task<PaginatedResult<TDto>> GetPagedAsync(QueryParameters parameters);
    }
}
