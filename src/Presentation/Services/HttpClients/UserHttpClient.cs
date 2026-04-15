namespace Presentation.Services.HttpClients
{
    public class UserHttpClient : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;
        private const string BaseUrl = "api/users";

        public UserHttpClient(HttpClient httpClient,
            IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
            SetAuthorizationHeaderAsync().GetAwaiter();
        }

        private async Task SetAuthorizationHeaderAsync()
        {
            var token = await _authService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserDto>>();
            return users ?? [];
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{Uri.EscapeDataString(id)}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task<PaginatedResult<UserDto>> GetPagedAsync(QueryParameters parameters)
        {
            await SetAuthorizationHeaderAsync();
            var queryString = $"?pageNumber={parameters.PageNumber}&pageSize={parameters.PageSize}";

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
                queryString += $"&searchTerm={Uri.EscapeDataString(parameters.SearchTerm)}";

            if (!string.IsNullOrEmpty(parameters.SortColumn))
                queryString += $"&sortColumn={Uri.EscapeDataString(parameters.SortColumn)}&isDescending={parameters.IsDescending}";

            var response = await _httpClient.GetAsync($"{BaseUrl}/paged{queryString}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PaginatedResult<UserDto>>();
            return result ?? new PaginatedResult<UserDto>
            {
                Items = [],
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalCount = 0
            };
        }

        public async Task<UserDto> CreateAsync(UserDto userDto)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, userDto);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Failed to create user. Status: {response.StatusCode}, Error: {errorContent}");
            }

            var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();
            return createdUser ?? throw new InvalidOperationException("Failed to create user");
        }

        public async Task<UserDto> UpdateAsync(UserDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.Id))
                throw new ArgumentException("User ID cannot be empty", nameof(userDto));

            var response = await _httpClient.PutAsJsonAsync(
                $"{BaseUrl}/{Uri.EscapeDataString(userDto.Id)}",
                userDto);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Failed to update user. Status: {response.StatusCode}, Error: {errorContent}");
            }

            var updatedUser = await response.Content.ReadFromJsonAsync<UserDto>();
            return updatedUser ?? throw new InvalidOperationException("Failed to update user");
        }

        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("ID cannot be empty", nameof(id));

            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{Uri.EscapeDataString(id)}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Failed to delete user. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }

        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            var response = await _httpClient.GetAsync($"{BaseUrl}/username/{Uri.EscapeDataString(username)}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }
    }
}
