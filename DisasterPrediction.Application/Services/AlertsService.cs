using AutoMapper;
using DisasterPrediction.Application.Common.BaseClass;
using DisasterPrediction.Application.Common.Interfaces;
using DisasterPrediction.Application.DTOs;
using DisasterPrediction.Application.DTOs.Common;
using DisasterPrediction.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Services
{
    public class AlertsService : BaseService, IAlertsService
    {
        public AlertsService(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper) : base(context, currentUserService, mapper)
        {
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

        public void SendWarningMessage()
        {

        }

    }
}
