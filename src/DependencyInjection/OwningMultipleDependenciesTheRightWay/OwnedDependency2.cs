namespace OwningMultipleDependenciesTheRightWay
{
	public interface IOwnedDependency2
	{
		public int InstanceNumber { get; }
	}


	public class OwnedDependency2 : IOwnedDependency2
	{
		private static volatile int PreviousInstanceNumber;

		public int InstanceNumber { get; }
		public OwnedDependency2()
		{
			InstanceNumber = System.Threading.Interlocked.Increment(ref PreviousInstanceNumber);
		}
	}
}
