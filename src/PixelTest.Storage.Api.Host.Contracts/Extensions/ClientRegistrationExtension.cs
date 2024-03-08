using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PixelTest.Core.Exceptions;
using PixelTest.Storage.Api.Host.Contracts.Constants;
using PixelTest.Storage.Api.Host.Contracts.Interfaces;
using Refit;

namespace PixelTest.Storage.Api.Host.Contracts.Extensions;

public static  class ClientRegistrationExtension
{
	private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

	public static IServiceCollection AddStorageApiClient(this IServiceCollection self, IConfiguration config)
	{
		var url = config.GetRequiredSection(EnvironmentNames.StorageHost).Value;
		url.ThrowIfNullOrEmpty(EnvironmentNames.StorageHost);

		self.AddRefitClient<IStorageApiClient>()
			.ConfigureHttpClient(cfg =>
			{
				cfg.BaseAddress = new Uri(url!);
				cfg.Timeout = DefaultTimeout;
			});

		return self;
	}

	public static void ThrowIfNullOrEmpty(this string? str, string key)
	{
		if (string.IsNullOrEmpty(str))
		{
			throw new NullValueException(key);
		}
	}


}
