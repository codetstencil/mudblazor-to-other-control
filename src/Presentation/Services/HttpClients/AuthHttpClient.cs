namespace Presentation.Services.HttpClients
{
    public class AuthHttpClient(HttpClient httpClient, ILocalStorageService localStorageService)
        : IAuthService
    {
        private const string BaseUrl = "api/auth";

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{BaseUrl}/login", loginDto);
                response.EnsureSuccessStatusCode();
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
                await localStorageService.SetItemAsync("authToken", authResponse.Token);
                return authResponse;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return new AuthResponseDto
                {
                    IsAuthenticated = false,
                    Message = $"Login failed: {ex.Message}"
                };
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                return new AuthResponseDto
                {
                    IsAuthenticated = false,
                    Message = $"Error processing response: {ex.Message}"
                };
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                await localStorageService.RemoveItemAsync("authToken");
                await httpClient.PostAsync($"{BaseUrl}/logout", null);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Logout Error: {ex.Message}");
            }
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await localStorageService.GetItemAsync<string>("authToken");
            return !string.IsNullOrEmpty(token);
        }

        public async Task<string> GetTokenAsync()
        {
            return await localStorageService.GetItemAsync<string>("authToken");
        }
    }
}
