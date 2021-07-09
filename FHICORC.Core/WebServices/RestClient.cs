using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using FHICORC.Core.Data;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Interface;
using Xamarin.Forms;
using static System.Net.WebRequestMethods;

namespace FHICORC.Core.WebServices
{
    public class RestClient : IRestClient
    {
        private readonly ISettingsService _settingsService;
        private readonly IConnectivityService _connectivityService;
        private readonly ISslCertificateService _sslCertificateService;
        private readonly IAssemblyService _assemblyService;

        private static object _httpClientHandler = null;
        private SemaphoreSlim _semaphoreSlim;

        public HttpClient HttpClient { get; private set; }
        public static object HttpClientHandler
        {
            get => _httpClientHandler;
            set
            {
                if (_httpClientHandler != null) return;
                if (value is NativeMessageHandler)
                    _httpClientHandler = value;
                else
                    throw new Exception($"{nameof(HttpClientHandler)} incorrect type!");
            }
        }

        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Local
        };

        public RestClient(
            ISettingsService settingsService,
            IConnectivityService connectivityService, 
            ISslCertificateService sslCertificateService,
            IAssemblyService assemblyService)
        {
            _settingsService = settingsService;
            _connectivityService = connectivityService;
            _sslCertificateService = sslCertificateService;
            _assemblyService = assemblyService;
            
            SetupTrustedCertificate();

            if (Device.RuntimePlatform == Device.Android)
            {
                HttpClient = HttpClientHandler != null
                    ? new HttpClient((CertificateValidationHandler)HttpClientHandler)
                    : new HttpClient(new CertificateValidationHandler(this));
            }
            else
            {
                HttpClient = HttpClientHandler != null
                    ? new HttpClient((NativeMessageHandler)HttpClientHandler)
                    : new HttpClient(new NativeMessageHandler(throwOnCaptiveNetwork: false, customSSLVerification: true));

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = ServerCertificateValidationCallback;
            }

            HttpClient.Timeout = TimeSpan.FromSeconds(_settingsService.DefaultTimeout);
            HttpClient.BaseAddress = new Uri(_settingsService.BaseUrl);
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Add("Authorization", _settingsService.AuthorizationHeader); //Only first line of defense. Expected to get leaked.

            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        void SetupTrustedCertificate()
        {
            string certPrefix = "FHICORC.Certs";
            string cert = _settingsService.TrustedSSLCertificateFileName;

            if (cert != null)
            {
                string certManifestResource = $"{certPrefix}.{cert}";
                try
                {
                    Stream certStream = _assemblyService.GetSharedFormsAssembly().GetManifestResourceStream(certManifestResource);
                    
                    _sslCertificateService.SetTrustedCertificate(certStream);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        public bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            try
            {
                X509Certificate trustedCert = _sslCertificateService.GetTrustedCertificate();
                
                if (trustedCert != null)
                {
                    // NOTE: Return true here if you want to skip SSL pinning during development
                    //return true;
                    return string.Equals(trustedCert.GetPublicKeyString(), certificate.GetPublicKeyString());
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void ClearAccessTokenHeader()
        {
            if (HttpClient.DefaultRequestHeaders.Contains("AccessToken"))
            {
                HttpClient.DefaultRequestHeaders.Remove("AccessToken");
            }
        }

        public virtual async Task<ApiResponse<T>> Get<T>(string uri)
        {
            Debug.Print($"GET {uri}");
            ApiResponse<T> result = new ApiResponse<T>(uri);

            try
            {
                HttpResponseMessage response = await PerformRequest(Http.Get, uri);
                
                result.StatusCode = (int)response.StatusCode;
                
                if (!response.IsSuccessStatusCode)
                {
                    Debug.Print(response.ReasonPhrase);
                    result.ResponseText = response.ReasonPhrase;
                    HandleErrors(result);
                    return result;
                }

                result.ResponseText = await response.Content.ReadAsStringAsync();
                Debug.Print($"Response: {result.ResponseText}");
                result.Data = JsonConvert.DeserializeObject<T>(result.ResponseText, JsonSerializerSettings);
                PrettyPrintJsonObject("Response", result.Data);
            }
            catch (Exception e)
            {
                HandleErrors(result, e);
            }

            return result;
        }

        public async Task<ApiResponse> Post(string url)
        {
            return await Post<object>(null, url);
        }

        public virtual async Task<ApiResponse<T>> Post<T>(object body, string url)
        {
            Debug.Print($"POST {url}");
            string jsonBody = JsonConvert.SerializeObject(body, JsonSerializerSettings);
            PrettyPrintJsonObject("Body", body);
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            ApiResponse<T> result = new ApiResponse<T>(url);

            try
            {                
                HttpResponseMessage response = await PerformRequest(Http.Post, url, content);

                result.StatusCode = (int)response.StatusCode;

                if (!response.IsSuccessStatusCode)
                {
                    Debug.Print(response.ReasonPhrase);
                    result.ResponseText = response.ReasonPhrase;
                    HandleErrors(result);
                    return result;
                }

                result.ResponseText = await response.Content.ReadAsStringAsync();
                result.Data = JsonConvert.DeserializeObject<T>(result.ResponseText, JsonSerializerSettings);
                PrettyPrintJsonObject("Response", result.Data);

            }
            catch (Exception e)
            {
                HandleErrors(result, e);
            }

            return result;
        }

        void HandleErrors<T>(ApiResponse<T> result, Exception e)
        {
            result.Exception = e;

            if (!_connectivityService.HasInternetConnection())
            {
                result.ErrorType = ServiceErrorType.NoInternetConnection;
            }
            else if (IsBadConnectionError(e))
            {
                result.ErrorType = ServiceErrorType.BadInternetConnection;
            }
            else if (e.InnerException is AuthenticationException)
            {
                result.ErrorType = ServiceErrorType.TrustFailure;
            }
            else if (e is TaskCanceledException)
            {
                //As long as we don't cancel tasks manually, then TaskCanceledException will only be thrown for timeouts
                result.ErrorType = ServiceErrorType.Timeout;
            }
            else
            {
                result.ErrorType = ServiceErrorType.InternalAppError;
            }
        }

        void HandleErrors<T>(ApiResponse<T> result)
        {
            if (result.IsSuccessfull)
                return;

            if (result.StatusCode == 401)
            {
                return;
            }

            if (result.StatusCode == 410)
            {
                result.ErrorType = ServiceErrorType.Gone;
                return;
            }

            result.ErrorType = ServiceErrorType.ServerError;
        }

        bool IsBadConnectionError(Exception e)
        {
            bool isIOSException = e?.InnerException is IOException
                && (e.InnerException.Message.Contains("the transport connection")
                || e.InnerException.Message.Contains("The server returned an invalid or unrecognized response"));

            //This will contain a more detailed error description in the Inner exception
            bool isWebException = e is WebException;

            return isIOSException || isWebException;
        }

        void PrettyPrintJsonObject(string prefix, object jsonObj)
        {
            try
            {
                Debug.Print($"{prefix}: {JsonConvert.SerializeObject(jsonObj, Formatting.Indented, JsonSerializerSettings)}");
            }
            catch { }
        }

        public virtual async Task<ApiResponse<Stream>> GetFileAsStreamAsync(string url)
        {
            Debug.Print($"Downloading file: {url}");
            ApiResponse<Stream> result = new ApiResponse<Stream>(url);
            try
            {
                HttpResponseMessage response = await PerformRequest(Http.Get, url);
               
                result.StatusCode = (int)response.StatusCode;

                if (!response.IsSuccessStatusCode)
                {
                    result.ResponseText = response.ReasonPhrase;
                    HandleErrors(result);
                    return result;
                }

                Stream content = await response.Content.ReadAsStreamAsync();
                result.Headers = response.Headers;
                if (content.Length > 0)
                    result.Data = content;

                Debug.WriteLine("Page content: " + content);
            }
            catch (Exception e)
            {
                HandleErrors(result, e);
            }

            return result;
        }

        public async Task<HttpResponseMessage> PerformRequest(string method, string url, HttpContent body = null)
        {
            HttpResponseMessage response = default;
            bool tryAgain = false;
            try
            {
                response = method == Http.Post
                    ? await HttpClient.PostAsync(url, body)
                    : await HttpClient.GetAsync(url);
            }
            catch (Exception e)
            {
                if (IsBadConnectionError(e))
                    tryAgain = true;
                else
                    throw;
            }

            if (tryAgain)
            {
                Debug.Print("Failed http call due to bad connection. Retrying...");
                Thread.Sleep(100);
                response = method == Http.Post
                    ? await HttpClient.PostAsync(url, body)
                    : await HttpClient.GetAsync(url);
            }

            if (response == default)
            {
                throw new HttpRequestException($"No response");
            }
            Debug.Print($"Statuscode: {(int)response.StatusCode} {response.StatusCode.ToString()}");

            return response;
        }

        public void RegisterLocalesRequestHeaders(string versionNumber)
        {
            ClearLocalesRequestHeaders();

            HttpClient.DefaultRequestHeaders.Add("CurrentVersionNo", versionNumber);
        }

        public void ClearLocalesRequestHeaders()
        {
            if (HttpClient.DefaultRequestHeaders.Contains("CurrentVersionNo"))
            {
                HttpClient.DefaultRequestHeaders.Remove("CurrentVersionNo");
            }
        }

        public void RegisterValuesetsRequestHeaders(string lastTimestamp)
        {
            ClearValuesetsRequestHeaders();

            HttpClient.DefaultRequestHeaders.Add("LastFetched", lastTimestamp);
        }

        public void ClearValuesetsRequestHeaders()
        {
            if (HttpClient.DefaultRequestHeaders.Contains("LastFetched"))
            {
                HttpClient.DefaultRequestHeaders.Remove("LastFetched");
            }
        }
    }
}