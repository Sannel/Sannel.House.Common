using Sannel.House.Sensor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sannel.House.Tests.Sensor
{
	public class TCPSensorPacketListenerMock : TCPSensorPacketListener
	{
		public TCPSensorPacketListenerMock(ILogger<TCPSensorPacketListener> logger) : base(logger)
		{
		}

		public Task ReadStreamAsyncWrapper(Stream s)
		{
			return ReadStreamAsync(s);
		}
	}
}
