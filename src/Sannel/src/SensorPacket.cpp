
#include "SensorPacket.h"
#include <algorithm>

void Sannel::House::Sensor::ResetSensorPacketUnion(SensorPacketUnion* packet)
{
	std::fill_n(packet->Data, sizeof(packet->Data), 255);
}