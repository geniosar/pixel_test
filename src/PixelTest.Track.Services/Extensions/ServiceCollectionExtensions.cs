using Microsoft.Extensions.DependencyInjection;
using PixelTest.Track.Services.Contracts.Interfaces;
using PixelTest.Track.Services.Services;

namespace PixelTest.Track.Services.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddTrackService(this IServiceCollection self) =>
		self.AddTransient<ITrackService, TrackService>();
}
