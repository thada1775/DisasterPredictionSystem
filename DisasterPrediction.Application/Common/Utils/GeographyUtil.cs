using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Common.Utils
{
    public static class GeographyUtil
    {
        public static bool ValidateLatLon(decimal lat, decimal lon)
        {
            if (lat < -90 && lat > 90) return false;
            if (lon < -180 && lon > 180) return false;

            return true;
        }
    }
}
