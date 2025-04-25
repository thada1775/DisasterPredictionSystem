using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.DTOs
{
    [ValidateNever]
    public class LocationCoordinatesDto
    {
        public string RegionId { get; set; } = default!;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
