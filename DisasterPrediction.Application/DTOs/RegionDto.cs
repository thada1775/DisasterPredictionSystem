using DisasterPrediction.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.DTOs
{
    public class RegionDto
    {
        public string RegionId { get; set; } = default!;
        public LocationCoordinatesDto LocationCoordinates { get; set; } = default!;
        public List<string> DisasterTypes { get; set; } = new List<string>();
        [NotMapped]
        public string? DisasaterTypesString { get; set; }
    }
}
