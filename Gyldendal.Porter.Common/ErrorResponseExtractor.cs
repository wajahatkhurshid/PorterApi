using System;
using System.Net;
using System.Net.Http;
using Gyldendal.Porter.Common.Exceptions;
using Newtonsoft.Json;

namespace Gyldendal.Porter.Common
{
    public class ErrorResponseExtractor : IErrorResponseExtractor
    {
        public HttpResponseMessage Extract(Exception exception)
        {
            var response = GetSerializedContent(exception);
            return response;
        }

        private static HttpResponseMessage GetSerializedContent(Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            object content;

            switch (exception)
            {
                case TaxonomyException taxonomyException:
                    content = new TaxonomyException(taxonomyException.ErrorCode, taxonomyException.Description);
                    break;
                case ApiException apiException:
                    statusCode = HttpStatusCode.BadRequest;
                    content = new ApiException(apiException.ErrorCode, apiException.Description);
                    break;
                default:
                    content =  exception;
                    break;
            }

            var serializedContent = JsonConvert.SerializeObject(content);
            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(serializedContent, System.Text.Encoding.UTF8, "application/json")
            };
        }
    }
}
