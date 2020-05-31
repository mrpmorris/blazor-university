using System;

namespace OwningComponentScopes
{
	public interface IMyOtherScopedService
	{
		public TimeSpan DeltaCreationTime { get; }
		public int InstanceNumber { get; }
	}


	public sealed class MyOtherScopedService : IMyOtherScopedService, IDisposable
	{
		public TimeSpan DeltaCreationTime { get; }
		public int InstanceNumber { get; }

		private static volatile int PreviousInstanceNumber;

		public MyOtherScopedService()
		{
			DeltaCreationTime = DateTime.UtcNow - AppLifetime.StartTimeUtc;
			InstanceNumber = System.Threading.Interlocked.Increment(ref PreviousInstanceNumber);
		}

		void IDisposable.Dispose()
		{
			System.Diagnostics.Debug.WriteLine($"Disposed MyOtherScopedService instance {InstanceNumber}");
		}
	}

}
