using Microsoft.Extensions.Logging;
using PixelTest.Storage.Api.Host.Contracts.Interfaces;
using PixelTest.Track.Services.Contracts.Interfaces;
using PixelTest.Track.Services.Contracts.Models;

namespace PixelTest.Track.Services.Services;

internal class TrackService : ITrackService
{
	private readonly ILogger _logger;
	private readonly IStorageApiClient _storageClient;

	public TrackService(ILogger<TrackService> logger, IStorageApiClient storageClient)
	{
		_logger = logger;
		_storageClient = storageClient;
	}

	public async Task<bool> TrackDataAsync(TrackDataModel trackData, CancellationToken cancellationToken)
	{
		try
		{
			var response = await _storageClient.SaveTrackAsync(new()
			{
				IP = trackData.IP,
				Referrer = trackData.Referrer,
				UserAgent = trackData.UserAgent
			}, cancellationToken);

			return response.IsSuccessStatusCode;

		} catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to track data. Data: '{@data}'.", trackData);
		}

		return false;
	}
}
