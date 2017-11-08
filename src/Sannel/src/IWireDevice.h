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
/* This is generated code so probably best not to edit it */
#ifndef _IWIREDEVICE_H_
#define _IWIREDEVICE_H_

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif


namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			class IWireDevice
			{
			public:
				virtual void Write(uint8_t b) = 0;
				virtual void Write(uint8_t b1, uint8_t b2) = 0;
				virtual uint8_t WriteRead(uint8_t write) = 0;
				virtual void WriteRead(uint8_t write, uint8_t* read, int length) = 0;
			private:
			};
		}
	}
}

#endif
