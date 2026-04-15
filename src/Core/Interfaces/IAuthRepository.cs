namespace Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<AuthResult> LoginAsync(string username, string password);
        Task LogoutAsync();
        Task<bool> IsAuthenticatedAsync();
        Task<bool> UserExistsAsync(string username);
    }
}
