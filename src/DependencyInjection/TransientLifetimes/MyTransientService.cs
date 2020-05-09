namespace TransientLifetimes
{
	public interface IMyTransientService
	{
		public int InstanceNumber { get; }
	}

	public sealed class MyTransientService : IMyTransientService
	{
		public int InstanceNumber { get; }

		private static volatile int PreviousInstanceNumber;

		public MyTransientService()
		{
			InstanceNumber = System.Threading.Interlocked.Increment(ref PreviousInstanceNumber);
		}
	}
}
