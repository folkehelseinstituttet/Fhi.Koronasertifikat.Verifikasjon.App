namespace FHICORC.Core.Services.Model.Exceptions
{
    /// <summary>
    ///  Exception for missing data
    /// </summary>
    public class MissingDataException : System.Exception
    {
        public MissingDataException(string message)
            : base(message)
        {
        }

        public MissingDataException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}
