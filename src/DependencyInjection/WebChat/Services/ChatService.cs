using System;
using System.Collections.Concurrent;
using System.Linq;

namespace WebChat.Services
{
	public interface IChatService
	{
		bool SendMessage(string username, string message);
		string[] GetChatHistory();
		event EventHandler<string> TextAdded;
	}

	public class ChatService : IChatService
	{
		public event EventHandler<string> TextAdded;

		private ConcurrentQueue<string> ChatHistory = new ConcurrentQueue<string>();
		public string[] GetChatHistory() => ChatHistory.Reverse().Take(50).Reverse().ToArray();

		public bool SendMessage(string username, string message)
		{
			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(message))
				return false;

			string line = $"<{username}> {message}";
			TextAdded?.Invoke(null, line);

			ChatHistory.Enqueue(line);
			if (ChatHistory.Count > 100)
				RemoveHistory();

			return true;
		}


		private void RemoveHistory()
		{
			lock (ChatHistory)
			{
				while (ChatHistory.Count > 50)
					ChatHistory.TryDequeue(out _);
			}
		}
	}
}
