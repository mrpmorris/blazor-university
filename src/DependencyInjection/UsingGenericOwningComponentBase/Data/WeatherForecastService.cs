using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UsingGenericOwningComponentBase.Data
{
	public class WeatherForecastService
	{
		private volatile int Locked;

		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		public async Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
		{
			if (Interlocked.CompareExchange(ref Locked, 1, 0) != 0)
				throw new InvalidOperationException(
					"A second operation started on this context before a previous operation completed. Any "
					+ "instance members are not guaranteed to be thread-safe.");

			try
			{
				await Task.Delay(3000);
				var rng = new Random();
				return Enumerable.Range(1, 5).Select(index => new WeatherForecast
				{
					Date = startDate.AddDays(index),
					TemperatureC = rng.Next(-20, 55),
					Summary = Summaries[rng.Next(Summaries.Length)]
				}).ToArray();
			}
			finally
			{
				Interlocked.Decrement(ref Locked);
			}
		}
	}
}
