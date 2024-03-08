using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PixelTest.Storage.Services.Configurations;
using PixelTest.Storage.Services.Contracts.Interfaces;
using PixelTest.Storage.Services.Contracts.Models;
using PixelTest.Storage.Services.Services;

namespace PixelTest.Storage.Tests;

public class TrackDataSaverTests
{
	private ITrackDataSaverWorker _worker;
	private ITrackDataSaverWriter _writer;
	private readonly string _path = "test.data.txt";
	private readonly Fixture _fixture = new();

	[SetUp]
	public void Setup()
	{
		var dataSaver = new TrackDataSaver(Mock.Of<ILogger<TrackDataSaver>>(), TrackDataSaverConfiguration.Create(_path));
		_worker = dataSaver;
		_writer = dataSaver;
	}

	[Test]
	public async Task SaveData_ShouldntCreateFile_WhenDataIsNotExist()
	{
		await _worker.SaveDataAsync();

		var isExist = File.Exists(_path);

		isExist.Should().BeFalse();
	}

	[Test]
	public async Task SaveData_ShouldCreateFile_WhenDataIstExist()
	{
		var data = _fixture.Create<TrackDataModel>();
		await _writer.AddNewTrackAsync(data);
		await _worker.SaveDataAsync();

		var isExist = File.Exists(_path);

		isExist.Should().BeTrue();
	}

	[Test]
	public async Task SaveData_ShouldCreateFileWithCorrectData_WhenDataIstExist()
	{
		var data = _fixture.Create<TrackDataModel>();
		await _writer.AddNewTrackAsync(data);
		await _worker.SaveDataAsync();

		var isExist = File.Exists(_path);
		var row = isExist ? File.ReadAllText(_path) : null;
		var parts = Array.Empty<string>();
		if (row is not null)
		{
			parts = row.Split('|');
		}

		isExist.Should().BeTrue();
		row.Should().NotBeNullOrEmpty();
		parts.Count().Should().Be(4);
		parts[1].Should().Be(data.Referrer);
		parts[2].Should().Be(data.UserAgent);
		parts[3].Should().Be(data.IP);
	}

	[TearDown]
	public void TearDown()
	{
		if (File.Exists(_path))
		{
			File.Delete(_path);
		}
	}
}
