using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ComponentLifecycles.Pages
{
	public class ComponentLifecyclesPageBase : ComponentBase, IDisposable
	{
		public ComponentLifecyclesPageBase()
		{
			Log("Component created");
		}

		public override async Task SetParametersAsync(ParameterView parameters)
		{
			Log("SetParametersAsync() started");
			await base.SetParametersAsync(parameters);
			Log("SetParametersAsync() base called");
			await Task.Delay(100);
			Log("SetParametersAsync() end wait 1");
			await Task.Delay(100);
			Log("SetParametersAsync() end wait 2");
			Log("SetParametersAsync() finished");
		}

		protected override void OnInitialized()
		{
			Log("OnInitialized() started");
			base.OnInitialized();
			Log("OnInitialized() finished");
		}

		protected override async Task OnInitializedAsync()
		{
			Log("OnInitializedAsync() started");
			await base.OnInitializedAsync();
			Log("OnInitializedAsync() base called");
			await Task.Delay(100);
			Log("OnInitializedAsync() end wait 1");
			await Task.Delay(100);
			Log("OnInitializedAsync() end wait 2");
			Log("OnInitializedAsync() finished");
		}

		protected override void OnParametersSet()
		{
			Log("OnParametersSet() started");
			base.OnParametersSet();
			Log("OnParametersSet() finished");
		}

		protected override async Task OnParametersSetAsync()
		{
			Log("OnParametersSetAsync() started");
			await base.OnParametersSetAsync();
			Log("OnParametersSetAsync() base called");
			await Task.Delay(100);
			Log("OnParametersSetAsync() end wait 1");
			await Task.Delay(100);
			Log("OnParametersSetAsync() end wait 2");
			Log("OnParametersSetAsync() finished");
		}

		protected override void OnAfterRender(bool firstRender)
		{
			Log($"OnAfterRender({firstRender}) started");
			base.OnAfterRender(firstRender);
			Log($"OnAfterRender({firstRender}) finished");
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			Log($"OnAfterRenderAsync({firstRender}) started");
			await base.OnParametersSetAsync();
			Log($"OnAfterRenderAsync({firstRender}) base called");
			await Task.Delay(100);
			Log($"OnAfterRenderAsync({firstRender}) end wait 1");
			await Task.Delay(100);
			Log($"OnAfterRenderAsync({firstRender}) end wait 2");
			Log($"OnAfterRenderAsync({firstRender}) finished");
		}


		public virtual void Dispose()
		{
			Log("Disposed");
		}

		protected void Log(string text)
		{
			Debug.WriteLine($"{GetType().Name}: {text}");
		}
	}
}
