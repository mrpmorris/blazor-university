using System;

namespace DependencyScopes
{
	public interface IMyTransientService
	{
		public TimeSpan DeltaCreationTime { get; }
		public int InstanceNumber { get; }
	}

	public class MyTransientService : IMyTransientService
	{
		public TimeSpan DeltaCreationTime { get; }
		public int InstanceNumber { get; }

		private static volatile int PreviousInstanceNumber;

		public MyTransientService()
		{
			DeltaCreationTime = DateTime.UtcNow - AppLifetime.StartTimeUtc;
			InstanceNumber = System.Threading.Interlocked.Increment(ref PreviousInstanceNumber);
		}
	}
}
