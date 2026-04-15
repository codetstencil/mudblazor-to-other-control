namespace Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);

        Task LogoutAsync();

        Task<bool> IsAuthenticatedAsync();

        Task<string> GetTokenAsync();
    }
}
