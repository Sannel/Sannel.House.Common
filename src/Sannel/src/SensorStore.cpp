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

#include "stdafx.h";

#include "Sannel.h"
#include "SensorStore.h"

using namespace Sannel::House::Sensor;

SensorStore::SensorStore(unsigned char size)
{
	this->size = size;
	packets = new SensorPacket[size];
	current = 0;
	macAddress.Byte1 = 0;
	macAddress.Byte2 = 0;
	macAddress.Byte3 = 0;
	macAddress.Byte4 = 0;
	macAddress.Byte5 = 0;
	macAddress.Byte6 = 0;
}

void SensorStore::SetMacAddress(unsigned char* mac)
{
	macAddress.Byte1 = mac[0];
	macAddress.Byte2 = mac[1];
	macAddress.Byte3 = mac[2];
	macAddress.Byte4 = mac[3];
	macAddress.Byte5 = mac[4];
	macAddress.Byte6 = mac[5];
}

MAddress SensorStore::GetMacAddress()
{
	return macAddress;
}

unsigned char SensorStore::GetSize()
{
	return size;
}

unsigned char SensorStore::GetStoredPackets()
{
	return current;
}

bool SensorStore::AddReading(SensorPacket &packet)
{
	if (current >= size) 
	{
		return true;
	}

	this->packets[current].SensorType = packet.SensorType;
	this->packets[current].Offset = packet.Offset;
	for (int i = 0; i < 10; i++) 
	{
		this->packets[current].Values[i] = packet.Values[i];
	}

	current++;

	return current >= size;
}

SensorPacket &SensorStore::GetPacket(unsigned char index)
{
	if (index >= 0 && index < size)
	{
		return packets[index];
	}
}

void SensorStore::WritePackets(Stream &stream)
{
	stream.print(current);
	stream.print(macAddress.Byte1);
	stream.print(macAddress.Byte2);
	stream.print(macAddress.Byte3);
	stream.print(macAddress.Byte4);
	stream.print(macAddress.Byte5);
	stream.print(macAddress.Byte6);

	SensorPacket* p;
	for (unsigned char i = 0; i < current; i++)
	{
		p = &packets[i];

		stream.print(int(p->SensorType));
		stream.print(p->Offset);
		for (unsigned char j = 0; j < 10; j++) 
		{
			stream.print(p->Values[j]);
		}
	}
}

void SensorStore::Reset()
{
	current = 0;

	for (unsigned int i = 0; i < size; i++) 
	{
		ResetSensorPacket(packets[i]);
	}
}