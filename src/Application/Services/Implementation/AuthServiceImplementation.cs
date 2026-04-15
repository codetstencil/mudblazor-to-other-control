public class AuthServiceImplementation(IAuthRepository authRepository) : IAuthService
{
    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var response = await authRepository.LoginAsync(loginDto.Username, loginDto.Password);
       return response.ToDto();
    }

    public async Task LogoutAsync() => await authRepository.LogoutAsync();

    public async Task<bool> IsAuthenticatedAsync() => await authRepository.IsAuthenticatedAsync();

    public Task<string> GetTokenAsync()
    {
        throw new NotImplementedException();
    }
}
