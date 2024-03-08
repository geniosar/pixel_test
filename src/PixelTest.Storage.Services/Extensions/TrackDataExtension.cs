using PixelTest.Storage.Services.Contracts.Models;
using System.Globalization;

namespace PixelTest.Storage.Services.Extensions;

internal static class TrackDataExtension
{
	internal static string ToMessage(this TrackDataModel trackData) =>
		$"{trackData.CreatedAt?.ToString("o", CultureInfo.InvariantCulture)}|{trackData.Referrer??"null"}|{trackData.UserAgent??"null"}|{trackData.IP}";
}
