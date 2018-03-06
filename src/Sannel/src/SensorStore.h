/* Copyright 2018 Sannel Software, L.L.C.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

	   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.*/

#ifndef _SENSORSTORE_H_
#define _SENSORSTORE_H_

#include "SensorPacket.h"
#include "MAddress.h"
#include "ITemperatureSensor.h"
#include "ITHPSensor.h"

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
#else
	#include "..\..\Sannel.Tests\Stream.h"
#endif

namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			class SensorStore
			{
			public:
				SensorStore(unsigned char size);
				void SetMacAddress(unsigned char* mac);

				MAddress GetMacAddress();
				unsigned char GetSize();

				unsigned char GetStoredPackets();

				bool AddReading(SensorPacket &packet);
				bool AddReading(ITemperatureSensor &sensor, unsigned long offset=0);
				bool AddReading(ITHPSensor &sensor, unsigned long offset = 0);

				SensorPacket &GetPacket(unsigned char index);



				void WritePackets(Stream &stream);
				void Reset();

			private:
				unsigned char current;
				unsigned char size;
				MAddress macAddress;
				SensorPacket *packets;
			};
		}
	}
}

#endif