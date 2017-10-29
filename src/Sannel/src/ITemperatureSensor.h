#ifndef _ITEMPERATURESENSOR_H_
#define _ITEMPERATURESENSOR_H_

#include "ISensor.h"

namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			class ITemperatureSensor : public ISensor {
			public:
				virtual double GetTemperatureCelsius() = 0;
			};
		}
	}
}

#endif