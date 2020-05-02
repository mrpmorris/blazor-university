using System;

namespace DependencyScopes
{
	public interface IMyScopedService
	{
		public TimeSpan DeltaCreationTime { get; }
		public int InstanceNumber { get; }
	}

	public class MyScopedService : IMyScopedService
	{
		public TimeSpan DeltaCreationTime { get; }
		public int InstanceNumber { get; }

		private static volatile int PreviousInstanceNumber;

		public MyScopedService()
		{
			DeltaCreationTime = DateTime.UtcNow - AppLifetime.StartTimeUtc;
			InstanceNumber = System.Threading.Interlocked.Increment(ref PreviousInstanceNumber);
		}
	}
}
