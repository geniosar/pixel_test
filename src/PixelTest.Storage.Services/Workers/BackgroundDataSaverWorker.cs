using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PixelTest.Storage.Services.Contracts.Interfaces;
using PixelTest.Storage.Services.Contracts.Interfaces.Configurations;

namespace PixelTest.Storage.Services.Workers;

public class BackgroundDataSaverWorker : BackgroundService
{
	private readonly ILogger _logger;
	private readonly ITrackDataSaverWorker _worker;
	private readonly IBackgroundDataSaverWorkerConfiguration _configuration;

	public BackgroundDataSaverWorker(ILogger<BackgroundDataSaverWorker> logger, 
		ITrackDataSaverWorker worker, 
		IBackgroundDataSaverWorkerConfiguration configuration)
	{
		_logger = logger;
		_worker = worker;
		_configuration = configuration;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("Start execute BackgroundDataSaverWorker");
		try
		{
			while (true)
			{
				await _worker.SaveDataAsync();

				try
				{
					await Task.Delay(TimeSpan.FromSeconds(_configuration.SaveDataDelay), stoppingToken);
				} catch (TaskCanceledException)
				{
					await _worker.SaveDataAsync();
				}
			}
		} catch (Exception ex)
		{
			_logger.LogError(ex, "Execute background process failed and stop working.");
		}
	}
}
