using DisasterPrediction.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Interfaces
{
    public interface IAlertService
    {
        Task<RegionDto> CreateEntityAsync(RegionDto request);
        Task<RegionDto> UpdateEntityAsync(RegionDto request);
        Task DeleteEntityAsync(string id);
        Task<RegionDto> GetEntityAsync(string id);
        Task<List<RegionDto>> FindEntityuAsync();
    }
}
