using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Sensor
{
	[Exportable]
	public interface ILightSensor
	{
		float GetLuxLevel();
	}
}
