using Microsoft.AspNetCore.Mvc;
using PixelTest.Track.Api.Host.Constants;
using PixelTest.Core.Extensions;
using PixelTest.Track.Services.Contracts.Interfaces;

namespace PixelTest.Track.Api.Host.Controllers;

[ApiController]
[Route("api")]
public class TrackController : ControllerBase
{
	private readonly ILogger _logger;
	private readonly ITrackService _trackService;

	public TrackController(ILogger<TrackController> logger, ITrackService trackService)
	{
		_logger = logger;
		_trackService = trackService;
	}

	[HttpGet("track")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> TrackAsync(CancellationToken cancellationToken)
	{
		try
		{
			var ipAdd = GetHeaderValueAs<string>("X-Forwarded-For")?.SplitCsv()?.FirstOrDefault();

			// bug: RemoteIpAddress is always null in DNX RC1 Update1
			if (ipAdd.IsNullOrWhitespace() && HttpContext?.Connection?.RemoteIpAddress != null)
				ipAdd = HttpContext.Connection.RemoteIpAddress.ToString();

			if (ipAdd.IsNullOrWhitespace())
				ipAdd = GetHeaderValueAs<string>("REMOTE_ADDR");

			if (ipAdd is null)
			{
				return BadRequest("IP address not recognized.");

			}

			var referrer = GetHeaderValueAs<string>("Referrer");
			var userAgent = GetHeaderValueAs<string>("User-Agent");
			if (await _trackService.TrackDataAsync(new()
			{
				IP = ipAdd,
				Referrer = referrer,
				UserAgent = userAgent
			}, cancellationToken))
			{
				return new FileContentResult(Convert.FromBase64String(TrackApiConstants.ClearGif1X1), "image/gif");
			}


		} catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to process track request.");
		}

		return BadRequest("Internal server error.");
	}

	private T? GetHeaderValueAs<T>(string headerName)
	{
		if (HttpContext?.Request?.Headers?.TryGetValue(headerName, out var values) ?? false)
		{
			var rawValues = values.ToString();

			if (!rawValues.IsNullOrWhitespace())
				return (T)Convert.ChangeType(values.ToString(), typeof(T));
		}

		return default;
	}
}
