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
   
#ifndef _SENSORPACKET_
#define _SENSORPACKET_

#include "SensorTypes.h"

#ifndef READINGS_BUFFER
#define READINGS_BUFFER 15 // the number of readings we hold in memory before sending it to the server
#endif

namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			struct SensorPacket
			{
				unsigned char MacAddress[6];
				SensorTypes SensorType;
				double Values[10];
			};
			
			void ResetSensorPacket(SensorPacket &packet);
		}
	}
}

#endif
