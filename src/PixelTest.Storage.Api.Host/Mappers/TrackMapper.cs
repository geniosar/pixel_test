using PixelTest.Storage.Api.Host.Contracts.Requests;
using PixelTest.Storage.Services.Contracts.Models;

namespace PixelTest.Storage.Api.Host.Mappers;

internal static class TrackMapper
{
	internal static TrackDataModel ToModel(this TrackDataRequest request) =>
		new()
		{
			IP = request.IP,
			Referrer = request.Referrer,
			UserAgent = request.UserAgent,
			CreatedAt = DateTime.UtcNow
		};
}
