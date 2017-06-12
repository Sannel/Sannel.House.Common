
#ifndef _SENSORTYPES_H_
#define _SENSORTYPES_H_

namespace Sannel
{
	namespace House
	{
#ifndef ARDUINO
		public 
#endif
		enum class SensorTypes : int
		{
			Test = 0,
			Temperature = 1
		};
	}
}

#endif