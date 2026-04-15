namespace Application.Services
{
    public class AuthService(
        HttpClient httpClient,
        AuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorageService)
        : IAuthService
    {
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginModel)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/auth/login", loginModel);
                if (response.IsSuccessStatusCode)
                {
                    var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
                    if (authResponse?.IsAuthenticated == true)
                    {
                        // Store token in local storage
                        await localStorageService.SetItemAsync("authToken", authResponse.Token);
                        // Update authentication state
                        await authStateProvider.GetAuthenticationStateAsync();
                        return authResponse;
                    }
                }
                return new AuthResponseDto { IsAuthenticated = false };
            }
            catch (Exception ex)
            {
                // Handle exception and return appropriate response
                return new AuthResponseDto { IsAuthenticated = false, Message = ex.Message };
            }
        }

        public async Task LogoutAsync()
        {
            // Remove token from local storage
            await localStorageService.RemoveItemAsync("authToken");
            // Notify authentication state changed
            await authStateProvider.GetAuthenticationStateAsync();
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();
            return authState.User.Identity is { IsAuthenticated: true };
        }

        public async Task<string> GetTokenAsync()
        {
            return await localStorageService.GetItemAsync<string>("authToken");
        }
    }
}
