using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace FHICORC.Core.WebServices
{
    public class ApiResponse
    {
        public string Endpoint { get; set; }
        public string ResponseText { get; set; }
        public HttpHeaders Headers { get; set; }

        public int StatusCode { get; set; }
        public bool IsSuccessfull => HasSuccessfullStatusCode && Exception == null;
        public bool HasSuccessfullStatusCode => (StatusCode == (int)HttpStatusCode.OK || StatusCode == (int)HttpStatusCode.Created);

        public ServiceErrorType ErrorType { get; set; } = ServiceErrorType.None;
        public Exception Exception { get; set; }

        public ApiResponse(string url)
        {
            Endpoint = url.Split(new string[]{"/"}, StringSplitOptions.None).Last();
        }

        public ApiResponse()
        {
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }

        public ApiResponse(string url) : base(url)
        {
            Data = default(T);
        }
        public ApiResponse(T data)
        {
            Data = data;
        }

        public ApiResponse(T data, int statuscode)
        {
            Data = data;
            StatusCode = statuscode;
        }

        public ApiResponse(ApiResponse apiResponse)
        {
            this.Endpoint = apiResponse.Endpoint;
            this.Exception = apiResponse.Exception;
            this.Headers = apiResponse.Headers;
            this.StatusCode = apiResponse.StatusCode;
        }

        public ApiResponse(ApiResponse apiResponse, T data)
        {
            this.Endpoint = apiResponse.Endpoint;
            this.Exception = apiResponse.Exception;
            this.Headers = apiResponse.Headers;
            this.StatusCode = apiResponse.StatusCode;
            this.ErrorType = apiResponse.ErrorType;
            this.Data = data;
        }
    }
}
