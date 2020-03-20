using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebAssemblyConfigurationFiles
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddSomething(this IServiceCollection services, Action<Options> options)
		{
			var optionsInstance = new Options();
			options(optionsInstance);

			if (string.IsNullOrEmpty(optionsInstance.Url))
				throw new NullReferenceException(nameof(optionsInstance.Url));
			if (string.IsNullOrEmpty(optionsInstance.AccessToken))
				throw new NullReferenceException(nameof(optionsInstance.AccessToken));

			System.Diagnostics.Debug.WriteLine("URL " + optionsInstance.Url);
			System.Diagnostics.Debug.WriteLine("AccessToken " + optionsInstance.AccessToken);

			return services;
		}
	}

	public class Options
	{
		public string Url { get; set; }
		public string AccessToken { get; set; }
	}
}
