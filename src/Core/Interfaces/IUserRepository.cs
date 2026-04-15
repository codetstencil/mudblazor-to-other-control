using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAllAsync();

        Task<IdentityUser?> GetByIdAsync(string id);

        Task<IdentityUser?> GetByUsernameAsync(string username);

        Task<IdentityUser> AddAsync(IdentityUser user, string password);

        Task<IdentityUser> UpdateAsync(IdentityUser user);

        Task DeleteAsync(string id);

        Task<(IEnumerable<IdentityUser> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending);
    }
}
