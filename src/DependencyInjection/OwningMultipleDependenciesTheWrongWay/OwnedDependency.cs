namespace OwningMultipleDependenciesTheWrongWay
{
	public interface IOwnedDependency
	{
		public int InstanceNumber { get; }
	}


	public class OwnedDependency : IOwnedDependency
	{
		private static volatile int PreviousInstanceNumber;

		public int InstanceNumber { get; }
		public OwnedDependency()
		{
			InstanceNumber = System.Threading.Interlocked.Increment(ref PreviousInstanceNumber);
		}
	}
}
