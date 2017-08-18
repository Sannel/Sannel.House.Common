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

#include "stdafx.h"

#include "SensorStore.h"

using namespace Sannel::House::Sensor;

SensorStore::SensorStore(int size)
{
	this->size = size;
	this->packets = new SensorPacket[size];
	this->macAddress[0] = 0;
	this->macAddress[1] = 0;
	this->macAddress[2] = 0;
	this->macAddress[3] = 0;
	this->macAddress[4] = 0;
	this->macAddress[5] = 0;
}

void SensorStore::SetMacAddress(unsigned char* mac)
{

}

const unsigned char* SensorStore::GetMacAddress()
{
	return this->macAddress;
}

int SensorStore::GetSize()
{
	return size;
}