using DisasterPrediction.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Interfaces
{
    public interface IRegionService
    {
        Task<RegionDto> CreateAsync(RegionDto request);
    }
}
