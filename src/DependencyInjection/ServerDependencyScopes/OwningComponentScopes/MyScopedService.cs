using System;

namespace OwningComponentScopes
{
	public interface IMyScopedService
	{
		public TimeSpan DeltaCreationTime { get; }
		public int InstanceNumber { get; }
	}

	public sealed class MyScopedService : IMyScopedService, IDisposable
	{
		public TimeSpan DeltaCreationTime { get; }
		public int InstanceNumber { get; }

		private static volatile int PreviousInstanceNumber;

		public MyScopedService()
		{
			DeltaCreationTime = DateTime.UtcNow - AppLifetime.StartTimeUtc;
			InstanceNumber = System.Threading.Interlocked.Increment(ref PreviousInstanceNumber);
		}

		void IDisposable.Dispose()
		{
			System.Diagnostics.Debug.WriteLine($"Disposed MyScopedService instance {InstanceNumber}");
		}
	}
}
