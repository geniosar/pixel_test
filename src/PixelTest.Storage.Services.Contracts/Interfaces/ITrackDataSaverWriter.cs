using PixelTest.Storage.Services.Contracts.Models;

namespace PixelTest.Storage.Services.Contracts.Interfaces;

public interface ITrackDataSaverWriter
{
	Task AddNewTrackAsync(TrackDataModel track);
}
