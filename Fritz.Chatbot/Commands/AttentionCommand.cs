using System;
using System.Text;
using System.Threading.Tasks;
using Fritz.StreamLib.Core;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fritz.Chatbot.Commands
{
	public class AttentionCommand : IBasicCommand
	{
		private readonly IConfiguration Configuration;
		private readonly IHubAccessor _hubAccessor;

		public ILogger Logger { get; }

		public AttentionCommand(IConfiguration configuration, ILoggerFactory loggerFactory, IHubAccessor hubAccessor)
		{
			this.Configuration = configuration;
			this.Logger = loggerFactory.CreateLogger("AttentionCommand");
			this._hubAccessor = hubAccessor;

			var thisUri = new Uri(configuration["FritzBot:ServerUrl"], UriKind.Absolute);
			var attentionUri = new Uri(thisUri, "attentionhub");

			Logger.LogTrace($"Connecting AttentionCommand to: {attentionUri}");
		}

		public string Trigger => "attention";

		public string Description => "Play audio queue to divert attention to chat";

	public TimeSpan? Cooldown => TimeSpan.Parse(Configuration["FritzBot:AttentionCommand:Cooldown"]);

		public async Task Execute(IChatService chatService, string userName, ReadOnlyMemory<char> rhs)
		{
			await _hubAccessor.AlertFritz();

			var attentionText = Configuration["FritzBot:AttentionCommand:TemplateText"];

			await chatService.SendMessageAsync(string.Format(attentionText, userName));
		}
	}
}
