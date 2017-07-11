/* Copyright 2017 Sannel Software, L.L.C.

   Licensed under the Apache License, Version 2.0 (the ""License"");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

	   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an ""AS IS"" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.*/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Sensor
{
	public class TCPSensorPacketListener : IDisposable
	{
		protected bool running = false;
		protected TcpListener listener;

		public event EventHandler<SensorPacketReceivedEventArgs> PacketReceived;

		public async void Begin(uint port)
		{
			running = true;
			listener = new TcpListener(new IPEndPoint(IPAddress.Any, (int)port));

			listener.Start();

			while(running)
			{
				var client = await listener.AcceptTcpClientAsync();
				if (client != null)
				{
					ProcessClient(client);
				}
			}
		}

		protected virtual async void ProcessClient(TcpClient client)
		{
			await Task.Delay(0); // let the client accept another connection
			using (client)
			{
				using (var stream = client.GetStream())
				{
					var count = stream.ReadByte();
					for(var i = 0; i < count; i++)
					{
						var buffer = new byte[84];
						var read = await stream.ReadAsync(buffer, 0, buffer.Length);
						var packet = new SensorPacket();
						packet.Fill(buffer);
						try
						{
							PacketReceived?.Invoke(this, new SensorPacketReceivedEventArgs()
							{
								Packet = packet
							});
						}
						catch
						{ }
					}
				}
			}
		}

		public void Dispose()
		{
			running = false;
			listener?.Stop();
		}
	}
}
