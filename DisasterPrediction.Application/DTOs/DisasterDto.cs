using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.DTOs
{
    public class DisasterDto
    {
        public string RegionId { get; set; } = default!;
        public string DisasterType { get; set; } = default!;
        public short RiskScore { get; set; }
        public string? RiskLevel { get; set; }
        public bool AlertTriggered { get; set; } = false;
    }
}
