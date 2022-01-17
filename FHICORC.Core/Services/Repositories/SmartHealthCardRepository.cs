using System.Threading.Tasks;
using FHICORC.Core.Interfaces;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Coding;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Shc;
using FHICORC.Core.WebServices;
using System;
using FHICORC.Core.Services.Model.SmartHealthCardModel.Issuer;

namespace FHICORC.Core.Services.Repositories
{
    public class SmartHealthCardRepository : BaseRepository, ISmartHealthCardRepository
    {
        public SmartHealthCardRepository(
            IRestClient restClient,
            IUrlService urlService) : base(restClient, urlService)
        {
        }

        public async Task<SmartHealthCardVaccineInfo> GetVaccineInfo(SmartHealthCardCoding[] vaccineCodes)
        {
            string url = _urlService.ResolveUrl(ApiEndpoint.ShcVaccineInfo);
            SmartHealthCardVaccineInfoRequest body = new SmartHealthCardVaccineInfoRequest(vaccineCodes);
            ApiResponse<SmartHealthCardVaccineInfo> response = await _restClient.Post<SmartHealthCardVaccineInfo>(body, url);

            if (response.ErrorType != ServiceErrorType.None)
            {
                throw new Exception($"Error when getting vaccine info, error type: {response.ErrorType}");
            }

            return response.Data;
        }

        public async Task<SmartHealthCardIssuer> GetIssuerTrust(string issuer)
        {
            string url = _urlService.ResolveUrl(ApiEndpoint.ShcIssuerTrust);
            SmartHealthCardIssuerRequest body = new SmartHealthCardIssuerRequest(issuer);
            ApiResponse<SmartHealthCardIssuer> response = await _restClient.Post<SmartHealthCardIssuer>(body, url);

            if (response.ErrorType != ServiceErrorType.None)
            {
                throw new Exception($"Error when getting issuer trust, error type: {response.ErrorType}");
            }

            return response.Data;
        }
    }
}
