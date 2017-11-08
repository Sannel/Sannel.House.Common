using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House
{
	public static class Defaults
	{
		public static class Development
		{
			public const uint SENSOR_BROADCAST_PORT = 8175;
		}

		public static class Production
		{
			public const uint SENSOR_BROADCAST_PORT = 8257;
		}
	}
}
