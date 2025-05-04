using AutoMapper;
using DisasterPrediction.Application.Common.BaseClass;
using DisasterPrediction.Application.Common.Exceptions;
using DisasterPrediction.Application.Common.Interfaces;
using DisasterPrediction.Application.Common.Utils;
using DisasterPrediction.Application.DTOs;
using DisasterPrediction.Application.Interfaces;
using DisasterPrediction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Services
{
    public class AlertSettingService : BaseService , IAlertSettingService
    {
        public AlertSettingService(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper) : base(context, currentUserService, mapper)
        {
        }

        public async Task<AlertSettingDto> CreateSettingAsync(AlertSettingDto request)
        {
            await ValidateCreate(request);
            var entity = Mapper.Map<AlertSetting>(request);
            await Context.AlertSettings.AddAsync(entity);
            await Context.SaveChangesAsync();

            var result = Mapper.Map<AlertSettingDto>(entity);
            return result;
        }

        public async Task<AlertSettingDto> UpdateSettingAsync(AlertSettingDto request)
        {
            var entity = await ValidateUpdate(request);

            entity.DisasterType = request.DisasterType;
            entity.ThresholdScore = request.ThresholdScore;

            await Context.SaveChangesAsync();

            var result = Mapper.Map<AlertSettingDto>(entity);
            return result;
        }

        public async Task DeleteSettingAsync(string id, string disasterType)
        {
            var entity = await ValidateDelete(id, disasterType);

            Context.AlertSettings.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<AlertSettingDto> GetSettingAsync(string id, string disasterType)
        {
            var entity = await Context.AlertSettings.FirstOrDefaultAsync(x => x.RegionId.ToLower() == id.ToLower() && x.DisasterType.ToLower() == disasterType.ToLower());
            var result = Mapper.Map<AlertSettingDto>(entity);

            return result;
        }

        public async Task<List<AlertSettingDto>> FindSettingAsync(AlertSettingFilterDto filterDto)
        {
            var query = Context.AlertSettings.AsNoTracking();
            if(!string.IsNullOrWhiteSpace(filterDto.RegionId))
                query = query.Where(x => x.RegionId == filterDto.RegionId);

            var result = Mapper.Map<List<AlertSettingDto>>(await query.ToListAsync());
            return result;
        }


        #region Private Method
        private async Task ValidateCreate(AlertSettingDto request)
        {
            Dictionary<string, List<string>> errorValidation = new Dictionary<string, List<string>>();
            if (request == null)
                throw new NotFoundException("Region is required");

            if (string.IsNullOrWhiteSpace(request.RegionId))
                InsertErrorValidation(errorValidation, "RegionId", "Required");

            var region = await Context.Regions.FirstOrDefaultAsync(x => x.RegionId == request.RegionId);
            if (region == null)
                InsertErrorValidation(errorValidation, "Region", "Invalid");
            if (string.IsNullOrWhiteSpace(request.DisasterType))
                InsertErrorValidation(errorValidation, "DisasterType", "Required");

            if (region != null && !region.DisasterTypes.ToLower().Contains(region.DisasterTypes.ToLower()))
                InsertErrorValidation(errorValidation, "DisasterType", "Not match with region");

            if (!ListUtil.IsEmptyList(errorValidation))
                throw new ValidationException(errorValidation);
        }
        private async Task<AlertSetting> ValidateUpdate(AlertSettingDto request)
        {
            await ValidateCreate(request);
            var entity = await Context.AlertSettings.FindAsync(request.RegionId, request.DisasterType );
            if (entity == null)
                throw new NotFoundException("Target was not found");

            return entity;
        }

        private async Task<AlertSetting> ValidateDelete(string id, string disasterType)
        {
            var entity = await Context.AlertSettings.FindAsync(id, disasterType);
            if (entity == null)
                throw new NotFoundException("Target was not found");

            return entity;
        }
        #endregion
    }
}
