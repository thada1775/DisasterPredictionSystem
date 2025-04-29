using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.DTOs.Earthquake
{
    public class EarthquakeApi
    {
        public string? type { get; set; }
        public Metadata metadata { get; set; } = default!;
        public List<Feature> features { get; set; } = new List<Feature>();
        public List<double> bbox { get; set; } = new List<double>();
    }

    public class Feature
    {
        public string? type { get; set; }
        public Properties properties { get; set; } = default!;
        public Geometry geometry { get; set; } = default!;
        public string? id { get; set; }
    }

    public class Geometry
    {
        public string? type { get; set; }
        public List<double> coordinates { get; set; } = new List<double>();
    }

    public class Metadata
    {
        public long generated { get; set; }
        public string? url { get; set; }
        public string? title { get; set; }
        public int status { get; set; }
        public string? api { get; set; }
        public int count { get; set; }
    }

    public class Properties
    {
        public double mag { get; set; }
        public string? place { get; set; }
        public long time { get; set; }
        public object? updated { get; set; }
        public object? tz { get; set; }
        public string? url { get; set; }
        public string? detail { get; set; }
        public int? felt { get; set; }
        public double? cdi { get; set; }
        public object? mmi { get; set; }
        public object? alert { get; set; }
        public string? status { get; set; }
        public int tsunami { get; set; }
        public int sig { get; set; }
        public string? net { get; set; }
        public string? code { get; set; }
        public string? ids { get; set; }
        public string? sources { get; set; }
        public string? types { get; set; }
        public int? nst { get; set; }
        public double? dmin { get; set; }
        public double rms { get; set; }
        public double? gap { get; set; }
        public string? magType { get; set; }
        public string? type { get; set; }
        public string? title { get; set; }
    }
}
