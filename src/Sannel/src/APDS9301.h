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
#ifndef _APDS9301_H_
#define _APDS9301_H_

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif


#include "IWire.h"
#include "IWireDevice.h"
#include "ISensor.h"
#include "ILightSensor.h"
#include "APDS9301IntegrationTimes.h"


#define APDS9301_CONTROL_REG 128
#define APDS9301_TIMING_REG 129
#define APDS9301_THRESHLOWLOW_REG 130
#define APDS9301_THRESHLOWHI_REG 131
#define APDS9301_THRESHHILOW_REG 132
#define APDS9301_THRESHHIHI_REG 133
#define APDS9301_INTERRUPT_REG 134
#define APDS9301_ID_REG 138
#define APDS9301_DATA0LOW_REG 140
#define APDS9301_DATA0HI_REG 141
#define APDS9301_DATA1LOW_REG 142
#define APDS9301_DATA1HI_REG 143

namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			namespace Light
			{
				class APDS9301 : public ISensor, public ILightSensor
				{
				public:
					APDS9301(IWireDevice& device);
					void Begin() override;
					bool PowerEnable(bool powerOn);
					bool SetGain(bool high);
					bool SetIntegrationTime(APDS9301_IntegrationTime integrationTime);
					bool EnableInterrupt(bool interruptMode);
					bool ClearInterruptFlag();
					bool SetCyclesForInterrupt(uint8_t cycles);
					bool SetLowThreshold(uint16_t threshold);
					bool SetHighThreshold(uint16_t threshold);
					uint8_t GetIDReg();
					bool GetGain();
					APDS9301_IntegrationTime GetIntegrationTime();
					uint8_t GetCyclesForInterrupt();
					uint16_t GetLowThreshold();
					uint16_t GetHighThreshold();
					uint16_t ReadCH0Level();
					uint16_t ReadCH1Level();
					float GetLuxLevel() override;
				private:
					uint8_t getRegister(uint8_t regAddress);
					bool setRegister(uint8_t regAddress, uint8_t newVal);
					uint16_t getTwoRegisters(uint8_t regAddress);
					bool setTwoRegisters(uint8_t regAddress, uint16_t newVal);
					IWireDevice* device;
				};
			}
		}
	}
}

#endif
