using System;
using FHICORC.Core.Services.Interface;

namespace FHICORC.Tests.TestMocks
{
    public class MockDateTimeService : IDateTimeService
    {
        public DateTime Now { get; set; } = DateTime.UtcNow;
    }
}