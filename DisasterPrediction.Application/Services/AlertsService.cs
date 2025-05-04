using AutoMapper;
using DisasterPrediction.Application.Common.BaseClass;
using DisasterPrediction.Application.Common.Interfaces;
using DisasterPrediction.Application.Common.Utils;
using DisasterPrediction.Application.DTOs;
using DisasterPrediction.Application.DTOs.Common;
using DisasterPrediction.Application.Interfaces;
using DisasterPrediction.Domain.Entities;
using DisasterPrediction.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Services
{
    public class AlertsService : BaseService, IAlertsService
    {
        private readonly IApiService _apiService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ICacheService _cacheService;
        public AlertsService(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper
            , IApiService apiService, IConfiguration configuration, IEmailService emailService, ICacheService cacheService) : base(context, currentUserService, mapper)
        {
            _apiService = apiService;
            _configuration = configuration;
            _emailService = emailService;   
            _cacheService = cacheService;
        }

        public async Task<SearchResult<AlertHistoryDto>> FindSummaryHistoryAsync(AlertHistoryFilterDto filter)
        {
            await ConvertDateTimePropertiesToUtc(filter);
            var result = new SearchResult<AlertHistoryDto>();

            int totalRecord = 0;
            var query = Context.AlertHistories.AsNoTracking();

            query = query.Where(x => filter.StartDate < x.CreateDate && x.CreateDate < filter.EndDate);

            totalRecord = await query.CountAsync();

            if (filter.PageSize != null && filter.PageNumber != null)
            {
                query = query
                    .Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                    .Take(filter.PageSize.Value)
                    .Select(t => t);
            }

            result.Data = Mapper.Map<List<AlertHistoryDto>>(await query.ToListAsync());
            result.PageNumber = filter.PageNumber ?? 0;
            result.PageSize = filter.PageSize ?? 0;
            result.TotalRecords = totalRecord;
            result.Keyword = filter.Keyword;
            result.TotalPages = totalRecord != 0 && filter.PageSize.HasValue ? (int)Math.Ceiling((double)totalRecord / filter.PageSize.Value) : 0;

            await FormatProperties(result.Data);
            return result;
        }

        public async Task<List<AlertHistoryDto>> GetHistoryByRegionAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return new List<AlertHistoryDto>();
            var result = Mapper.Map<List<AlertHistoryDto>>(await Context.AlertHistories.Where(x => x.RegionId == id).ToListAsync());

            return result;
        }

        public async Task SendWarningMessage()
        {
            var currentDateTime = DateTime.UtcNow;
            var disasterService = new DisasterService(Context, CurrentUserService, Mapper, _apiService, _configuration,_cacheService);
            var disasterRisks = await disasterService.GetDisasterRisk();
            var moreRisks = disasterRisks.Where(x => !x.AlertTriggered).ToList();
            if (ListUtil.IsEmptyList(moreRisks))
                return;

            var riskRegions = moreRisks.Select(x => x.RegionId).ToHashSet();
            var usersDic = Context.Users.Where(x => x.RegionId != null && riskRegions.Contains(x.RegionId)).ToLookup(x => x.RegionId!).ToDictionary(x => x.Key, y => y.ToList());

            var alertSendDtos = moreRisks.Select(x => new AlertSendDto()
            {
                RegionId = x.RegionId,
                DisasterType = x.DisasterType,
                RiskScore = x.RiskScore,
                AlertMessage = FindAlertMessage(x.DisasterType),
                Timestamp = currentDateTime,
            }).ToList();

            //Send Email
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            foreach (var alert in alertSendDtos)
            {
                if (usersDic.ContainsKey(alert.RegionId))
                {
                    var userRegions = usersDic[alert.RegionId];
                    foreach (var userRegion in userRegions)
                    {
                        if (!string.IsNullOrWhiteSpace(userRegion.Email))
                            await _emailService.SendEmailAsync(userRegion.Email, alert.DisasterType, alert.AlertMessage);
                    }
                }
            }

            var alertEntities = moreRisks.Select(x => new AlertHistory()
            {
                RegionId= x.RegionId,
                RiskScore= x.RiskScore,
                CreateDate = currentDateTime,
                DisasterType= x.DisasterType,
                RiskLevel = x.RiskLevel ?? string.Empty,
            }).ToList();

            await Context.AlertHistories.AddRangeAsync(alertEntities);
            await Context.SaveChangesAsync();
        }

        #region Private Method
        private string FindAlertMessage(string disasterType)
        {
            string message = string.Empty;
            switch (disasterType)
            {
                case SystemConstant.Disaster.Flood: 
                    message = _configuration.GetSection("AlertMessage:Flood").Value!;
                    break;
                case SystemConstant.Disaster.Wildfire:
                    message = _configuration.GetSection("AlertMessage:Wildfire").Value!;
                    break;
                case SystemConstant.Disaster.Earthquake:
                    message = _configuration.GetSection("AlertMessage:Earthquake").Value!;
                    break;
                default:
                    break;
            }

            return message;
        }

        #endregion

    }
}
