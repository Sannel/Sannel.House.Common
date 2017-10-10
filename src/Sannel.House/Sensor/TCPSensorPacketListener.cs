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

		protected virtual void ProcessClient(TcpClient client)
		{
			Task.Run(() =>
			{
				try
				{
					logger.LogInformation("New client connected from {0}", client.Client.RemoteEndPoint);
					using (client)
					{
						client.ReceiveBufferSize = 88;
						using (var stream = client.GetStream())
						{
							logger.LogInformation("Have stream {0}", stream != null);
							ReadStream(stream);
						}
					}
				}
				catch(Exception ex)
				{
					logger.LogError(new EventId(), ex, "Exception while dealing with client");
				}
			});
		}

		protected virtual bool ReadBytes(ref byte[] bits, int length, Stream stream)
		{
			var read = 0;
			var count = 0;

			while(count < length && stream.CanRead)
			{
				read = stream.Read(bits, count, length - count);
				count += read;
			}

			return count == length;

		}

		protected virtual SensorPacketsReceivedEventArgs ReadStream(Stream stream)
		{

			logger.LogInformation("Begin of ReadStreamAsync");
			if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream));
			}

			var count = stream.ReadByte();

			logger.LogDebug("Incoming packages count {0}", count);

			var macBytes = new byte[]{
					0,0,0,0,0,0,0,0
				};

			if(!ReadBytes(ref macBytes, 6, stream))
			{
				logger.LogDebug("Mac address was not the correct length.");
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
				for(var k = 0; k < 8; k++)
				{
					buffer[k] = 0;
				}
				for(var b = 9;b < buffer.Length; b++)
				{
					buffer[b] = 255;
				}

				ReadBytes(ref buffer, buffer.Length, stream);

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
