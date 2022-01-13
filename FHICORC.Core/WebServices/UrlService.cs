using FHICORC.Core.Interfaces;

namespace FHICORC.Core.WebServices
{
    /// <summary>
    /// Url service for resolving urls.
    /// Could maybe be a part of RestClient, but kept outside to separate concerns.
    /// </summary>
    public class UrlService : IUrlService
    {
        private readonly ISettingsService _settingsService;

        public UrlService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public string ResolveUrl(ApiEndpoint endpoint)
        {
            return $"{_settingsService.BaseUrl}" +
                $"{_settingsService.ApiVersion}" +
                $"{ApiEndpointExtension.GetString(endpoint)}";
        }
    }
}
