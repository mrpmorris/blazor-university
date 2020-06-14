namespace OwningMultipleDependenciesTheWrongWay
{
	public interface IInjectedDependency
	{
		public int InstanceNumber { get; }
	}


	public class InjectedDependency : IInjectedDependency
	{
		private static volatile int PreviousInstanceNumber;

		public int InstanceNumber { get; }
		public InjectedDependency()
		{
			InstanceNumber = System.Threading.Interlocked.Increment(ref PreviousInstanceNumber);
		}
	}
}
