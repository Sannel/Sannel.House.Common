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
		protected TcpListener listener;

		public event EventHandler<SensorPacketReceivedEventArgs> PacketReceived;

		public async void Begin(uint port)
		{
			listener = new TcpListener(new IPEndPoint(IPAddress.Any, (int)port));

			listener.Start();

			while(true)
			{
				var client = await listener.AcceptTcpClientAsync();
				if (client != null)
				{
					Task.Run(() => ProcessClient(client));
				}
			}
		}

		protected virtual void ProcessClient(TcpClient client)
		{
			using (client)
			{
				using (var stream = client.GetStream())
				{
				}
			}
		}

		public void Dispose()
		{
			listener.Stop();
		}
	}
}
