using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Domain.Entities
{
    public class LocationCoordinates
    {
        public long LocationCoordinatesId { get; set; }
        public string RegionId { get; set; } = default!;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
