
#include "SensorPacket.h"
#include <stdlib.h>

void Sannel::House::Sensor::ResetSensorPacketUnion(SensorPacketUnion* packet)
{
	std::fill_n(packet->Data, sizeof(packet->Data), 255);
}