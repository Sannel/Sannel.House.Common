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
#ifndef _MACADDRESS_H_
#define _MACADDRESS_H_

namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			struct MAddress
			{
				unsigned char Byte1;
				unsigned char Byte2;
				unsigned char Byte3;
				unsigned char Byte4;
				unsigned char Byte5;
				unsigned char Byte6;
			};
		}
	}
}

#endif