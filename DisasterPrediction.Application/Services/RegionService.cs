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
        private readonly ICacheService _cacheService;
        private const string _regionCacheKey = "region_{0}";
        public RegionService(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper, ICacheService cacheService) : base(context, currentUserService, mapper)
        {
            _cacheService = cacheService;
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

            var cacheKey = string.Format(_regionCacheKey, request.RegionId);
            await _cacheService.RemoveAsync(cacheKey);

            if (entity.LocationCoordinates == null)
                entity.LocationCoordinates = new LocationCoordinates();

            entity.LocationCoordinates.Latitude = request.LocationCoordinates.Latitude;
            entity.LocationCoordinates.Longitude = request.LocationCoordinates.Longitude;
            entity.DisasterTypes = JsonSerializer.Serialize(request.DisasterTypes);

            await Context.SaveChangesAsync();

            var result = Mapper.Map<RegionDto>(entity);
            if (!string.IsNullOrWhiteSpace(entity.DisasterTypes))
                result.DisasterTypes = JsonSerializer.Deserialize<List<string>>(entity.DisasterTypes)!;

            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(15));
            return result;
        }

        public async Task DeleteEntityAsync(string id)
        {
            var entity = await ValidateDelete(id);

            var cacheKey = string.Format(_regionCacheKey, id);
            await _cacheService.RemoveAsync(cacheKey);

            Context.Regions.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<RegionDto> GetEntityAsync(string id)
        {
            var cacheKey = string.Format(_regionCacheKey, id);
            var cachedRegion = await _cacheService.GetAsync<RegionDto>(cacheKey);

            if (cachedRegion != null)
                return cachedRegion;

            var entity = await Context.Regions.Where(x => x.RegionId == id).Include(x => x.LocationCoordinates).FirstOrDefaultAsync();
            var result = Mapper.Map<RegionDto>(entity);

            if (entity != null && !string.IsNullOrWhiteSpace(entity.DisasterTypes))
                result.DisasterTypes = JsonSerializer.Deserialize<List<string>>(entity.DisasterTypes) ?? new List<string>();

            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(15));

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
        private void ValidateCreate(RegionDto request)
        {
            Dictionary<string, List<string>> errorValidation = new Dictionary<string, List<string>>();
            if (request == null)
                throw new NotFoundException("Region is required");

            if (string.IsNullOrWhiteSpace(request.RegionId))
                InsertErrorValidation(errorValidation, "RegionId", "Required.");
            if (request.RegionId.Length > 50)
                InsertErrorValidation(errorValidation, "RegionId", "Length more than 50");
            if (ListUtil.IsEmptyList(request.DisasterTypes))
                InsertErrorValidation(errorValidation, "DisasterTypes", "Required.");
            if (!GeographyUtil.ValidateLatLon(request.LocationCoordinates.Latitude, request.LocationCoordinates.Longitude))
                InsertErrorValidation(errorValidation, "LocationCoordinates", "Latitude or Longitude is invalid.");

            if (!ListUtil.IsEmptyList(errorValidation))
                throw new ValidationException(errorValidation);
        }
        private async Task<Region> ValidateUpdate(RegionDto request)
        {
            ValidateCreate(request);
            var entity = await Context.Regions.Include(x => x.LocationCoordinates).FirstOrDefaultAsync(x => x.RegionId.ToLower() == request.RegionId.ToLower());
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
