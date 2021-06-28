using System;
namespace FHICORC.Core.WebServices
{
    public enum ServiceErrorType
    {
        None,
        NoInternetConnection,
        BadInternetConnection,
        Timeout,
        TrustFailure, //Certificate pinning
        InternalAppError, //Unknown exception in the app, i.e. Parsing exceptions
        ServerError,
        RefreshTokenRenewalFailed,
        Maintenance,
        InQueue,
        LockNemID,
        UnknownError,
        UserSessionExpired,
        Gone

    }
}
