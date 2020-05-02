using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.ComponentModel.DataAnnotations;
using WebChat.Services;

namespace WebChat.Pages
{
	public partial class Index : IDisposable
	{
		[Required(ErrorMessage = "Enter name")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Enter a message")]
		public string Text { get; set; }

		[Inject]
		private IChatService ChatService { get; set; }
		[Inject]
		private IJSRuntime JSRuntime { get; set; }

		private string ChatWindowText => ChatService.ChatWindowText;
		private ElementReference ChatWindow;

		protected override void OnInitialized()
		{
			base.OnInitialized();
			ChatService.TextAdded += TextAdded;
		}

		protected override void OnAfterRender(bool firstRender)
		{
			base.OnAfterRender(firstRender);
			//JSRuntime.InvokeVoidAsync("BlazorUniversity.scrollToBottom", ChatWindow);
		}

		private void SendMessage()
		{
			if (ChatService.SendMessage(Name, Text))
				Text = "";
		}

		private void TextAdded(object sender, EventArgs e)
		{
			InvokeAsync(StateHasChanged);
		}

		void IDisposable.Dispose()
		{
			ChatService.TextAdded -= TextAdded;
		}
	}
}
