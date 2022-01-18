using System;

namespace FHICORC.Core.Services.Model.Exceptions
{
    /// <summary>
    ///  Exception for invalid signature
    /// </summary>
    public class InvalidSignatureException : Exception
    {
        public InvalidSignatureException(string message)
            : base(message)
        {
        }

        public InvalidSignatureException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
