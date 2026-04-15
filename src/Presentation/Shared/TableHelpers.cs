using System.Buffers.Text;
using System.Net.Http;

namespace Presentation.Shared
{
    public static class TableHelpers
    {
        /// <summary>
        /// Generic method to handle server-side data loading for MudTable.
        /// </summary>
        /// <typeparam name="T">The type of data being loaded</typeparam>
        /// <param name="state">Current table state from MudTable</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="searchTerm">Search term to filter results</param>
        /// <param name="getPagedFunc">Function to get paged data</param>
        /// <param name="errorMessage">Error message to display if loading fails</param>
        /// <param name="snackBar">Snackbar service for displaying messages</param>
        /// <param name="isLoadingCallback">Callback to update loading state</param>
        /// <returns>TableData containing the items for the current page</returns>
        public static async Task<TableData<T>> ServerReload<T>(
            TableState state,
            CancellationToken cancellationToken,
            string searchTerm,
            Func<QueryParameters, Task<PaginatedResult<T>>> getPagedFunc,
            string errorMessage,
            ISnackbar snackBar,
            Action<bool> isLoadingCallback)
        {
            try
            {
                isLoadingCallback(true);

                if (cancellationToken.IsCancellationRequested)
                    return new TableData<T>();

                var parameters = new QueryParameters
                {
                    PageNumber = state.Page + 1,
                    PageSize = state.PageSize,
                    SearchTerm = searchTerm,
                    SortColumn = state.SortLabel,
                    IsDescending = state.SortDirection == SortDirection.Descending
                };

                var result = await getPagedFunc(parameters);
                if (result != null)
                {
                    return new TableData<T>
                    {
                        TotalItems = result.TotalCount,
                        Items = result.Items
                    };
                }

                return new TableData<T>
                {
                    TotalItems = 0,
                    Items = []
                };
            }
            catch (Exception ex)
            {
                snackBar.Add($"{errorMessage}: {ex.Message}", Severity.Error);
                return new TableData<T>();
            }
            finally
            {
                isLoadingCallback(false);
            }
        }

        public static string BuildQueryString(QueryParameters parameters)
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
