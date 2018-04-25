/* Copyright 2018 Sannel Software, L.L.C.

Licensed under the Apache License, Version 2.0 (the ""License"");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an ""AS IS"" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.*/

/******************************************************************************
BME280.cpp

This file is a modified version of SparkFun's library


SparkFunBME280.cpp
BME280 Arduino and Teensy Driver
Marshall Taylor @ SparkFun Electronics
May 20, 2015
https://github.com/sparkfun/SparkFun_BME280_Arduino_Library

This code is released under the [MIT License](http://opensource.org/licenses/MIT).
Please review the LICENSE.md file included with this example. If you have any questions
or concerns with licensing, please contact techsupport@sparkfun.com.
Distributed as-is; no warranty is given.
******************************************************************************/
//See SparkFunBME280.h for additional topology notes.

#include "BME280.h"
#include "stdint.h"
#include <math.h>

using namespace Sannel::House::Sensor::Temperature;

//****************************************************************************//
//
//  Settings and configuration
//
//****************************************************************************//

//Constructor -- Specifies default configuration
BME280::BME280(IWireDevice& device)
{
	this->device = &device;

	//renMode can be:
	//  0, Sleep mode
	//  1 or 2, Forced mode
	//  3, Normal mode
	RunMode = 3; //Normal mode

	//tStandby can be:
	//  0, 0.5ms
	//  1, 62.5ms
	//  2, 125ms
	//  3, 250ms
	//  4, 500ms
	//  5, 1000ms
	//  6, 10ms
	//  7, 20ms
	TimeStandBy = 0;

	//filter can be off or number of FIR coefficients to use:
	//  0, filter off
	//  1, coefficients = 2
	//  2, coefficients = 4
	//  3, coefficients = 8
	//  4, coefficients = 16
	Filter = 0;

	// tempOverSample can be :
	//  0, skipped
	//  1 through 5, oversampling *1, *2, *4, *8, *16 respectively
	TemperatureOverSample = 1;

	//pressOverSample can be:
	//  0, skipped
	//  1 through 5, oversampling *1, *2, *4, *8, *16 respectively
	PressureOverSample = 1;

	//humidOverSample can be:
	//  0, skipped
	//  1 through 5, oversampling *1, *2, *4, *8, *16 respectively
	HumidityOverSample = 1;
}

//****************************************************************************//
//
//  Configuration section
//
//  This uses the stored SensorSettings to start the IMU
//  Use statements such as "mySensor.settings.commInterface = SPI_MODE;" to 
//  configure before calling .begin();
//
//****************************************************************************//
void BME280::Begin()
{
	//Reading all compensation data, range 0x88:A1, 0xE1:E7
	uint8_t dataToWrite = 0;  //Temporary variable

	digT1 = ((uint16_t)((readRegister(BME280_DIG_T1_MSB_REG) << 8) + readRegister(BME280_DIG_T1_LSB_REG)));
	digT2 = ((int16_t)((readRegister(BME280_DIG_T2_MSB_REG) << 8) + readRegister(BME280_DIG_T2_LSB_REG)));
	digT3 = ((int16_t)((readRegister(BME280_DIG_T3_MSB_REG) << 8) + readRegister(BME280_DIG_T3_LSB_REG)));

	digP1 = ((uint16_t)((readRegister(BME280_DIG_P1_MSB_REG) << 8) + readRegister(BME280_DIG_P1_LSB_REG)));
	digP2 = ((int16_t)((readRegister(BME280_DIG_P2_MSB_REG) << 8) + readRegister(BME280_DIG_P2_LSB_REG)));
	digP3 = ((int16_t)((readRegister(BME280_DIG_P3_MSB_REG) << 8) + readRegister(BME280_DIG_P3_LSB_REG)));
	digP4 = ((int16_t)((readRegister(BME280_DIG_P4_MSB_REG) << 8) + readRegister(BME280_DIG_P4_LSB_REG)));
	digP5 = ((int16_t)((readRegister(BME280_DIG_P5_MSB_REG) << 8) + readRegister(BME280_DIG_P5_LSB_REG)));
	digP6 = ((int16_t)((readRegister(BME280_DIG_P6_MSB_REG) << 8) + readRegister(BME280_DIG_P6_LSB_REG)));
	digP7 = ((int16_t)((readRegister(BME280_DIG_P7_MSB_REG) << 8) + readRegister(BME280_DIG_P7_LSB_REG)));
	digP8 = ((int16_t)((readRegister(BME280_DIG_P8_MSB_REG) << 8) + readRegister(BME280_DIG_P8_LSB_REG)));
	digP9 = ((int16_t)((readRegister(BME280_DIG_P9_MSB_REG) << 8) + readRegister(BME280_DIG_P9_LSB_REG)));

	digH1 = ((uint8_t)(readRegister(BME280_DIG_H1_REG)));
	digH2 = ((int16_t)((readRegister(BME280_DIG_H2_MSB_REG) << 8) + readRegister(BME280_DIG_H2_LSB_REG)));
	digH3 = ((uint8_t)(readRegister(BME280_DIG_H3_REG)));
	digH4 = ((int16_t)((readRegister(BME280_DIG_H4_MSB_REG) << 4) + (readRegister(BME280_DIG_H4_LSB_REG) & 0x0F)));
	digH5 = ((int16_t)((readRegister(BME280_DIG_H5_MSB_REG) << 4) + ((readRegister(BME280_DIG_H4_LSB_REG) >> 4) & 0x0F)));
	digH6 = ((int8_t)readRegister(BME280_DIG_H6_REG));

	//Set the oversampling control words.
	//config will only be writeable in sleep mode, so first insure that.
	writeRegister(BME280_CTRL_MEAS_REG, 0x00);

	//Set the config word
	dataToWrite = (TimeStandBy << 0x5) & 0xE0;
	dataToWrite |= (Filter << 0x02) & 0x1C;
	writeRegister(BME280_CONFIG_REG, dataToWrite);

	//Set ctrl_hum first, then ctrl_meas to activate ctrl_hum
	dataToWrite = HumidityOverSample & 0x07; //all other bits can be ignored
	writeRegister(BME280_CTRL_HUMIDITY_REG, dataToWrite);

	//set ctrl_meas
	//First, set temp oversampling
	dataToWrite = (TemperatureOverSample << 0x5) & 0xE0;
	//Next, pressure oversampling
	dataToWrite |= (PressureOverSample << 0x02) & 0x1C;
	//Last, set mode
	dataToWrite |= (RunMode) & 0x03;
	//Load the byte
	writeRegister(BME280_CTRL_MEAS_REG, dataToWrite);

	readRegister(0xD0);
}

//Strictly resets.  Run .Begin() afterwards
void BME280::Reset()
{
	writeRegister(BME280_RST_REG, 0xB6);
}

//****************************************************************************//
//
//  Pressure Section
//
//****************************************************************************//
float BME280::GetPressure()
{
	// Returns pressure in Pa as unsigned 32 bit integer in Q24.8 format (24 integer bits and 8 fractional bits).
	// Output value of “24674867” represents 24674867/256 = 96386.2 Pa = 963.862 hPa
	int32_t adc_P = ((uint32_t)readRegister(BME280_PRESSURE_MSB_REG) << 12) | ((uint32_t)readRegister(BME280_PRESSURE_LSB_REG) << 4) | ((readRegister(BME280_PRESSURE_XLSB_REG) >> 4) & 0x0F);

	int64_t var1, var2, p_acc;
	var1 = (storedTemperature) - 128000;
	var2 = var1 * var1 * (int64_t)digP6;
	var2 = var2 + ((var1 * (int64_t)digP5) << 17);
	var2 = var2 + (((int64_t)digP4) << 35);
	var1 = ((var1 * var1 * (int64_t)digP3) >> 8) + ((var1 * (int64_t)digP2) << 12);
	var1 = (((((int64_t)1) << 47) + var1))*((int64_t)digP1) >> 33;
	if (var1 == 0)
	{
		return 0; // avoid exception caused by division by zero
	}
	p_acc = 1048576 - adc_P;
	p_acc = (((p_acc << 31) - var2) * 3125) / var1;
	var1 = (((int64_t)digP9) * (p_acc >> 13) * (p_acc >> 13)) >> 25;
	var2 = (((int64_t)digP8) * p_acc) >> 19;
	p_acc = ((p_acc + var1 + var2) >> 8) + (((int64_t)digP7) << 4);

	return (float)p_acc / 256.0;
}

//****************************************************************************//
//
//  Humidity Section
//
//****************************************************************************//
float BME280::GetRelativeHumidity()
{
	// Returns humidity in %RH as unsigned 32 bit integer in Q22. 10 format (22 integer and 10 fractional bits).
	// Output value of “47445” represents 47445/1024 = 46. 333 %RH
	int32_t adc_H = ((uint32_t)readRegister(BME280_HUMIDITY_MSB_REG) << 8) | ((uint32_t)readRegister(BME280_HUMIDITY_LSB_REG));

	int32_t var1;
	var1 = (storedTemperature - ((int32_t)76800));
	var1 = (((((adc_H << 14) - (((int32_t)digH4) << 20) - (((int32_t)digH5) * var1)) +
		((int32_t)16384)) >> 15) * (((((((var1 * ((int32_t)digH6)) >> 10) * (((var1 * ((int32_t)digH3)) >> 11) + ((int32_t)32768))) >> 10) + ((int32_t)2097152)) *
		((int32_t)digH2) + 8192) >> 14));
	var1 = (var1 - (((((var1 >> 15) * (var1 >> 15)) >> 7) * ((int32_t)digH1)) >> 4));
	var1 = (var1 < 0 ? 0 : var1);
	var1 = (var1 > 419430400 ? 419430400 : var1);

	return (float)(var1 >> 12) / 1024.0;

}



//****************************************************************************//
//
//  Temperature Section
//
//****************************************************************************//

float BME280::GetTemperatureCelsius()
{
	// Returns temperature in DegC, resolution is 0.01 DegC. Output value of “5123” equals 51.23 DegC.
	// t_fine carries fine temperature as global value

	//get the reading (adc_T);
	int32_t adc_T = ((uint32_t)readRegister(BME280_TEMPERATURE_MSB_REG) << 12) | ((uint32_t)readRegister(BME280_TEMPERATURE_LSB_REG) << 4) | ((readRegister(BME280_TEMPERATURE_XLSB_REG) >> 4) & 0x0F);

	//By datasheet, calibrate
	int64_t var1, var2;

	var1 = ((((adc_T >> 3) - ((int32_t)digT1 << 1))) * ((int32_t)digT2)) >> 11;
	var2 = (((((adc_T >> 4) - ((int32_t)digT1)) * ((adc_T >> 4) - ((int32_t)digT1))) >> 12) *
		((int32_t)digT3)) >> 14;
	storedTemperature = var1 + var2;
	float output = (storedTemperature * 5 + 128) >> 8;

	output = output / 100;

	return output;
}

uint8_t BME280::readRegister(uint8_t offset)
{
	return this->device->WriteRead(offset);
}

void BME280::readRegisterRegion(uint8_t *outputPointer, uint8_t offset, uint8_t length)
{
	//define pointer that will point to the external space
	uint8_t i = 0;
	char c = 0;

	this->device->WriteRead(offset, outputPointer, length);
}

int16_t BME280::readRegisterInt16(uint8_t offset)
{
	uint8_t myBuffer[2];
	this->device->WriteRead(offset, myBuffer, 2);
	int16_t output = (int16_t)myBuffer[0] | int16_t(myBuffer[1] << 8);

	return output;
}

void BME280::writeRegister(uint8_t offset, uint8_t dataToWrite)
{
	device->Write(offset, dataToWrite);
}
