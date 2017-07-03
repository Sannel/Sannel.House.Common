using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Sensor
{
    public class SensorPacket
    {
		public int DeviceId { get; set; }
		public SensorTypes SensorType { get; set; }

		public double[] Values { get; internal set; } = new double[9];

		public void Fill(byte[] data)
		{
			DeviceId = 0;
			SensorType = SensorTypes.Test;
			for(var i = 0; i < Values.Length; i++)
			{
				Values[i] = 0;
			}

			if(data == null)
			{
				return;
			}

			if(data.Length >= 4)
			{
				DeviceId = BitConverter.ToInt32(data, 0);
			}

			if(data.Length >= 8)
			{
				SensorType = (SensorTypes)BitConverter.ToInt32(data, 4);
			}

			int startIndex;
			for(var i = 0; i < Values.Length; i++)
			{
				startIndex = 8 + (i * 8);
				if(data.Length >= startIndex + 8)
				{
					Values[i] = BitConverter.ToDouble(data, startIndex);
				}
			}
		}
	}
}
