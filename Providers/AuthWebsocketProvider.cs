using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using Zenject;

namespace BSChallenger.Providers
{
	internal class AuthWebsocketProvider : IDisposable
	{
		private WebSocketServer server;
		private static AuthWebsocketProvider _instance;
		internal bool started = false;
		public void Dispose()
		{
			started = false;
			server.Stop();
		}

		public void Initialize()
		{
			_instance = this;

			server = new WebSocketServer("ws://localhost:3000");
			server.AddWebSocketService<SocketBehavior>("/");
			server.Start();
			started = true;
		}

		private class SocketBehavior : WebSocketBehavior
		{
			protected override void OnMessage(MessageEventArgs e)
			{
				Console.WriteLine(e.Data);
				var msg = e.Data == "BALUS"
						  ? "Are you kidding?"
						  : "I'm not available now.";

				Send(msg);
			}
		}
	}
}
