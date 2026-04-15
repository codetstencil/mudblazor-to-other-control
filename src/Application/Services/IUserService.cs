namespace Application.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();

        Task<UserDto?> GetByIdAsync(string id);

        Task<UserDto?> GetByUsernameAsync(string username);

        Task<PaginatedResult<UserDto>> GetPagedAsync(QueryParameters parameters);

        Task<UserDto> CreateAsync(UserDto userDto);

        Task<UserDto> UpdateAsync(UserDto userDto);

        Task DeleteAsync(string id);
    }
}
