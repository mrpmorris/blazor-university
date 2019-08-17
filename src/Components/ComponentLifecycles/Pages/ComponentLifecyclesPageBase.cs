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

		protected override void OnAfterRender()
		{
			Log("OnAfterRender()");
			base.OnAfterRender();
		}

		protected override Task OnAfterRenderAsync()
		{
			Log("OnAfterRenderAsyc()");
			return base.OnAfterRenderAsync();
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
