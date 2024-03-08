using Microsoft.Extensions.Logging;
using PixelTest.Core.Common;
using PixelTest.Storage.Services.Contracts.Interfaces;
using PixelTest.Storage.Services.Contracts.Interfaces.Configurations;
using PixelTest.Storage.Services.Contracts.Models;
using PixelTest.Storage.Services.Extensions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PixelTest.Storage.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]
namespace PixelTest.Storage.Services.Services;

internal class TrackDataSaver : ITrackDataSaverWriter, ITrackDataSaverWorker
{
	private List<TrackDataModel> _writer { get; set; } = new();
	private List<TrackDataModel> _reader { get; set; } = new();
	private AsyncLocker _locker = new();

	private readonly ILogger _logger;
	private readonly ITrackDataSaverConfiguration _configuration;

	public TrackDataSaver(ILogger<TrackDataSaver> logger, ITrackDataSaverConfiguration configuration)
	{
		_logger = logger;
		_configuration = configuration;
	}

	public async Task AddNewTrackAsync(TrackDataModel track)
	{
		try
		{
			using (await _locker.LockAsync())
			{
				_writer.Add(track);
			}
		} catch (Exception ex)
		{
			_logger.LogError(ex, "Failed add new track data {@data}", track);
		}
	}

	public async Task SaveDataAsync()
	{
		try
		{
			using (await _locker.LockAsync())
			{
				if (_writer.Count > 0)
				{
					_reader.Clear();
					_reader.AddRange(_writer);
					_writer.Clear();
				}
			}
			if (_reader.Count > 0)
			{
				File.AppendAllText(_configuration.Path!, Environment.NewLine + string.Join(Environment.NewLine, _reader.Select(x => x.ToMessage())));
				_reader.Clear();
			}

		} catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to save data");
		}
		finally
		{
			using (await _locker.LockAsync())
			{
				_writer.AddRange(_reader);
			}
		}
	}
}
