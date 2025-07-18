﻿using AutoMapper;
using DisasterPrediction.Application.Common.Interfaces;
using DisasterPrediction.Application.Common.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Common.BaseClass
{
    public class BaseService
    {
        public readonly ICurrentUserService CurrentUserService;
        public readonly IApplicationDbContext Context;
        public readonly IMapper Mapper;

        public BaseService(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            Context = context;
            CurrentUserService = currentUserService;
            Mapper = mapper;
        }

        public async Task FormatProperties<T>(T obj, string? requestTimeZoneId = null)
        {
            if (obj == null)
                return;

            string timeZoneId = "Asia/Bangkok";

            if (!string.IsNullOrWhiteSpace(CurrentUserService.UserId))
                timeZoneId = (await Context.Users.FindAsync(CurrentUserService.UserId))?.TimeZoneId ?? timeZoneId;

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(requestTimeZoneId ?? timeZoneId);

            FindTypeConvertDateTime(obj, timeZone);
        }

        public async Task ConvertDateTimePropertiesToUtc<T>(T obj, string? requestTimeZoneId = null)
        {
            if (obj == null)
                return;

            string timeZoneId = "Asia/Bangkok";

            if (!string.IsNullOrWhiteSpace(CurrentUserService.UserId))
                timeZoneId = (await Context.Users.FindAsync(CurrentUserService.UserId))?.TimeZoneId ?? timeZoneId;

            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(requestTimeZoneId ?? timeZoneId);
            FindTypeConvertDateTime(obj, timeZone,true);
        }

        public void FindTypeConvertDateTime<T>(T obj, TimeZoneInfo timeZone, bool isConvertInput = false)
        {
            if (obj == null)
                return;

            // Check if the input is a collection
            if (obj is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    if (isConvertInput)
                        ConvertToUtc(item, timeZone);
                    else
                        ConvertProperties(item, timeZone);
                }
            }
            else
            {
                if (isConvertInput)
                    ConvertToUtc(obj, timeZone);
                else
                    ConvertProperties(obj, timeZone);
            }
        }
        public void InsertErrorValidation(Dictionary<string, List<string>> errorValidation, string key, string value)
        {
            if (!ListUtil.IsEmptyList(errorValidation) && errorValidation.ContainsKey(key))
                errorValidation[key].Add(value);
            else
                errorValidation.Add(key, new List<string>() { value });
        }

        #region Private Method
        private void ConvertProperties(object obj, TimeZoneInfo timeZone)
        {
            if (obj == null) return;

            foreach (var property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanWrite)
                    continue;
                if ((property.PropertyType.IsClass || typeof(IEnumerable).IsAssignableFrom(property.PropertyType)) && property.PropertyType != typeof(string))
                {
                    var nestedObject = property.GetValue(obj);
                    FindTypeConvertDateTime(nestedObject, timeZone);
                }
                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    var value = property.GetValue(obj);
                    if (value is DateTime dateTime)
                    {
                        if (dateTime.Kind == DateTimeKind.Utc)
                        {
                            var localTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZone);
                            property.SetValue(obj, localTime);
                        }
                        else if (dateTime.Kind != DateTimeKind.Utc)
                        {
                            var utcTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZone);
                            property.SetValue(obj, utcTime);
                        }
                    }
                }
                else if (property.PropertyType.IsEnum)
                {
                    var enumValue = property.GetValue(obj);
                    if (enumValue != null)
                    {
                        string enumName = enumValue.ToString()!;
                        string displayNameProperty = property.Name + "DisplayName";

                        var displayProp = obj.GetType().GetProperty(displayNameProperty, BindingFlags.Public | BindingFlags.Instance);
                        if (displayProp != null && displayProp.CanWrite && displayProp.PropertyType == typeof(string))
                        {
                            displayProp.SetValue(obj, enumName);
                        }
                    }
                }
            }
        }
        private void ConvertToUtc(object obj, TimeZoneInfo timeZone)
        {
            if (obj == null) return;

            foreach (var property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanWrite)
                    continue;
                if ((property.PropertyType.IsClass || typeof(IEnumerable).IsAssignableFrom(property.PropertyType)) && property.PropertyType != typeof(string))
                {
                    var nestedObject = property.GetValue(obj);
                    FindTypeConvertDateTime(nestedObject, timeZone, true);
                }
                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    var value = property.GetValue(obj);
                    if (value is DateTime dateTime)
                    {
                        if (dateTime.Kind != DateTimeKind.Utc)
                        {
                            var utcTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZone);
                            property.SetValue(obj, utcTime);
                        }
                        
                    }
                }
            }
        }
        #endregion
    }
}
