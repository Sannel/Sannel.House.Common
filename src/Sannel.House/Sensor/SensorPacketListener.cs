using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sannel.House.Sensor
{
	public class SensorPacketListener : IDisposable
	{
		protected UdpClient client;

		public event EventHandler<SensorPacketReceivedEventArgs> PacketReceived;

		public async void Begin(uint port)
		{
			client?.Dispose();
			var broadcastAddress = new IPEndPoint(IPAddress.Any, (int)port);
			client = new UdpClient();
			client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

			client.Client.Bind(broadcastAddress);

			UdpReceiveResult result;
			while((result = await client.ReceiveAsync()) != null)
			{
				var packet = new SensorPacket();
				packet.Fill(result.Buffer);
				try
				{
					PacketReceived?.Invoke(this, new SensorPacketReceivedEventArgs()
					{
						Packet = packet
					});
				}
				catch(Exception)
				{

				}
			}
		}

		public void Dispose()
		{
			client?.Dispose();
		}
	}
}
