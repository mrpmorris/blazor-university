using Microsoft.JSInterop;

namespace CallingStaticDotNetMethods.Configuration
{
	public static class JavaScriptConfiguration
	{
		private static JavaScriptSettings Settings;

		internal static void SetSettings(JavaScriptSettings settings)
		{
			Settings = settings;
		}

		[JSInvokable("GetSettings")]
		public static JavaScriptSettings GetSettings() => Settings;
	}
}
