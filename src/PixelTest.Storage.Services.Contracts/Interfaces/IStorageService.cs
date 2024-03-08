using PixelTest.Storage.Services.Contracts.Models;

namespace PixelTest.Storage.Services.Contracts.Interfaces;

public interface IStorageService
{
	Task<bool> SaveTrackAsync(TrackDataModel trackData, CancellationToken cancellationToken);
}
