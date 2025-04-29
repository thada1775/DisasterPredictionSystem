using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Common.Interfaces
{
    public interface IApiService
    {
        Task<string> SendRequestAsync(string url, string? apiKey, HttpMethod method, object? requestBody = null, Dictionary<string, string> queryParams = null);
    }
}
