using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.DTOs
{
    public class AlertHistoryDto
    {
        public long AlertHistoryId { get; set; }
        public string RegionId { get; set; } = default!;
        public string DisasterType { get; set; } = default!;
        public short RiskScore { get; set; }
        public string RiskLevel { get; set; } = default!;
        public DateTime CreateDate { get; set; }
    }
}
