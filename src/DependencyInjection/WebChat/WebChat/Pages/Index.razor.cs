using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebChat.Services;

namespace WebChat.Pages
{
	public partial class Index : IDisposable
	{
		[Inject]
		private IChatService ChatService { get; set; }

		[MinLength(1, ErrorMessage = "Required")]
		private string Name;
		[MinLength(1, ErrorMessage = "Required")]
		private string Text;

		private string[] ChatHistory = Array.Empty<string>();
		private string ChatHistoryText => string.Join('\r', ChatHistory);

		protected override void OnInitialized()
		{
			base.OnInitialized();
			ChatHistory = ChatService.GetChatHistory();
			ChatService.TextAdded += TextAdded;
		}

		private void SendMessage()
		{
			if (ChatService.SendMessage(Name, Text))
				Text = "";
		}

		private void TextAdded(object sender, string line)
		{
			ChatHistory = ChatHistory.Take(49).Append(line).ToArray();
			InvokeAsync(StateHasChanged);
		}

		void IDisposable.Dispose()
		{
			ChatService.TextAdded -= TextAdded;
		}
	}
}
