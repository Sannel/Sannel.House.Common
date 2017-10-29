#ifndef _IHUMIDITYSENSOR_H_
#define _IHUMIDITYSENSOR_H_

#include "ISensor.h"

namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			class IHumiditySensor : public ISensor {
			public:
				virtual double GetRelativeHumidity() = 0;
			};
		}
	}
}

#endif