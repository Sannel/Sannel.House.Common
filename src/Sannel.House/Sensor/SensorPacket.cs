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
using System.Text;

namespace Sannel.House.Sensor
{
	public class SensorPacket
	{
		public SensorTypes SensorType { get; set; }

		public uint MillisOffset { get; set; } = 0;

		public double[] Values { get; internal set; } = new double[10];

		public void Fill(byte[] data)
		{
			SensorType = SensorTypes.Test;
			for (var i = 0; i < Values.Length; i++)
			{
				Values[i] = 0;
			}

			if (data == null)
			{
				return;
			}

			if (data.Length >= 4)
			{
				SensorType = (SensorTypes)BitConverter.ToInt32(data, 0);
			}

			if (data.Length >= 8)
			{
				MillisOffset = BitConverter.ToUInt32(data, 4);
			}

			int startIndex;
			for (var i = 0; i < Values.Length; i++)
			{
				startIndex = 8 + (i * 8);
				if (data.Length >= startIndex + 8)
				{
					Values[i] = BitConverter.ToDouble(data, startIndex);
				}
			}
		}
	}
}
