using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Gyldendal.Porter.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using ServiceStack.Text;

namespace Gyldendal.Porter.Api.Middleware
{
    /// <summary>
    /// Adds a logger on requests and responses
    /// </summary>
    public class HttpLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;


        /// <summary>
        /// Initializes logger specific dependencies
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public HttpLoggingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        /// <summary>
        /// Log the specific request/response
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            _logger.SetCurrentThreadContext(Guid.NewGuid().ToString());

            await using var requestBodyStream = _recyclableMemoryStreamManager.GetStream();
            var originalRequestBody = context.Request.Body;

            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            var url = context.Request.GetDisplayUrl();
            var requestBodyText = await new StreamReader(requestBodyStream).ReadToEndAsync();
            var headers = GetHeaders(context.Request.Headers);

            _logger.Info(
                $"REQUEST METHOD: {context.Request.Method}{Environment.NewLine}" +
                $"REQUEST HEADERS: {headers}{Environment.NewLine}" +
                $"REQUEST BODY: {requestBodyText}{Environment.NewLine}" +
                $"REQUEST URL: {url}", isGdprSafe: true);

            requestBodyStream.Seek(0, SeekOrigin.Begin);

            if (url.EndsWith("/GpmSubscription"))
            {
                var requestData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestBodyText));
                context.Request.Body = new MemoryStream(requestData);
            }
            else
            {
                context.Request.Body = requestBodyStream;
            }

            await _next(context);
            context.Request.Body = originalRequestBody;
        }

        private static string GetHeaders(IHeaderDictionary headers)
        {
            var headerBuilder = new StringBuilder(Environment.NewLine);

            foreach (var (key, value) in headers)
            {
                headerBuilder.AppendLine($"{key}:{value}");
            }

            return headerBuilder.ToString();
        }
    }
}
