﻿using System;
using System.Threading.Tasks;

namespace Fritz.StreamTools.Services
{
	public interface IChatService
  {

		string Name { get; }

		event EventHandler<ChatMessageEventArgs> OnChatMessage;

		Task<bool> SendMessageAsync(string message);
		Task<bool> SendWhisperAsync(string userName, string message);
		Task<bool> TimeoutUserAsync(string userName, TimeSpan time);
		Task<bool> BanUserAsync(string userName);

	}
}