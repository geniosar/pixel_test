using Microsoft.Extensions.Logging;
using PixelTest.Storage.Services.Contracts.Interfaces;
using PixelTest.Storage.Services.Contracts.Models;

namespace PixelTest.Storage.Services.Services;

internal class StorageService : IStorageService
{
	private readonly ILogger _logger;
	private readonly ITrackDataSaverWriter _trackDataSaver;

	public StorageService(ILogger<StorageService> logger, ITrackDataSaverWriter trackDataSaver)
	{
		_logger = logger;
		_trackDataSaver = trackDataSaver;
	}

	public async Task<bool> SaveTrackAsync(TrackDataModel trackData, CancellationToken cancellationToken)
	{
		try
		{
			await _trackDataSaver.AddNewTrackAsync(trackData);

			return true;
		} catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to push data: '{@data}'", trackData);
		}

		return false;
	}
}
