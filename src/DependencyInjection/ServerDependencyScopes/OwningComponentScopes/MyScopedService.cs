using System;

namespace OwningComponentScopes
{
	public interface IMyScopedService
	{
		public TimeSpan DeltaCreationTime { get; }
		public int InstanceNumber { get; }
		public IMyOtherScopedService MyOtherScopedService { get;  }
	}

	public sealed class MyScopedService : IMyScopedService, IDisposable
	{
		public TimeSpan DeltaCreationTime { get; }
		public int InstanceNumber { get; }
		public IMyOtherScopedService MyOtherScopedService { get;  }

		private static volatile int PreviousInstanceNumber;

		public MyScopedService(IMyOtherScopedService myOtherScopedService)
		{
			DeltaCreationTime = DateTime.UtcNow - AppLifetime.StartTimeUtc;
			InstanceNumber = System.Threading.Interlocked.Increment(ref PreviousInstanceNumber);
			MyOtherScopedService = myOtherScopedService;
		}

		void IDisposable.Dispose()
		{
			System.Diagnostics.Debug.WriteLine($"Disposed MyScopedService instance {InstanceNumber}");
		}
	}
}
