using Microsoft.AspNetCore.Mvc;
using PixelTest.Storage.Api.Host.Contracts.Requests;
using PixelTest.Storage.Api.Host.Mappers;
using PixelTest.Storage.Services.Contracts.Interfaces;

namespace PixelTest.Track.Api.Host.Controllers;

[ApiController]
[Route("api")]
public class StorageController : ControllerBase
{
	private readonly ILogger _logger;
	private readonly IStorageService _storageService;

	public StorageController(ILogger<StorageController> logger, IStorageService storageService)
	{
		_logger = logger;
		_storageService = storageService;
	}

	[HttpPost("save")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> SaveDataAsync([FromBody] TrackDataRequest request, CancellationToken cancellationToken)
	{
		try
		{
			if (await _storageService.SaveTrackAsync(request.ToModel(), cancellationToken))
			{
				return Ok();
			}
		} catch (Exception ex)
		{
			_logger.LogError(ex, "Failed process track data request. Request: '{@request}'", request);
			return BadRequest(ex.Message);
		}

		return BadRequest("Internal server error");
	}
}