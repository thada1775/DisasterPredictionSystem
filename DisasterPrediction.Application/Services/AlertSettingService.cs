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
            ValidateCreate(request);
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

        public async Task DeleteSettingAsync(string id)
        {
            var entity = await ValidateDelete(id);

            Context.AlertSettings.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<AlertSettingDto> GetSettingAsync(string id)
        {
            var entity = await Context.AlertSettings.FirstOrDefaultAsync(x => x.RegionId == id);
            var result = Mapper.Map<AlertSettingDto>(entity);

            return result;
        }

        public async Task<List<AlertSettingDto>> FindSettingAsync()
        {
            var result = Mapper.Map<List<AlertSettingDto>>(await Context.AlertSettings.ToListAsync());
            return result;
        }


        #region Private Method
        private void ValidateCreate(AlertSettingDto request)
        {
            Dictionary<string, List<string>> errorValidation = new Dictionary<string, List<string>>();
            if (request == null)
                throw new NotFoundException("Region is required");

            if (string.IsNullOrWhiteSpace(request.RegionId))
                InsertErrorValidation(errorValidation, "RegionId", "Required.");
            if (string.IsNullOrWhiteSpace(request.DisasterType))
                InsertErrorValidation(errorValidation, "DisasterType", "Required.");

            if (!ListUtil.IsEmptyList(errorValidation))
                throw new ValidationException(errorValidation);
        }
        private async Task<AlertSetting> ValidateUpdate(AlertSettingDto request)
        {
            ValidateCreate(request);
            var entity = await Context.AlertSettings.FindAsync(request.RegionId);
            if (entity == null)
                throw new NotFoundException("Target was not found");

            return entity;
        }

        private async Task<AlertSetting> ValidateDelete(string id)
        {
            var entity = await Context.AlertSettings.FindAsync(id);
            if (entity == null)
                throw new NotFoundException("Target was not found");

            return entity;
        }
        #endregion
    }
}
