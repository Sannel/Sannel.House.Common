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


#ifndef _AWIREDEVICE_H_
#define _AWIREDEVICE_H_

#include "IWireDevice.h"

namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			class AWireDevice : public IWireDevice
			{
			public:
				AWireDevice(uint8_t id);
				void Write(uint8_t b) override;
				void Write(uint8_t b1, uint8_t b2) override;
				void Write(uint8_t b1, uint8_t b2, uint8_t b3) override;
				uint8_t WriteRead(uint8_t write) override;
				void WriteRead(uint8_t write, uint8_t* read, int length) override;
				unsigned int Read(uint8_t* read, int length) override;
				unsigned int Read(int* read, int length) override;
				unsigned int Read(unsigned int* read, int length) override;
				uint8_t ReadByte() override;
			private:
				uint8_t address;
			};
		}
	}
}

#endif

