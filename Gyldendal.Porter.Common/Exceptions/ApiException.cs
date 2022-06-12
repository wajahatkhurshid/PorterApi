using System.Net;

namespace Gyldendal.Porter.Common.Exceptions
{
    public class ApiException : ExceptionBase
    {
        public ApiException(ulong errorCode, string description) : base(HttpStatusCode.BadRequest, errorCode, description)
        {
        }
    }
}
