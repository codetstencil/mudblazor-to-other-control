using System.Net.Http.Headers;

namespace Presentation.Services.HttpClients
{
    public interface IEntity
    {
        int Id { get; }
    }

    public abstract class BaseHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;
        private readonly string _baseUrl;

        protected BaseHttpClient(HttpClient httpClient, IAuthService authService, string baseUrl)
        {
            _httpClient = httpClient;
            _authService = authService;
            _baseUrl = baseUrl;
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            SetAuthorizationHeaderAsync().GetAwaiter();
        }

        private async Task SetAuthorizationHeaderAsync()
        {
            var token = await _authService .GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        protected async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            try
            {
                var response = await _httpClient.GetAsync(_baseUrl);
                response.EnsureSuccessStatusCode();

                var items = await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
                return items ?? [];
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                throw;
            }
        }

        protected async Task<T?> GetByIdAsync<T>(int id) where T : class
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<T>($"{_baseUrl}/{id}");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Failed to retrieve {typeof(T).Name} with ID {id}.", ex);
            }
            catch (JsonException ex)
            {
                throw new ApplicationException($"Failed to parse {typeof(T).Name} data for ID {id}.", ex);
            }
        }

        protected async Task<T> CreateAsync<T>(T entity) where T : class
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_baseUrl, entity);
                response.EnsureSuccessStatusCode();

                var createdEntity = await response.Content.ReadFromJsonAsync<T>();
                if (createdEntity == null)
                {
                    throw new ApplicationException($"Created {typeof(T).Name} response was null");
                }

                return createdEntity;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Failed to create {typeof(T).Name}.", ex);
            }
            catch (JsonException ex)
            {
                throw new ApplicationException($"Failed to parse created {typeof(T).Name} data.", ex);
            }
        }

        protected async Task<T> UpdateAsync<T>(T entity, int id) where T : class
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", entity);
                response.EnsureSuccessStatusCode();

                var updatedEntity = await response.Content.ReadFromJsonAsync<T>();
                if (updatedEntity == null)
                {
                    throw new ApplicationException($"Updated {typeof(T).Name} response was null");
                }

                return updatedEntity;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Failed to update {typeof(T).Name} with ID {id}.", ex);
            }
            catch (JsonException ex)
            {
                throw new ApplicationException($"Failed to parse updated {typeof(T).Name} data for ID {id}.", ex);
            }
        }

        protected async Task DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Failed to delete entity with ID {id}.", ex);
            }
        }

        protected async Task<PaginatedResult<T>> GetPagedAsync<T>(QueryParameters parameters)
        {
            try
            {
                var queryString = BuildQueryString(parameters);
                var response = await _httpClient.GetAsync($"{_baseUrl}/paged{queryString}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<PaginatedResult<T>>();
                return result ?? new PaginatedResult<T>
                {
                    Items = [],
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize,
                    TotalCount = 0
                };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                throw;
            }
        }

/*
        protected async Task<IEnumerable<T>> GetByNameAsync<T>(string name)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/artist/{Uri.EscapeDataString(name)}");
                response.EnsureSuccessStatusCode();

                var items = await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
                return items ?? [];
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                throw;
            }
        }
*/

        protected async Task<IEnumerable<T>> SearchAsync<T>(string searchTerm)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/search?term={Uri.EscapeDataString(searchTerm)}");
                response.EnsureSuccessStatusCode();

                var items = await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
                return items ?? [];
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                throw;
            }
        }

        private static string BuildQueryString(QueryParameters parameters)
        {
            var queryParams = new List<string>
            {
                $"pageNumber={parameters.PageNumber}",
                $"pageSize={parameters.PageSize}"
            };

            if (!string.IsNullOrEmpty(parameters.SearchTerm))
            {
                queryParams.Add($"searchTerm={Uri.EscapeDataString(parameters.SearchTerm)}");
            }

            if (!string.IsNullOrEmpty(parameters.SortColumn))
            {
                queryParams.Add($"sortColumn={parameters.SortColumn}");
                queryParams.Add($"isDescending={parameters.IsDescending}");
            }

            return $"?{string.Join("&", queryParams)}";
        }
    }
}
