using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Domain.Entities
{
    public class AlertSetting
    {
        public long AlertSettingId { get; set; }
        public string RegionId { get; set; } = default!;
        public string DisasterType { get; set; } = default!;
        public short ThresholdScore { get; set; }
    }
}
