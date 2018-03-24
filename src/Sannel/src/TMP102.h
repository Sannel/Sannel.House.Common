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
#ifndef _TMP102_H_
#define _TMP102_H_

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

#include "IWire.h"
#include "IWireDevice.h"
#include "ISensor.h"
#include "ITemperatureSensor.h"


#define TMP102_TEMPERATURE_REGISTER 0
#define TMP102_CONFIG_REGISTER 1
#define TMP102_T_LOW_REGISTER 2
#define TMP102_T_HIGH_REGISTER 3

namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			namespace Temperature
			{
				class TMP102 : public ITemperatureSensor
				{
				public:
					TMP102(IWireDevice& device);
					double GetTemperatureCelsius() override;
					void Sleep();
					void Wakeup();
					void SetLowTemperatureCelsius(double temperature);
					void SetHighTemperatureCelsius(double temperature);
					double ReadLowTemperatureCelsius();
					double ReadHighTemperatureCelsius();
					void SetConversionRate(uint8_t rate);
					void SetExtendedMode(bool mode);
					void SetAlertPolarity(bool polarity);
					void SetFault(uint8_t faultSetting);
					void SetAlertMode(bool mode);
					void Begin() override;
				private:
					uint8_t Alert();
					void openPointerRegister(uint8_t pointerReg);
					uint8_t readRegister(uint8_t registerNumber);
					IWireDevice* device;
				};
			}
		}
	}
}

#endif
