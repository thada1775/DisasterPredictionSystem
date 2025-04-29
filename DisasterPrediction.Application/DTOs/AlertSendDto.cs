using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.DTOs
{
    public class AlertSendDto
    {
        public string RegionId { get; set; } = default!;
        public string DisasterType { get; set; } = default!;
        public short RiskScore { get; set; }
        public string AlertMessage { get; set; } = default!;
        public DateTime Timestamp { get; set; }
    }
}
