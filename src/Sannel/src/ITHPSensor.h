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
/* This is generated code so probably best not to edit it */
#ifndef _ITHPSENSOR_H_
#define _ITHPSENSOR_H_

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

#include "ISensor.h"
#include "IHumiditySensor.h"
#include "IPressureSensor.h"
#include "ITemperatureSensor.h"


namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			class ITHPSensor : public ITemperatureSensor, public ISensor, public IHumiditySensor, public IPressureSensor
			{
			public:
			private:
			};
		}
	}
}

#endif
