using System;
namespace FHICORC.Tests.TestMocks
{
    public class TestException : Exception
    {
        public TestException() : base() { }
        public TestException(string message) : base(message) { }
        public TestException(string message, Exception innerException) : base(message, innerException) { }

        public string StackTraceString { get; set; }
        public override string StackTrace => StackTraceString;
    }
}
