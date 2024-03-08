using System.Text.Json.Serialization;

namespace PixelTest.Storage.Api.Host.Contracts.Requests;

public class TrackDataRequest
{
	[JsonPropertyName("referrer")]
	public string? Referrer { get; set; }

	[JsonPropertyName("user_agent")]
	public string? UserAgent { get; set; }

	[JsonPropertyName("ip")]
	public string? IP { get; set; }
}
