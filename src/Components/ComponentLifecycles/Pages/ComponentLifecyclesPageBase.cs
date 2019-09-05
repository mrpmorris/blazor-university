using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ComponentLifecycles.Pages
{
	public class ComponentLifecyclesPageBase : ComponentBase, IDisposable
	{
		public ComponentLifecyclesPageBase()
		{
			Log("Component created");
		}

		public override Task SetParametersAsync(ParameterView parameters)
		{
			Log("SetParametersAsync()");
			return base.SetParametersAsync(parameters);
		}

		protected override void OnInitialized()
		{
			Log("OnInitialized()");
			base.OnInitialized();
		}

		protected override Task OnInitializedAsync()
		{
			Log("OnInitializedAsync()");
			return base.OnInitializedAsync();
		}

		protected override void OnParametersSet()
		{
			Log("OnParametersSet()");
			base.OnParametersSet();
		}

		protected override Task OnParametersSetAsync()
		{
			Log("OnParametersSetAsync()");
			return base.OnParametersSetAsync();
		}

		protected override void OnAfterRender(bool firstRender)
		{
			Log($"OnAfterRender({firstRender})");
			base.OnAfterRender(firstRender);
		}

		protected override Task OnAfterRenderAsync(bool firstRender)
		{
			Log($"OnAfterRenderAsyc({firstRender})");
			return base.OnAfterRenderAsync(firstRender);
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
