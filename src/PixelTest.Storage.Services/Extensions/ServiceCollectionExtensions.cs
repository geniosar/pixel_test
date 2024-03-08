using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PixelTest.Storage.Services.Configurations;
using PixelTest.Storage.Services.Constants;
using PixelTest.Storage.Services.Contracts.Interfaces;
using PixelTest.Storage.Services.Contracts.Interfaces.Configurations;
using PixelTest.Storage.Services.Services;
using Serilog;

namespace PixelTest.Storage.Services.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddTrackDataSaver(this IServiceCollection self, IConfiguration configuration)
	{
		var path = configuration.GetValue<string>(AppEnvironmentNames.DataFilePath);
		if (string.IsNullOrEmpty(path))
		{
			Log.Error("App environment not found: {name}", AppEnvironmentNames.DataFilePath);
			throw new ArgumentException($"App environment not found: {AppEnvironmentNames.DataFilePath}");
		}

		self.AddSingleton<ITrackDataSaverConfiguration>(_ =>
			TrackDataSaverConfiguration.Create(path));
		self.AddSingleton<TrackDataSaver>();
		self.AddSingleton<ITrackDataSaverWriter>(x => x.GetRequiredService<TrackDataSaver>());
		self.AddSingleton<ITrackDataSaverWorker>(x => x.GetRequiredService<TrackDataSaver>());

		return self;
	}

	public static IServiceCollection AddBackgroundDataSaverWorkerConfiguration(this IServiceCollection self, IConfiguration configuration)
	{
		var delay = configuration.GetValue<int>(AppEnvironmentNames.SaveDataDelay);
		if (delay <= 0)
		{
			Log.Error("App environment not found: {name}", AppEnvironmentNames.SaveDataDelay);
			throw new ArgumentException($"App environment not found: {AppEnvironmentNames.SaveDataDelay}");
		}

		self.AddSingleton<IBackgroundDataSaverWorkerConfiguration>(_ =>
			BackgroundDataSaverWorkerConfiguration.Create(delay));

		return self;
	}

	public static IServiceCollection AddStorageService(this IServiceCollection self) =>
		self.AddTransient<IStorageService, StorageService>();
}
