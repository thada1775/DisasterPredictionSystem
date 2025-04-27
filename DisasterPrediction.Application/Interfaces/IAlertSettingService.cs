using DisasterPrediction.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Interfaces
{
    public interface IAlertSettingService
    {
        Task<AlertSettingDto> CreateSettingAsync(AlertSettingDto request);
        Task<AlertSettingDto> UpdateSettingAsync(AlertSettingDto request);
        Task DeleteSettingAsync(string id);
        Task<AlertSettingDto> GetSettingAsync(string id);
        Task<List<AlertSettingDto>> FindSettingAsync();
    }
}
