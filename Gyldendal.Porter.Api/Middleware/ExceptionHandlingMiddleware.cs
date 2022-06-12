using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Gyldendal.Porter.Common;
using Microsoft.AspNetCore.Http;

namespace Gyldendal.Porter.Api.Middleware
{
    /// <summary>
    /// Handles application exceptions
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ContentType = "application/json";
        private readonly ILogger _logger;
        private readonly IErrorResponseExtractor _errorResponseExtractor;

        /// <summary>
        /// Initializes dependencies for exception handling middleware
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        /// <param name="errorResponseExtractor"></param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger, IErrorResponseExtractor errorResponseExtractor)
        {
            _next = next;
            _logger = logger;
            _errorResponseExtractor = errorResponseExtractor;
        }

        /// <summary>
        /// Handles exception
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // if response sending has already started, re-throw the exception
                if (context.Response.HasStarted)
                {
                    throw;
                }

                var content = await GetBodyFromRequest(request);
                _logger.Error($"Error encountered.  {_logger.GetPropertyNameAndValue(() => request.Path) } , HTTPContent:  {content}", ex, isGdprSafe: true);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task<string> GetBodyFromRequest(HttpRequest request)
        {
            request.Body.Position = 0;

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = ContentType;
            var errorResponse = _errorResponseExtractor.Extract(exception);
            var content = await errorResponse.Content.ReadAsStringAsync();

            context.Response.StatusCode = (int)errorResponse.StatusCode;
            await context.Response.WriteAsync(content);
        }
    }
}
