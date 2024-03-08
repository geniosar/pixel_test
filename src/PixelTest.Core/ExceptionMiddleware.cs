using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PixelTest.Core
{
	public class ExceptionMiddleware
	{
		private readonly ILogger _logger;

		private readonly RequestDelegate _next;

		private readonly IHostEnvironment _environment;

		public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next, IHostEnvironment environment)
		{
			_next = next;
			_logger = logger;
			_environment = environment;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			} catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception");
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = 500;
				await HttpResponseWritingExtensions.WriteAsync(text: ex.Message, response: context.Response);
			}
		}
	}
}
