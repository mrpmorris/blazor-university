using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Diagnostics;

namespace NavigatingViaCode.Pages
{
	public class CounterBase : ComponentBase, IDisposable
	{
		// Inject an instance of the IUriHelper
		[Inject] protected IUriHelper UriHelper { get; set; }

		protected override void OnInit()
		{
			// Subscribe to the event
			UriHelper.OnLocationChanged += LocationChanged;
			base.OnInit();
		}

		public void Dispose()
		{
			// Unsubscribe from the event when our component is disposed
			UriHelper.OnLocationChanged -= LocationChanged;
		}

		private void LocationChanged(object sender, LocationChangedEventArgs e)
		{
			string navigationMethod = e.IsNavigationIntercepted ? "HTML" : "code";
			Debug.WriteLine($"Notified of navigation via {navigationMethod} to {e.Location}");
		}
	}
}
