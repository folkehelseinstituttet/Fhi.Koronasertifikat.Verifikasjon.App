﻿using FHICORC.Configuration;
using FHICORC.Core.Interfaces;

namespace FHICORC.Configuration
{
    public static class Urls
    {
        public static string _baseUrl => IoCContainer.Resolve<ISettingsService>().BaseUrl;

        public static string _apiVersion => IoCContainer.Resolve<ISettingsService>().ApiVersion;

        public static string URL_GET_PUBLIC_KEY => $"{_baseUrl}{_apiVersion}/publickey";
        public static string URL_GET_TEXTS => $"{_baseUrl}{_apiVersion}/text";
        public static string URL_GET_BUSINESS_RULES => $"{_baseUrl}{_apiVersion}/rule";
    }
}
