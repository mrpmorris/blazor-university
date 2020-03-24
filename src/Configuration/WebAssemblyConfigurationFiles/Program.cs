using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebAssemblyEmbeddedConfigurationFiles.ConfigurationObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace WebAssemblyEmbeddedConfigurationFiles
{
	public class Program
	{
		private static readonly Lazy<IServiceProvider> StartupConfigurationServiceProvider =
			new Lazy<IServiceProvider>(CreateStartupConfigurationServiceProvider);

		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);

			builder.Services.AddSomething(options =>
			{
				var settings =
					StartupConfigurationServiceProvider.Value
					.GetRequiredService<IOptions<ServiceSettings>>()
					 .Value; // IOptions<T>.Value
				options.Url = settings.Url;
				options.AccessToken = settings.AccessToken;
			});

			AddConfiguration(builder.Services);
			builder.RootComponents.Add<App>("app");

			builder.Services.AddBaseAddressHttpClient();

			await builder.Build().RunAsync();
		}

		private static void AddConfiguration(IServiceCollection services)
		{
			var embeddedFileProvider = new EmbeddedFileProvider(typeof(Program).Assembly);
			IConfiguration config = new ConfigurationBuilder()
				.AddJsonFile(
					path: "appsettings.json",
					optional: false,
					reloadOnChange: false,
					provider: embeddedFileProvider)
				.AddJsonFile(
					path: "appsettings.development.json",
					optional: true,
					reloadOnChange: false,
					provider: embeddedFileProvider)
				.Build();
			services.Configure<ServiceSettings>(result => 
				config
					.GetSection("ServiceSettings")
					.Bind(result));
		}

		private static IServiceProvider CreateStartupConfigurationServiceProvider()
		{
			var services = new ServiceCollection();
			AddConfiguration(services);
			return services.BuildServiceProvider();
		}
	}
}
