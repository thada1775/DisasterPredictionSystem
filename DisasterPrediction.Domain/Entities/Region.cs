using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Domain.Entities
{
    public class Region
    {
        public string RegionId { get; set; } = default!;
        public string DisasterTypes { get; set; } = default!;
        public LocationCoordinates LocationCoordinates { get; set; } = default!;
        public List<AlertSetting> AlertSettings { get; set; } = new List<AlertSetting>();


    }
}
