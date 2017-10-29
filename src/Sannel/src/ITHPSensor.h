#ifndef _ITHPSENSOR_H_
#define _ITHPSENSOR_H_

#include "ITemperatureSensor.h"
#include "IHumiditySensor.h"
#include "IPressureSensor.h"

namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			class ITHPSensor : public ITemperatureSensor, public IHumiditySensor, public IPressureSensor {
			};
		}
	}
}

#endif
