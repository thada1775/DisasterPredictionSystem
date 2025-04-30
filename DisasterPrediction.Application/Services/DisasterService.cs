using AutoMapper;
using DisasterPrediction.Application.Common.BaseClass;
using DisasterPrediction.Application.Common.Exceptions;
using DisasterPrediction.Application.Common.Interfaces;
using DisasterPrediction.Application.Common.Utils;
using DisasterPrediction.Application.DTOs;
using DisasterPrediction.Application.DTOs.Earthquake;
using DisasterPrediction.Application.DTOs.Weather;
using DisasterPrediction.Application.Interfaces;
using DisasterPrediction.Domain.Entities;
using DisasterPrediction.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Services
{
    public class DisasterService : BaseService, IDisasterService
    {
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;
        public DisasterService(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper, IApiService apiService
            , IConfiguration configuration) : base(context, currentUserService, mapper)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        public async Task<List<DisasterDto>> GetDisasterRisk()
        {
            var allRegion = Context.Regions.Include(x => x.LocationCoordinates).Include(x => x.AlertSetting).ToList();
            if (ListUtil.IsEmptyList(allRegion))
                throw new ValidationException("Region not found.");

            var currentDateTime = DateTime.UtcNow;
            var result = new List<DisasterDto>();

            var alertHistories = await Context.AlertHistories.Where(x => (currentDateTime - x.CreateDate).Minutes < 180).ToListAsync();

            foreach (var region in allRegion)
            {
                if (string.IsNullOrWhiteSpace(_configuration["WeatherApiKey"]))
                    throw new ValidationException("ApiKey config missing.");

                Dictionary<string, string> queryStrings = new Dictionary<string, string>()
                {
                    {"key",_configuration["WeatherApiKey"]! },
                    {"q",$"{region.LocationCoordinates?.Latitude},{region.LocationCoordinates?.Longitude}" }
                };

                var responseString = await _apiService.SendRequestAsync("http://api.weatherapi.com/v1/current.json", null, HttpMethod.Get, null, queryStrings);
                var currentWeather = JsonSerializer.Deserialize<WeatherApi>(responseString);

                if (currentWeather == null)
                    throw new ValidationException("Weather result not found");


                if (region.DisasterTypes.ToLower().Contains(SystemConstant.Disaster.Flood.ToLower()))
                {
                    var disaster = CalculateFloodRisk(region, alertHistories, currentWeather);
                    if (region.AlertSetting.ThresholdScore <= disaster.RiskScore)
                        result.Add(disaster);
                }
                if (region.DisasterTypes.ToLower().Contains(SystemConstant.Disaster.Wildfire.ToLower()))
                {
                    var disaster = CalculateWildfireRiks(region, alertHistories, currentWeather);
                    if (region.AlertSetting.ThresholdScore <= disaster.RiskScore)
                        result.Add(disaster);
                }
                if (region.DisasterTypes.ToLower().Contains(SystemConstant.Disaster.Earthquake.ToLower()))
                {
                    var disaster = await CalculateEarthquake(region, alertHistories, currentDateTime);
                    if (disaster != null && (region.AlertSetting.ThresholdScore <= disaster.RiskScore))
                        result.Add(disaster);
                }
            }

            return result;
        }

        #region Private Method
        private DisasterDto CalculateFloodRisk(Region region, List<AlertHistory> alertHistories, WeatherApi currentWeather)
        {
            DisasterDto warning = new DisasterDto()
            {
                RegionId = region.RegionId,
                DisasterType = SystemConstant.Disaster.Flood
            };
            if (currentWeather.current.precip_mm < 20)
            {
                warning.RiskScore = 20;
                warning.RiskLevel = SystemConstant.RiksLevel.Low;
            }
            else if (currentWeather.current.precip_mm >= 20 && currentWeather.current.precip_mm <= 49)
            {
                warning.RiskScore = 60;
                warning.RiskLevel = SystemConstant.RiksLevel.Medium;
            }
            else
            {
                warning.RiskScore = 85;
                warning.RiskLevel = SystemConstant.RiksLevel.High;
            }

            if (alertHistories.Any(x => x.RegionId == region.RegionId && x.DisasterType.ToLower().Contains(SystemConstant.Disaster.Flood.ToLower())))
                warning.AlertTriggered = true;

            return warning;
        }

        private DisasterDto CalculateWildfireRiks(Region region, List<AlertHistory> alertHistories, WeatherApi currentWeather)
        {
            DisasterDto warning = new DisasterDto()
            {
                RegionId = region.RegionId,
                DisasterType = SystemConstant.Disaster.Flood,
                RiskScore = (short)((currentWeather.current.temp_c * 2) - currentWeather.current.humidity)
            };

            if (warning.RiskScore <= 30)
                warning.RiskLevel = SystemConstant.RiksLevel.Low;
            else if (warning.RiskScore >= 31 && warning.RiskScore <= 70)
                warning.RiskLevel = SystemConstant.RiksLevel.Medium;
            else
                warning.RiskLevel = SystemConstant.RiksLevel.High;

            if (alertHistories.Any(x => x.RegionId == region.RegionId && x.DisasterType.ToLower().Contains(SystemConstant.Disaster.Wildfire.ToLower())))
                warning.AlertTriggered = true;

            return warning;
        }

        private async Task<DisasterDto?> CalculateEarthquake(Region region, List<AlertHistory> alertHistories, DateTime currentDateTime)
        {
            Dictionary<string, string> queryStrings = new Dictionary<string, string>()
                {
                    {"format","geojson"},
                    {"latitude",$"{region.LocationCoordinates.Latitude}"},
                    {"longitude",$"{region.LocationCoordinates.Longitude}"},
                    {"maxradiuskm","100"},
                    {"starttime",currentDateTime.ToString("yyyy-MM-dd") },
                    {"endtime",currentDateTime.AddDays(1).ToString("yyyy-MM-dd") }
                };

            var responseString = await _apiService.SendRequestAsync("https://earthquake.usgs.gov/fdsnws/event/1/query", null, HttpMethod.Get, null, queryStrings);
            var currentEarthquake = JsonSerializer.Deserialize<EarthquakeApi>(responseString);

            var matchedEarthquake = currentEarthquake?.features?.FirstOrDefault();
            if (matchedEarthquake == null)
                return null;

            DisasterDto warning = new DisasterDto()
            {
                RegionId = region.RegionId,
                DisasterType = SystemConstant.Disaster.Flood,
            };

            if (matchedEarthquake.properties.mag < 4.0)
            {
                warning.RiskScore = 10;
                warning.RiskLevel = SystemConstant.RiksLevel.Low;
            }
            else if (matchedEarthquake.properties.mag >= 4.0 && matchedEarthquake.properties.mag <= 5.9)
            {
                warning.RiskScore = 55;
                warning.RiskLevel = SystemConstant.RiksLevel.Medium;
            }
            else
            {
                warning.RiskScore = 90;
                warning.RiskLevel = SystemConstant.RiksLevel.High;
            }

            var earthquakeDateTime = DateTimeOffset.FromUnixTimeMilliseconds(matchedEarthquake.properties.time).UtcDateTime;

            if (alertHistories.Any(x => x.RegionId == region.RegionId && x.DisasterType.ToLower().Contains(SystemConstant.Disaster.Earthquake.ToLower())
                                   && x.CreateDate < earthquakeDateTime))
                warning.AlertTriggered = true;

            return warning;

        }

        #endregion

    }
}
