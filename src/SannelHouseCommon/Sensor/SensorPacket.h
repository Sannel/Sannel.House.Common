#pragma once

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
#else
	#include "WProgram.h"
#endif

#include "Sensor/SensorTypes.h"

namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			struct SensorPacket
			{
				int DeviceId;
				SensorTypes SensorType;
				double Values[9];
			};

			union SensorPacketUnion
			{
				SensorPacket Packet;
				uint8 Data[80];
			};
		}
	}
}
