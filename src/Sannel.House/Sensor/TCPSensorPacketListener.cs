/* Copyright 2018 Sannel Software, L.L.C.

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
using System.Collections.Specialized;
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

		public event EventHandler<SensorEntryReceivedEventArgs> EntriesReceived;

		public TCPSensorPacketListener(ILogger<TCPSensorPacketListener> logger)
		{
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public NameValueCollection Abbreviations { get; } = new NameValueCollection();

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
			=> Task.Run(() =>
			{
				try
				{
					logger.LogInformation("New client connected from {0}", client.Client.RemoteEndPoint);
					using (client)
					{
						client.ReceiveTimeout = 1000;
						client.ReceiveBufferSize = 88;
						using (var stream = client.GetStream())
						{
							logger.LogInformation("Have stream {0}", stream != null);
							ReadStream(stream);
						}
					}
				}
				catch (Exception ex)
				{
					logger.LogError(new EventId(), ex, "Exception while dealing with client");
				}
			});

		protected virtual bool ReadBytes(ref byte[] bits, int length, Stream stream)
		{
			var read = 0;
			var count = 0;
			var tryCount = 0;

			while (count < length && stream.CanRead && tryCount < 30)
			{
				if (tryCount > 0)
				{
					Task.Delay(200).GetAwaiter().GetResult();
				}
				read = stream.Read(bits, count, length - count);
				count += read;
				tryCount++;
			}

			return count == length;

		}

		protected virtual string FixAbbreviation(string value)
		{
			if (!string.IsNullOrWhiteSpace(Abbreviations[value]))
			{
				return Abbreviations[value];
			}

			return value;
		}

		protected virtual void ReadStream(Stream stream)
		{

			logger.LogInformation("Begin of ReadStream");
			if (stream == null)
			{
				throw new ArgumentNullException(nameof(stream));
			}

			var count = stream.ReadByte();

			logger.LogDebug("Incoming packages count {0}", count);

			var macBytes = new byte[]{
					0,0,0,0,0,0,0,0
				};

			if (!ReadBytes(ref macBytes, 6, stream))
			{
				logger.LogDebug("Mac address was not the correct length.");
				return;
			}

			var mac = BitConverter.ToInt64(macBytes, 0);

			logger.LogDebug("Mac address for packets {0}", mac);

			var args = new SensorEntryReceivedEventArgs
			{
				MacAddress = mac
			};

			for (var i = 0; i < count; i++)
			{
				var buffer = new byte[4];

				if (!ReadBytes(ref buffer, buffer.Length, stream))
				{
					logger.LogError("Invalid Packet from {0}", mac);
					return;
				}

				var entry = new RemoteSensorEntry();

				var t = BitConverter.ToUInt32(buffer, 0);

				entry.MillisOffset = t;

				if (!ReadBytes(ref buffer, buffer.Length, stream))
				{
					logger.LogError("Invalid Packet from {0}", mac);
					return;
				}

				t = BitConverter.ToUInt32(buffer, 0);

				if (Enum.IsDefined(typeof(SensorTypes), t))
				{
					entry.SensorType = ((SensorTypes)t).ToString();
				}
				else
				{
					entry.SensorType = t.ToString();
				}


				if (!ReadBytes(ref buffer, 1, stream))
				{
					logger.LogError("Invalid Packet from {0}", mac);
					return;
				}

				var cc = buffer[0];

				for (byte b = 0; b < cc; b++)
				{
					if (!ReadBytes(ref buffer, buffer.Length, stream))
					{
						logger.LogError("Invalid Packet from {0}", mac);
						return;
					}

					var property = Encoding.ASCII.GetString(buffer);

					property = FixAbbreviation(property);

					if (!ReadBytes(ref buffer, buffer.Length, stream))
					{
						logger.LogError("Invalid Packet from {0}", mac);
						return;
					}

					var value = BitConverter.ToSingle(buffer, 0);
					entry.Values[property] = value;
				}


				logger.LogDebug("Filled Packet {0}", entry);
				args.Entries.Add(entry);
			}

			FirePacketReceived(args);
		}

		protected virtual void FirePacketReceived(SensorEntryReceivedEventArgs args)
		{
			logger.LogInformation("Firing PackateReceived");
			EntriesReceived?.Invoke(this, args);
		}

		public void Dispose()
		{
			running = false;
			listener?.Stop();
		}
	}
}
