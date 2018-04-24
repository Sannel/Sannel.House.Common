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
/* This is generated code so probably best not to edit it */
#ifndef _SI7021_H_
#define _SI7021_H_

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif


#include "IWire.h"
#include "IWireDevice.h"
#include "ISensor.h"
#include "ITemperatureSensor.h"
#include "IHumiditySensor.h"
#include "Si7021Resolutions.h"


#define SI7021_TEMP_MEASURE_HOLD 227
#define SI7021_HUMD_MEASURE_HOLD 229
#define SI7021_TEMP_MEASURE_NOHOLD 243
#define SI7021_HUMD_MEASURE_NOHOLD 245
#define SI7021_TEMP_PREV 224
#define SI7021_WRITE_USER_REG 230
#define SI7021_READ_USER_REG 231
#define SI7021_SOFT_RESET 254
#define SI7021_HTRE 2
#define SI7021_CRC_POLY 9994240
#define SI7021_I2C_TIMEOUT 998
#define SI7021_BAD_CRC 999

namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			namespace Temperature
			{
				class Si7021 : public ITemperatureSensor, public IHumiditySensor
				{
				public:
					Si7021(IWireDevice& device);
					void Begin() override;
					float GetRelativeHumidity() override;
					float GetTemperatureCelsius() override;
					void Reset();
					void ChangeResolution(Si7021Resolutions i);
					void HeaterOn();
					void HeaterOff();
					double ReadStoredTemp();
				private:
					uint8_t bv(uint8_t bit);
					uint8_t checkID();
					uint16_t makeMeasurment(uint8_t command);
					void writeReg(uint8_t value);
					uint8_t readReg();
					IWireDevice* device;
				};
			}
		}
	}
}

#endif
