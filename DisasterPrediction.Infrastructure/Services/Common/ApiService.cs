using DisasterPrediction.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DisasterPrediction.Infrastructure.Services.Common
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> SendRequestAsync(string url, string? apiKey, HttpMethod method, object? requestBody = null, Dictionary<string, string> queryParams = null)
        {
            try
            {
                if (queryParams != null && queryParams.Count > 0)
                {
                    url = AddQueryString(url, queryParams);
                }

                using var request = new HttpRequestMessage(method, url);
                request.Headers.Add("Authorization", $"Bearer {apiKey}");
                request.Headers.Add("Accept", "application/json");
                if (!string.IsNullOrWhiteSpace(apiKey))
                    request.Headers.Add("apikey", apiKey);

                if (requestBody != null && (method == HttpMethod.Post || method == HttpMethod.Put))
                {
                    string jsonContent = JsonSerializer.Serialize(requestBody);
                    request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                }

                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private string AddQueryString(string url, Dictionary<string, string> queryParams)
        {
            var queryString = new StringBuilder();
            foreach (var param in queryParams)
            {
                if (queryString.Length > 0)
                    queryString.Append('&');
                queryString.Append($"{Uri.EscapeDataString(param.Key)}={Uri.EscapeDataString(param.Value)}");
            }
            return url.Contains("?") ? $"{url}&{queryString}" : $"{url}?{queryString}";
        }
    }
}
