using System;
using System.Net;
using System.Text;

namespace Gyldendal.Porter.Common.Exceptions
{
    public class ExceptionBase : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public ulong ErrorCode { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        public ExceptionBase(HttpStatusCode httpStatusCode, ulong errorCode, string description)
        {
            HttpStatusCode = httpStatusCode;
            ErrorCode = errorCode;
            Description = description;
            Details = this.ToString();
        }

        public sealed override string ToString()
        {
            var retVal = new StringBuilder();

            retVal.AppendLine(HttpStatusCode.ToString());
            retVal.AppendLine(ErrorCode.CheckIfNullThenDefault(nameof(ErrorCode)));
            retVal.AppendLine(Description.CheckIfNullThenDefault(nameof(Description)));
            retVal.AppendLine();
            retVal.AppendLine("Stack Trace: ");
            retVal.AppendLine(base.ToString());
            return retVal.ToString().RemoveEmptyLines();
        }
    }
}
