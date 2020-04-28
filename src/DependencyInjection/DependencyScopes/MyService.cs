using System;

namespace DependencyScopes
{
	public interface IMySingletonService
	{
		public TimeSpan CreatedAfter { get; }
	}

	public interface IMyScopedService
	{
		public TimeSpan CreatedAfter { get; }
	}

	public interface IMyTransientService
	{
		public TimeSpan CreatedAfter { get; }
	}

	public class MyService : IMySingletonService, IMyScopedService, IMyTransientService
	{
		public TimeSpan CreatedAfter { get; }

		public MyService()
		{
			CreatedAfter = DateTime.UtcNow - AppLifetime.StartTimeUtc;
			System.Threading.Thread.Sleep(100);
		}
	}
}
