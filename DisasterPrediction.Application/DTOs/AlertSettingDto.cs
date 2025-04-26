using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.DTOs
{
    public class AlertSettingDto
    {
        public string RegionId { get; set; } = default!;
        public string DisasterType { get; set; } = default!;
        public short ThresholdScore { get; set; }
    }
}
