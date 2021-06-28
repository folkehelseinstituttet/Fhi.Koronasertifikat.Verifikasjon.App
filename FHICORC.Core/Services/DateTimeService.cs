using System;
using FHICORC.Core.Services.Interface;

namespace FHICORC.Core.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}