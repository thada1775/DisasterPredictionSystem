using DisasterPrediction.Application.DTOs.Common;
using DisasterPrediction.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Interfaces
{
    public interface IAlertsService
    {
        Task<SearchResult<AlertHistoryDto>> FindSummaryHistoryAsync(AlertHistoryFilterDto filter);
        Task<List<AlertHistoryDto>> GetHistoryByRegionAsync(string id);
        Task SendWarningMessage();
    }
}
