using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Fritz.StreamLib.Core;
using Fritz.StreamTools.Hubs;

namespace Fritz.StreamTools.Services
{
	public class HubAccessor : IHubAccessor
	{
		private readonly IHubContext<AttentionHub> _attentionHub;

		public HubAccessor(IHubContext<AttentionHub> attentionHub)
		{
			_attentionHub = attentionHub;
		}
		public Task AlertFritz() => _attentionHub.Clients.All.SendAsync("AlertFritz");
	}
}
