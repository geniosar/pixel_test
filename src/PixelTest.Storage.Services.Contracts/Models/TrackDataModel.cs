namespace PixelTest.Storage.Services.Contracts.Models;

public class TrackDataModel
{
	public string? Referrer { get; set; }
	public string? UserAgent { get; set; }
	public string? IP { get; set; }
	public DateTime? CreatedAt { get; set; }
}
