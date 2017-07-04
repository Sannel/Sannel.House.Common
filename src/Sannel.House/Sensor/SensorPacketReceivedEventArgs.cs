using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Sensor
{
    public class SensorPacketReceivedEventArgs : EventArgs
    {
		public SensorPacket Packet { get; set; }
	}
}
