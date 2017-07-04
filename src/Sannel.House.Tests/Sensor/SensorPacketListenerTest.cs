using Sannel.House.Sensor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sannel.House.Tests.Sensor
{
    public class SensorPacketListenerTest
    {
		[Fact]
		public async Task Begin()
		{
			using(var listener = new SensorPacketListener())
			{
				listener.Begin(8172);

				await Task.Delay(50000000);
			}
		}
    }
}
