using PixelTest.Storage.Api.Host.Contracts.Requests;
using Refit;

namespace PixelTest.Storage.Api.Host.Contracts.Interfaces;

[Headers("Accept: application/json")]
public interface IStorageApiClient
{
	[Post("/api/save")]
	Task<IApiResponse> SaveTrackAsync([Body] TrackDataRequest request, CancellationToken cancellationToken = default);
}
