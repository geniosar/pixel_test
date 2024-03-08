using PixelTest.Track.Services.Contracts.Models;

namespace PixelTest.Track.Services.Contracts.Interfaces;

public interface ITrackService
{
	Task<bool> TrackDataAsync(TrackDataModel trackData, CancellationToken cancellationToken);
}
