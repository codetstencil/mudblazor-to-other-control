using Microsoft.AspNetCore.Identity;
using Infrastructure.Authentication;

namespace Infrastructure.Repositories
{
    public class AuthRepository(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        JwtTokenGenerator tokenGenerator)
        : IAuthRepository
    {
        public async Task<bool> IsAuthenticatedAsync()
        {
            return true; // This should be handled by the authentication middleware
        }

        public async Task<AuthResult> LoginAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return new AuthResult { Success = false, Message = "Invalid credentials" };
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
            {
                return new AuthResult { Success = false, Message = "Invalid credentials" };
            }

            return new AuthResult
            {
                Success = true,
                Token = tokenGenerator.GenerateToken(user)
            };
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await userManager.FindByNameAsync(username) != null;
        }
    }
}
