using AutoMapper;
using DisasterPrediction.Application.Common.BaseClass;
using DisasterPrediction.Application.Common.Exceptions;
using DisasterPrediction.Application.Common.Interfaces;
using DisasterPrediction.Application.Common.Utils;
using DisasterPrediction.Application.DTOs;
using DisasterPrediction.Application.Interfaces;
using DisasterPrediction.Domain.Entities;
using DisasterPrediction.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Services
{
    public class RegionService : BaseService, IRegionService
    {
        public RegionService(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper) : base(context, currentUserService, mapper)
        {
        }

        public async Task<RegionDto> CreateEntityAsync(RegionDto request)
        {
            ValidateCreate(request);
            var entity = Mapper.Map<Region>(request);
            entity.DisasterTypes = JsonSerializer.Serialize(request.DisasterTypes);
            await Context.Regions.AddAsync(entity);
            await Context.SaveChangesAsync();

            var result = Mapper.Map<RegionDto>(entity);
            if (!string.IsNullOrWhiteSpace(entity.DisasterTypes))
                result.DisasterTypes = JsonSerializer.Deserialize<List<string>>(entity.DisasterTypes) ?? new List<string>();
            return result;
        }

        public async Task<RegionDto> UpdateEntityAsync(RegionDto request)
        {
            var entity = await ValidateUpdate(request);
            if (entity.LocationCoordinates == null)
                entity.LocationCoordinates = new LocationCoordinates();

            entity.LocationCoordinates.Latitude = request.LocationCoordinates.Latitude;
            entity.LocationCoordinates.Longitude = request.LocationCoordinates.Longitude;

            await Context.SaveChangesAsync();

            var result = Mapper.Map<RegionDto>(entity);
            if (!string.IsNullOrWhiteSpace(entity.DisasterTypes))
                result.DisasterTypes = JsonSerializer.Deserialize<List<string>>(entity.DisasterTypes)!;
            return result;
        }

        public async Task DeleteEntityAsync(string id)
        {
            var entity = await ValidateDelete(id);

            Context.Regions.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<RegionDto> GetEntityAsync(string id)
        {
            var entity = await Context.Regions.Where(x => x.RegionId == id).Include(x => x.LocationCoordinates).FirstOrDefaultAsync();
            var result = Mapper.Map<RegionDto>(entity);

            if (entity != null && !string.IsNullOrWhiteSpace(entity.DisasterTypes))
                result.DisasterTypes = JsonSerializer.Deserialize<List<string>>(entity.DisasterTypes) ?? new List<string>();

            return result;
        }


        public async Task<List<RegionDto>> FindEntityuAsync()
        {
            var result = await Context.Regions.Include(x => x.LocationCoordinates)
                .Select(x => new RegionDto()
                {
                    RegionId = x.RegionId,
                    LocationCoordinates = new LocationCoordinatesDto()
                    {
                        Latitude = x.LocationCoordinates.Latitude,
                        Longitude = x.LocationCoordinates.Longitude
                    },
                    DisasaterTypesString = x.DisasterTypes
                }).ToListAsync();

            foreach (var entity in result)
            {
                if (!string.IsNullOrWhiteSpace(entity.DisasaterTypesString))
                    entity.DisasterTypes = JsonSerializer.Deserialize<List<string>>(entity.DisasaterTypesString) ?? new List<string>();
            }

            return result;
        }

        #region Private Method
        private void ValidateCreate(RegionDto dto)
        {
            Dictionary<string, List<string>> errorValidation = new Dictionary<string, List<string>>();
            if (dto == null)
                throw new NotFoundException("Region is required");

            if (string.IsNullOrWhiteSpace(dto.RegionId))
                InsertErrorValidation(errorValidation, "RegionId", "Required.");
            if (dto.RegionId.Length > 50)
                InsertErrorValidation(errorValidation, "RegionId", "Length more than 50");
            if (!ListUtil.IsEmptyList(dto.DisasterTypes))
                InsertErrorValidation(errorValidation, "DisasterTypes", "Required.");

            if (!ListUtil.IsEmptyList(errorValidation))
                throw new ValidationException(errorValidation);
        }
        private async Task<Region> ValidateUpdate(RegionDto request)
        {
            ValidateCreate(request);
            var entity = await Context.Regions.FindAsync(request.RegionId);
            if (entity == null)
                throw new NotFoundException("Target was not found");

            return entity;
        }
        private async Task<Region> ValidateDelete(string id)
        {
            var entity = await Context.Regions.FindAsync(id);
            if (entity == null)
                throw new NotFoundException("Target was not found");

            return entity;
        }
        #endregion
    }
}
