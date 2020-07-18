using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ComponentLifecycles.Pages
{
	public class EmbeddedComponent : ComponentBase, IDisposable
	{
		private volatile int IndentSize;
		public EmbeddedComponent()
		{
			Log("Component created");
			IncreaseIndent();
		}

		[Parameter]
		public int Count { get; set; }

		private void IncreaseIndent() => Interlocked.Increment(ref IndentSize);
		private void DecreaseIndent() => Interlocked.Decrement(ref IndentSize);

		public override async Task SetParametersAsync(ParameterView parameters)
		{
			Log("SetParametersAsync() started");
			IncreaseIndent();
			await base.SetParametersAsync(parameters);
			Log("SetParametersAsync() base called");
			DecreaseIndent();
			Log("SetParametersAsync() finished");
		}

		protected override void OnInitialized()
		{
			Log("OnInitialized() started");
			IncreaseIndent();
			base.OnInitialized();
			DecreaseIndent();
			Log("OnInitialized() finished");
		}

		protected override async Task OnInitializedAsync()
		{
			Log("OnInitializedAsync() started");
			IncreaseIndent();
			await base.OnInitializedAsync();
			Log("OnInitializedAsync() base called");
			DecreaseIndent();
			Log("OnInitializedAsync() finished");
		}

		protected override void OnParametersSet()
		{
			Log("OnParametersSet() started");
			IncreaseIndent();
			base.OnParametersSet();
			DecreaseIndent();
			Log("OnParametersSet() finished");
		}

		protected override async Task OnParametersSetAsync()
		{
			Log("OnParametersSetAsync() started");
			IncreaseIndent();
			await base.OnParametersSetAsync();
			Log("OnParametersSetAsync() base called");
			DecreaseIndent();
			Log("OnParametersSetAsync() finished");
		}

		protected override void OnAfterRender(bool firstRender)
		{
			Log($"OnAfterRender({firstRender}) started");
			IncreaseIndent();
			base.OnAfterRender(firstRender);
			DecreaseIndent();
			Log($"OnAfterRender({firstRender}) finished");
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			Log($"OnAfterRenderAsync({firstRender}) started");
			IncreaseIndent();
			await base.OnParametersSetAsync();
			Log($"OnAfterRenderAsync({firstRender}) base called");
			DecreaseIndent();
			Log($"OnAfterRenderAsync({firstRender}) finished");
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			Log("BuildRenderTree started");
			Log("BuildRenderTree finished");
		}

		public virtual void Dispose()
		{
			Log("Disposed");
		}

		protected override bool ShouldRender()
		{
			Log("ShouldRender");
			return base.ShouldRender();
		}

		protected void Log(string text, bool includeTypeName = true)
		{
			// ALT + 195 = ├
			// ALT + 196 = ─
			string typeName = !includeTypeName ? null : GetType().Name + ": ";
			string indentText = !includeTypeName ? null : string.Join("", Enumerable.Repeat("├──", IndentSize));
			Debug.WriteLine($"{DateTime.UtcNow.ToString("HH:mm:ss.fff")}> {indentText}{typeName}{text}");
		}
	}
}
