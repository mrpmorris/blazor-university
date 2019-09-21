using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using CustomValidation.Components;

namespace CustomValidation
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddValidatorsInAssembly(typeof(Startup).Assembly);
		}

		public void Configure(IComponentsApplicationBuilder app)
		{
			app.AddComponent<App>("app");
		}
	}
}
