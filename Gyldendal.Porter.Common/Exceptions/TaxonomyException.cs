using System.Net;

namespace Gyldendal.Porter.Common.Exceptions
{
    public class TaxonomyException : ExceptionBase
    {
        public TaxonomyException(ulong errorCode, string description) : base(HttpStatusCode.InternalServerError, errorCode, description)
        {
        }
    }
}
