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

#include "Stream.h"

size_t Stream::print(unsigned char c)
{
	data.push_back(c);
	return sizeof(c);
}

size_t Stream::print(double d)
{
	unsigned char* c = (unsigned char*)&d;

	int size = sizeof(d);

	for (int i = 0; i < size; i++)
	{
		data.push_back(c[i]);
	}

	return sizeof(d);
}

size_t Stream::print(int i)
{
	unsigned char* c = (unsigned char*)&i;
	int size = sizeof(i);
	for (int j = 0; j < size; j++)
	{
		data.push_back(c[j]);
	}

	return sizeof(i);
}

size_t Stream::print(unsigned long i)
{
	unsigned char* c = (unsigned char*)&i;
	int size = sizeof(i);
	for (int j = 0; j < size; j++)
	{
		data.push_back(c[j]);
	}

	return sizeof(i);
}

void Stream::Clear()
{
	data.clear();
}

std::vector<unsigned char> Stream::GetData()
{
	return data;
}