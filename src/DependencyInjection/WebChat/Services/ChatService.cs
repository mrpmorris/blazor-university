using System;
using System.Collections.Generic;
using System.Linq;

namespace WebChat.Services
{
	public interface IChatService
	{
		bool SendMessage(string username, string message);
		string ChatWindowText { get; }
		event EventHandler TextAdded;
	}

	public class ChatService : IChatService
	{
		public event EventHandler TextAdded;
		public string ChatWindowText { get; private set; }

		private readonly object SyncRoot = new object();
		private List<string> ChatHistory = new List<string>();

		public bool SendMessage(string username, string message)
		{
			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(message))
				return false;

			string line = $"<{username}> {message}";

			lock (SyncRoot)
			{
				ChatHistory.Add(line);
				while (ChatHistory.Count > 50)
					ChatHistory.RemoveAt(0);

				ChatWindowText = string.Join("\r\n", ChatHistory.Take(50));
			}

			TextAdded?.Invoke(this, EventArgs.Empty);
			return true;
		}
	}
}
