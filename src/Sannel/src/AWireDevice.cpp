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
#include "AWireDevice.h"

#include "Wire.h"

using namespace Sannel::House::Sensor;

AWireDevice::AWireDevice(uint8_t id) 
{
	address = id;
}

void AWireDevice::Write(uint8_t b)
{
	Wire.beginTransmission(address);
	Wire.write(b);
	Wire.endTransmission();
}

void AWireDevice::Write(uint8_t b1, uint8_t b2)
{
	Wire.beginTransmission(address);
	Wire.write(b1);
	Wire.write(b2);
	Wire.endTransmission();
}

void AWireDevice::Write(uint8_t b1, uint8_t b2, uint8_t b3)
{
	Wire.beginTransmission(address);
	Wire.write(b1);
	Wire.write(b2);
	Wire.write(b3);
	Wire.endTransmission();
}

unsigned int AWireDevice::Read(uint8_t* read, int length) 
{
	Wire.requestFrom((int)address, length);
	//Wire.endTransmission();
	unsigned int index = 0;
	while (Wire.available())
	{
		read[index] = Wire.read();
		index++;
	}

	return index;
}

unsigned int AWireDevice::Read(int* read, int length) 
{
	Wire.requestFrom((int)address, length);
	//Wire.endTransmission();
	unsigned int index = 0;
	while (Wire.available())
	{
		read[index] = Wire.read();
		index++;
	}

	return index;
}

unsigned int AWireDevice::Read(unsigned int* read, int length) 
{
	Wire.requestFrom((int)address, length);
	//Wire.endTransmission();
	unsigned int index = 0;
	while (Wire.available())
	{
		read[index] = Wire.read();
		index++;
	}

	return index;
}

uint8_t AWireDevice::ReadByte()
{
	Wire.requestFrom(address, (uint8_t)1);
	//Wire.endTransmission();
	if (Wire.available())
	{
		return (uint8_t)Wire.read();
	}
	return (uint8_t)0;
}


uint8_t AWireDevice::WriteRead(uint8_t write)
{
	uint8_t result;
	Wire.beginTransmission(address);
	Wire.write(write);
	Wire.endTransmission();

	Wire.requestFrom(address, (uint8_t)1);
	while (Wire.available()) // slave may send less than requested
	{
		result = Wire.read(); // receive a byte as a proper uint8_t
		return result;
	}
	return 0;
}

void AWireDevice::WriteRead(uint8_t write, uint8_t* read, int length)
{
	Wire.beginTransmission(address);
	Wire.write(write);
	Wire.endTransmission();

	Wire.requestFrom((int)address, length);
	int index = 0;
	while (Wire.available()) // slave may send less then requested
	{
		read[index] = Wire.read();
		index++;
	}
}
