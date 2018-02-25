using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fritz.StreamTools.Services.Mixer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Test.Services.MixerService
{
  public class MixerLiveShould
  {
		[Theory]
		[AutoMoqData]
		public async Task NotifyOnNewFollowers(IConfiguration config, ILoggerFactory loggerFactory)
		{
			// Arrage
			var channelId = 9999;
			var jsonResponse1 = "{\"type\":\"event\",\"event\":\"hello\",\"data\":{\"authenticated\":true}}";
			var jsonOkReply = "{\"id\":1,\"type\":\"reply\",\"result\":null,\"error\":null}";

			var fakeJsonRpc = new Mock<IJsonRpcWebSocket>();
			fakeJsonRpc.Setup(x => x.TryConnectAsync(It.IsAny<Func<string>>(), It.IsAny<string>(), It.IsAny<Func<Task>>()))
				.Callback((Func<string> a, string b, Func<Task> callback) =>	{
					callback?.Invoke();
				})
				.ReturnsAsync(true);
			fakeJsonRpc.Setup(x => x.SendAsync("livesubscribe", It.IsAny<object[]>())).ReturnsAsync(true).Verifiable();

			var fakeFactory = new Mock<IMixerFactory>();
			fakeFactory.Setup(x => x.CreateJsonRpcWebSocket(It.IsAny<ILogger>(), false)).Returns(fakeJsonRpc.Object);

			// Act
			var sut = new MixerLive(config, loggerFactory, fakeFactory.Object, CancellationToken.None);
			await sut.ConnectAndJoinAsync(channelId);

			// Assert
			await Assert.RaisesAsync<EventEventArgs>(a => sut.LiveEvent += a, b => sut.LiveEvent -= b, () =>
			{
				fakeJsonRpc.Raise(x => x.EventReceived += null, new EventEventArgs { Event = "reply", Data = JToken.Parse(jsonOkReply) });
				return Task.CompletedTask;
			});

			Mock.Verify(fakeJsonRpc);
		}

	}
}
