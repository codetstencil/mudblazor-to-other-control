namespace Application.Services
{
    public abstract class BaseHttpService(HttpClient httpClient, string baseUrl)
    {
        protected readonly HttpClient HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        protected readonly string BaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));

        protected async Task<IEnumerable<TDto>> GetAllAsync<TDto>()
        {
            try
            {
                var items = await HttpClient.GetFromJsonAsync<IEnumerable<TDto>>(BaseUrl);
                return items ?? [];
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Failed to retrieve {typeof(TDto).Name}s.", ex);
            }
        }

        protected async Task<TDto?> GetByIdAsync<TDto>(int id)
        {
            try
            {
                return await HttpClient.GetFromJsonAsync<TDto>($"{BaseUrl}/{id}");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Failed to retrieve {typeof(TDto).Name} with ID {id}.", ex);
            }
        }

        protected async Task<PaginatedResult<TDto>> GetPagedAsync<TDto>(QueryParameters parameters)
        {
            try
            {
                var queryString = BuildQueryString(parameters);
                var response = await HttpClient.GetAsync($"{BaseUrl}/paged{queryString}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<PaginatedResult<TDto>>();
                return result ?? new PaginatedResult<TDto>(
                    [],
                    0,
                    parameters.PageNumber,
                    parameters.PageSize
                );
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Failed to retrieve paged {typeof(TDto).Name}s.", ex);
            }
        }

        protected async Task<TDto> CreateAsync<TDto>(TDto dto)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync(BaseUrl, dto);
                response.EnsureSuccessStatusCode();
                var created = await response.Content.ReadFromJsonAsync<TDto>();

                if (created == null)
                {
                    throw new ApplicationException($"Created {typeof(TDto).Name} response was null");
                }

                return created;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Failed to create {typeof(TDto).Name}.", ex);
            }
        }

        protected async Task<TDto> UpdateAsync<TDto>(int id, TDto dto)
        {
            try
            {
                var response = await HttpClient.PutAsJsonAsync($"{BaseUrl}/{id}", dto);
                response.EnsureSuccessStatusCode();
                var updated = await response.Content.ReadFromJsonAsync<TDto>();

                if (updated == null)
                {
                    throw new ApplicationException($"Updated {typeof(TDto).Name} response was null");
                }

                return updated;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Failed to update {typeof(TDto).Name} with ID {id}.", ex);
            }
        }

        protected async Task DeleteAsync(int id)
        {
            try
            {
                var response = await HttpClient.DeleteAsync($"{BaseUrl}/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Failed to delete item with ID {id}.", ex);
            }
        }

        protected static string BuildQueryString(QueryParameters parameters)
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
                queryParams.Add($"sortColumn={Uri.EscapeDataString(parameters.SortColumn)}");
                queryParams.Add($"isDescending={parameters.IsDescending}");
            }

            return $"?{string.Join("&", queryParams)}";
        }
    }
}
