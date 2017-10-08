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
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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
		protected ILogger<TCPSensorPacketListener> logger;

		public event EventHandler<SensorPacketsReceivedEventArgs> PacketReceived;

		public TCPSensorPacketListener(ILogger<TCPSensorPacketListener> logger)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async void Begin(uint port)
		{
			running = true;
			listener = new TcpListener(new IPEndPoint(IPAddress.Any, (int)port));

			logger.LogInformation("Starting TcpListener on port {0}", port);
			listener.Start();

			while (running)
			{
				var client = await listener.AcceptTcpClientAsync();
				if (client != null)
				{
					ProcessClient(client);
				}
			}
		}

		protected virtual void ProcessClient(TcpClient client) =>
			Task.Run(async () =>
			{
				logger.LogInformation("New client connected from {0}", client.Client.RemoteEndPoint);
				using (client)
				{
					using (var stream = client.GetStream())
					{
						await ReadStreamAsync(stream);
					}
				}
			});

		protected virtual async Task<SensorPacketsReceivedEventArgs> ReadStreamAsync(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream));
			}

			var count = stream.ReadByte();

			logger.LogDebug("Incoming packages count {0}", count);

			var macBytes = new byte[]{
				0,0,0,0,0,0,0,0
			};

			var read = await stream.ReadAsync(macBytes, 0, 6);
			if (read != 6) // if its less then a mac address exit
			{
				return null;
			}

			var mac = BitConverter.ToInt64(macBytes, 0);

			logger.LogDebug("Mac address for packets {0}", mac);

			var args = new SensorPacketsReceivedEventArgs
			{
				MacAddress = mac
			};

			for (var i = 0; i < count; i++)
			{
				var buffer = new byte[88];
				var index = 0;
				while (index < buffer.Length)
				{
					read = await stream.ReadAsync(buffer, index, buffer.Length - index);
					logger.LogDebug("Read {0} bytes from stream", read);
					index += read;
				}

				var packet = new SensorPacket();
				packet.Fill(buffer);
				logger.LogDebug("Filled Packet {0}", packet);
				args.Packets.Add(packet);
			}

			FirePacketReceived(args);
			return args;
		}

		protected virtual void FirePacketReceived(SensorPacketsReceivedEventArgs args)
		{
			logger.LogInformation("Firing PackateReceived");
			PacketReceived?.Invoke(this, args);
		}

		public void Dispose()
		{
			running = false;
			listener?.Stop();
		}
	}
}
