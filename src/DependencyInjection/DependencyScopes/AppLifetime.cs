using System;

namespace DependencyScopes
{
	public static class AppLifetime
	{
		public static DateTime StartTimeUtc { get; } = DateTime.UtcNow;
		public static TimeSpan ElapsedTime => DateTime.UtcNow - StartTimeUtc;
	}
}
