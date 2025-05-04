using DisasterPrediction.Application.Common.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.DTOs
{
    public class AlertSettingFilterDto : BaseFilterDto
    {
        public string RegionId { get; set; } = default!;
    }
}
