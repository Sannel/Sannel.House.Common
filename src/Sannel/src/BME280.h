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
#ifndef _BME280_H_
#define _BME280_H_

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

#include "IWire.h"
#include "IWireDevice.h"
#include "ITHPSensor.h"

#define BME280_DIG_T1_LSB_REG 136
#define BME280_DIG_T1_MSB_REG 137
#define BME280_DIG_T2_LSB_REG 138
#define BME280_DIG_T2_MSB_REG 139
#define BME280_DIG_T3_LSB_REG 140
#define BME280_DIG_T3_MSB_REG 141
#define BME280_DIG_P1_LSB_REG 142
#define BME280_DIG_P1_MSB_REG 143
#define BME280_DIG_P2_LSB_REG 144
#define BME280_DIG_P2_MSB_REG 145
#define BME280_DIG_P3_LSB_REG 146
#define BME280_DIG_P3_MSB_REG 147
#define BME280_DIG_P4_LSB_REG 148
#define BME280_DIG_P4_MSB_REG 149
#define BME280_DIG_P5_LSB_REG 150
#define BME280_DIG_P5_MSB_REG 151
#define BME280_DIG_P6_LSB_REG 152
#define BME280_DIG_P6_MSB_REG 153
#define BME280_DIG_P7_LSB_REG 154
#define BME280_DIG_P7_MSB_REG 155
#define BME280_DIG_P8_LSB_REG 156
#define BME280_DIG_P8_MSB_REG 157
#define BME280_DIG_P9_LSB_REG 158
#define BME280_DIG_P9_MSB_REG 159
#define BME280_DIG_H1_REG 161
#define BME280_CHIP_ID_REG 208
#define BME280_RST_REG 224
#define BME280_DIG_H2_LSB_REG 225
#define BME280_DIG_H2_MSB_REG 226
#define BME280_DIG_H3_REG 227
#define BME280_DIG_H4_MSB_REG 228
#define BME280_DIG_H4_LSB_REG 229
#define BME280_DIG_H5_MSB_REG 230
#define BME280_DIG_H6_REG 231
#define BME280_CTRL_HUMIDITY_REG 242
#define BME280_STAT_REG 243
#define BME280_CTRL_MEAS_REG 244
#define BME280_CONFIG_REG 245
#define BME280_PRESSURE_MSB_REG 247
#define BME280_PRESSURE_LSB_REG 248
#define BME280_PRESSURE_XLSB_REG 249
#define BME280_TEMPERATURE_MSB_REG 250
#define BME280_TEMPERATURE_LSB_REG 251
#define BME280_TEMPERATURE_XLSB_REG 252
#define BME280_HUMIDITY_MSB_REG 253
#define BME280_HUMIDITY_LSB_REG 254

namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			namespace Temperature
			{
				class BME280 : public ITHPSensor
				{
				public:
					BME280(IWireDevice& device);
					void Begin() override;
					void Reset();
					double GetPressure() override;
					double GetRelativeHumidity() override;
					double GetTemperatureCelsius() override;
					uint8_t RunMode;
					uint8_t TimeStandBy;
					uint8_t Filter;
					uint8_t TemperatureOverSample;
					uint8_t PressureOverSample;
					uint8_t HumidityOverSample;
				private:
					uint8_t readRegister(uint8_t offset);
					void readRegisterRegion(uint8_t* outputPointer, uint8_t offset, uint8_t length);
					int16_t readRegisterInt16(uint8_t offset);
					void writeRegister(uint8_t offset, uint8_t dataToWrite);
					IWireDevice* device;
					int64_t storedTemperature;
					uint16_t digT1;
					int16_t digT2;
					int16_t digT3;
					uint16_t digP1;
					int16_t digP2;
					int16_t digP3;
					int16_t digP4;
					int16_t digP5;
					int16_t digP6;
					int16_t digP7;
					int16_t digP8;
					int16_t digP9;
					uint8_t digH1;
					int16_t digH2;
					uint8_t digH3;
					int16_t digH4;
					int16_t digH5;
					uint8_t digH6;
				};
			}
		}
	}
}

#endif
