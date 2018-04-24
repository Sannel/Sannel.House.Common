/*
This library was started from Sparkfun's Si7021 library see below for more info

SparkFun Si7021 Temperature and HUmidity Breakout
By: Joel Bartlett
SparkFun Electronics
Date: December 10, 2015

This is an Arduino library for the Si7021 Temperature and Humidity Sensor Breakout

This library is based on the following libraries:
HTU21D Temperature / Humidity Sensor Library
By: Nathan Seidle
https://github.com/sparkfun/HTU21D_Breakout/tree/master/Libraries
Arduino Si7010 relative humidity + temperature sensor
By: Jakub Kaminski, 2014
https://github.com/teoqba/ADDRESS
This Library is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
This Library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
For a copy of the GNU General Public License, see
<http://www.gnu.org/licenses/>.
*/

#include "Si7021.h"
#include "IWireDevice.h"

using namespace Sannel::House::Sensor::Temperature;

Si7021::Si7021(IWireDevice& device)
{
	this->device = &device;
}

void Si7021::Begin()
{
	uint8_t id_Temp_Hum = checkID();

	int x = 0;

	if (id_Temp_Hum == 0x15)//Ping CheckID register
	{
		x = 1;
	}
	else if (id_Temp_Hum == 0x32)
	{
		x = 2;
	}
	else
	{
		x = 0;
	}

	if (x == 1)
	{
		Serial.println("Si7021 Found");
	}
	else if (x == 2)
	{
		Serial.println("HTU21D Found");
	}
	else
	{
		Serial.println("No Devices Detected");
	}
}

/****************Si7021 & HTU21D Functions**************************************/


float Si7021::GetRelativeHumidity()
{
	// Measure the relative humidity
	uint16_t RH_Code = makeMeasurment(SI7021_HUMD_MEASURE_NOHOLD);
	float result = (125.0*RH_Code / 65536) - 6;
	return result;
}

float Si7021::GetTemperatureCelsius()
{
	// Read temperature from previous RH measurement.
	uint16_t temp_Code = makeMeasurment(SI7021_TEMP_PREV);
	float result = (175.72*temp_Code / 65536) - 46.85;
	return result;
}

float Si7021::ReadStoredTemp()
{
	// Measure temperature
	uint16_t temp_Code = makeMeasurment(SI7021_TEMP_MEASURE_NOHOLD);
	float result = (175.72*temp_Code / 65536) - 46.85;
	return result;
}

void Si7021::HeaterOn()
{
	// Turns on the ADDRESS heater
	uint8_t regVal = readReg();
	regVal |= bv(SI7021_HTRE);
	//turn on the heater
	writeReg(regVal);
}

void Si7021::HeaterOff()
{
	// Turns off the ADDRESS heater
	uint8_t regVal = readReg();
	regVal &= ~bv(SI7021_HTRE);
	writeReg(regVal);
}

void Si7021::ChangeResolution(Si7021Resolutions i)
{
	// Changes to resolution of ADDRESS measurements.
	// Set i to:
	//      RH         Temp
	// 0: 12 bit       14 bit (default)
	// 1:  8 bit       12 bit
	// 2: 10 bit       13 bit
	// 3: 11 bit       11 bit

	uint8_t regVal = readReg();
	// zero resolution bits
	regVal &= 0b011111110;
	switch (i) {
	case Si7021Resolutions::_8Bit:
		regVal |= 0b00000001;
		break;
	case Si7021Resolutions::_10Bit:
		regVal |= 0b10000000;
		break;
	case Si7021Resolutions::_11Bit:
		regVal |= 0b10000001;
		break;
	default:
		regVal |= 0b00000000;
		break;
	}
	// write new resolution settings to the register
	writeReg(regVal);
}

void Si7021::Reset()
{
	//Reset user resister
	writeReg(SI7021_SOFT_RESET);
}

uint8_t Si7021::checkID()
{
	uint8_t ID_1;

	// Check device ID
	this->device->Write(0xFC, 0xC9);

	ID_1 = this->device->ReadByte();

	return(ID_1);
}

uint16_t Si7021::makeMeasurment(uint8_t command)
{
	// Take one ADDRESS measurement given by command.
	// It can be either temperature or relative humidity
	// TODO: implement checksum checking

	int nBytes = 3;
	// if we are only reading old temperature, read only msb and lsb
	if (command == 0xE0) nBytes = 2;

	this->device->Write(command);
	// When not using clock stretching (*_NOHOLD commands) delay here
	// is needed to wait for the measurement.
	// According to datasheet the max. conversion time is ~22ms
	delay(100);

	uint8_t data[3];

	int read = this->device->Read(data, nBytes);
	if (read != nBytes) 
	{
		return 100;
	}

	unsigned int msb = data[0];
	unsigned int lsb = data[1];
	// Clear the last to bits of LSB to 00.
	// According to datasheet LSB of RH is always xxxxxx10
	lsb &= 0xFC;
	unsigned int mesurment = msb << 8 | lsb;

	return mesurment;
}

void Si7021::writeReg(uint8_t value)
{
	// Write to user register on ADDRESS
	this->device->Write(SI7021_WRITE_USER_REG, value);
}

uint8_t Si7021::readReg()
{
	// Read from user register on ADDRESS
	uint8_t regVal = this->device->WriteRead(SI7021_READ_USER_REG);
	return regVal;
}