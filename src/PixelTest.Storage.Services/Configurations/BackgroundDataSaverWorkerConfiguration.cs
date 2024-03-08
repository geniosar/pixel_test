using PixelTest.Storage.Services.Contracts.Interfaces.Configurations;

namespace PixelTest.Storage.Services.Configurations
{
	internal class BackgroundDataSaverWorkerConfiguration : IBackgroundDataSaverWorkerConfiguration
	{
		public int SaveDataDelay { get; }

		public static BackgroundDataSaverWorkerConfiguration Create(int delay) =>
			new(delay);

		private BackgroundDataSaverWorkerConfiguration(int delay)
		{
			SaveDataDelay = delay;
		}
	}
}
