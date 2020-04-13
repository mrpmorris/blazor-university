using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
