using System;
using System.Net.Http;

namespace Gyldendal.Porter.Common
{
    public interface IErrorResponseExtractor
    {
        HttpResponseMessage Extract(Exception exception);
    }
}
