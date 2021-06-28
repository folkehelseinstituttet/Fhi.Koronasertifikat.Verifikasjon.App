using System;

namespace FHICORC.Core.Services.Interface
{
    public interface ITokenPayload
    {
        DateTime? ExpiredDateTime();
        DateTime? IssueDateTime();
    }
}