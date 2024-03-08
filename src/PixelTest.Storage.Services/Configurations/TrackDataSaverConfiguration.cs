using PixelTest.Storage.Services.Contracts.Interfaces.Configurations;

namespace PixelTest.Storage.Services.Configurations;

internal class TrackDataSaverConfiguration : ITrackDataSaverConfiguration
{
	public string? Path { get; }

	public static TrackDataSaverConfiguration Create(string? path) =>
		new(path);

	private TrackDataSaverConfiguration(string? path)
	{
		Path = path;
	}
}
