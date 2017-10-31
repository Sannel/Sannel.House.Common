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

bool SensorStore::AddReading(ITemperatureSensor &sensor, unsigned long offset)
{
	SensorPacket p;
	ResetSensorPacket(p);

	p.SensorType = SensorTypes::Temperature;
	p.Offset = offset;
	p.Values[0] = sensor.GetTemperatureCelsius();

	return this->AddReading(p);
}

bool SensorStore::AddReading(ITHPSensor &sensor, unsigned long offset)
{
	SensorPacket p;
	ResetSensorPacket(p);

	p.SensorType = SensorTypes::TemperatureHumidityPressure;
	p.Offset = offset;
	p.Values[0] = sensor.GetTemperatureCelsius();
	p.Values[1] = sensor.GetRelativeHumidity();
	p.Values[2] = sensor.GetPressure();

	return this->AddReading(p);
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
	Serial.println(current);
	stream.write(current);
	stream.write(macAddress.Byte1);
	stream.write(macAddress.Byte2);
	stream.write(macAddress.Byte3);
	stream.write(macAddress.Byte4);
	stream.write(macAddress.Byte5);
	stream.write(macAddress.Byte6);
	stream.flush();

	Serial.print(macAddress.Byte1);
	Serial.print(" ");
	Serial.print(macAddress.Byte2);
	Serial.print(" ");
	Serial.print(macAddress.Byte3);
	Serial.print(" ");
	Serial.print(macAddress.Byte4);
	Serial.print(" ");
	Serial.print(macAddress.Byte5);
	Serial.print(" ");
	Serial.println(macAddress.Byte6);



	SensorPacket* p;
	for (unsigned char i = 0; i < current; i++)
	{
		p = &packets[i];

		byte *b;

		b = (byte*)&(p->SensorType);

		for (unsigned char y = 0; y < 4; y++) 
		{
			stream.write(b[y]);
		}
		stream.flush();
		Serial.print("Sensor Type ");
		Serial.println(int(p->SensorType));
		b = (byte*)&(p->Offset);
		for (unsigned char y = 0; y < 4; y++) 
		{
			stream.write(b[y]);
		}
		stream.flush();
		Serial.print("Offset ");
		Serial.println(p->Offset);

		for (unsigned char j = 0; j < 10; j++) 
		{
			b = (byte*)(&(p->Values[j]));
			for (unsigned char k = 0; k < 8; k++) 
			{
				stream.write(b[k]);
			}
			stream.flush();
			Serial.print("Values[");
			Serial.print(j);
			Serial.print("] = ");
			Serial.println(p->Values[j]);
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