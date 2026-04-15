using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IQueryable<IdentityUser> _users;

        public UserRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _users = _userManager.Users;
        }

        public async Task<IEnumerable<IdentityUser>> GetAllAsync()
        {
            return await _users.ToListAsync();
        }

        public async Task<IdentityUser?> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityUser?> GetByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<IdentityUser> AddAsync(IdentityUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
            }
            return user;
        }

        public async Task<IdentityUser> UpdateAsync(IdentityUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to update user: {errors}");
            }
            return user;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id)
                ?? throw new KeyNotFoundException($"User with ID {id} not found");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to delete user: {errors}");
            }
        }

        public async Task<(IEnumerable<IdentityUser> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            string? sortColumn,
            bool isDescending)
        {
            var query = _users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u =>
                    u.UserName.Contains(searchTerm) ||
                    u.Email.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            query = sortColumn?.ToLower() switch
            {
                "username" => isDescending
                    ? query.OrderByDescending(u => u.UserName)
                    : query.OrderBy(u => u.UserName),
                "email" => isDescending
                    ? query.OrderByDescending(u => u.Email)
                    : query.OrderBy(u => u.Email),
                _ => query.OrderBy(u => u.Id)
            };

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
