#ifndef _IPRESSURESENSOR_H_
#define _IPRESSURESENSOR_H_

#include "ISensor.h"

namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			class IPressureSensor : public ISensor {
			public:
				virtual double GetPressure() = 0;
			};
		}
	}
}

#endif
