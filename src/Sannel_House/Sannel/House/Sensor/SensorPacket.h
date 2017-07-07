#pragma once

#include "SensorTypes.h"

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

			void ResetSensorPacketUnion(SensorPacketUnion* packet)
			{
				std::fill_n(packet->Data, sizeof(packet->Data), 255);
			}
		}
	}
}
