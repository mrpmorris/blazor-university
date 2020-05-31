using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OwningComponentScopes
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");

			builder.Services.AddSingleton<IMySingletonService, MySingletonService>();
			builder.Services.AddScoped<IMyScopedService, MyScopedService>();
			builder.Services.AddScoped<IMyOtherScopedService, MyOtherScopedService>();
			builder.Services.AddTransient<IMyTransientService, MyTransientService>();

			await builder.Build().RunAsync();
		}
	}
}
