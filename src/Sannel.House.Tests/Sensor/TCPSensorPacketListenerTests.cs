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
using Sannel.House.Sensor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sannel.House.Tests.Sensor
{
	public class TCPSensorPacketListenerTests
	{
		//[Fact]
		//public async Task Begin()
		//{
		//	using(var listener = new UDPSensorPacketListener())
		//	{
		//		listener.Begin(8172);

		//		await Task.Delay(50000000);
		//	}
		//}

		[Fact]
		public async Task ReadStreamAsyncTest()
		{
			using (var listener = new TCPSensorPacketListenerMock())
			{
				var called = false;

				await Assert.ThrowsAsync<ArgumentNullException>("stream", () =>
				{
					return listener.ReadStreamAsyncWrapper(null);
				});

				var mstream = new MemoryStream();

				var m = new EventHandler<SensorPacketsReceivedEventArgs>((o, a) =>
				{
					called = true;
				});

				listener.PacketReceived += m;

				await listener.ReadStreamAsyncWrapper(mstream);

				Assert.False(called);

				var bits = new byte[4];

				mstream.WriteByte(1);
				mstream.Write(bits, 0, 4);
				mstream.Seek(0, SeekOrigin.Begin);

				await listener.ReadStreamAsyncWrapper(mstream);

				Assert.False(called);

				listener.PacketReceived -= m;

				called = false;

				mstream.Seek(0, SeekOrigin.Begin);
				mstream.WriteByte(1);

				bits = BitConverter.GetBytes((long)int.MaxValue);
				mstream.Write(bits, 0, 6);

				var expectedType = SensorTypes.SoilMoisture;
				var offset = 30u;

				m = new EventHandler<SensorPacketsReceivedEventArgs>((o, a) =>
				{
					called = true;
					Assert.Equal(a.MacAddress, int.MaxValue);
					Assert.Equal(1, a.Packets.Count);

					var packet = a.Packets[0];
					Assert.NotNull(packet);
					Assert.Equal(expectedType, packet.SensorType);
					Assert.Equal(offset, packet.MillisOffset);
					Assert.Equal(10, packet.Values.Length);
					for(var i = 0; i < 10; i++)
					{
						Assert.Equal(i + 1, packet.Values[i]);
					}
				});

				listener.PacketReceived += m;

				bits = BitConverter.GetBytes((int)expectedType);
				mstream.Write(bits, 0, bits.Length);

				bits = BitConverter.GetBytes(offset);
				mstream.Write(bits, 0, bits.Length);

				for(var i=0;i<10;i++)
				{
					bits = BitConverter.GetBytes((double)i + 1);
					mstream.Write(bits, 0, bits.Length);
				}

				mstream.Seek(0, SeekOrigin.Begin);

				await listener.ReadStreamAsyncWrapper(mstream);

				Assert.True(called);

			}
		}
	}
}
