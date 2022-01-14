using System;

namespace FHICORC.Core.WebServices
{
    /// <summary>
    /// Shared api endpoint enum for the whole project.
    /// Preferably core should not know of endpoints outside.
    /// </summary>
    public enum ApiEndpoint
    {
        PublicKey,
        Text,
        BusinessRules,
        ValueSets,
        ShcVaccineInfo
    }

    public static class ApiEndpointExtension
    {
        public static string GetString(ApiEndpoint endpoint)
        {
            switch (endpoint)
            {
                case ApiEndpoint.PublicKey:
                    return "/publickey";
                case ApiEndpoint.Text:
                    return "/text";
                case ApiEndpoint.BusinessRules:
                    return "/rule";
                case ApiEndpoint.ValueSets:
                    return "/valueset";
                case ApiEndpoint.ShcVaccineInfo:
                    return "/shc/vaccineinfo";
            }

            throw new NotImplementedException($"String not added for api endpoint {endpoint}");
        }
    }
}