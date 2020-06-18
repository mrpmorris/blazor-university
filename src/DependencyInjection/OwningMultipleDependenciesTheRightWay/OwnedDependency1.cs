using System;

namespace OwningMultipleDependenciesTheRightWay
{
	public interface IOwnedDependency1
	{
		public int InstanceNumber { get; }
	}


	public class OwnedDependency1 : IOwnedDependency1, IDisposable
	{
		private static volatile int PreviousInstanceNumber;

		public int InstanceNumber { get; }
		public OwnedDependency1()
		{
			InstanceNumber = System.Threading.Interlocked.Increment(ref PreviousInstanceNumber);
		}

		public void Dispose()
		{
			System.Diagnostics.Debug.WriteLine("Disposing " + GetType().Name);
		}
	}
}
