/* Copyright 2017 Sannel Software, L.L.C.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

	   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.*/
/* This is generated code so probably best not to edit it */
#pragma once

#include "SensorTypes.h"
#include <Arduino.h>

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

			void ResetSensorPacketUnion(SensorPacketUnion* packet);
		}
	}
}
