#ifndef _ISENSOR_H_
#define _ISENSOR_H_

namespace Sannel
{
	namespace House
	{
		namespace Sensor
		{
			class ISensor {
			public:
				virtual void Begin() = 0;
			};
		}
	}
}

#endif