using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House
{
	public static class Defaults
	{
		public static class Development
		{
			public static readonly uint SERVER_PORT = 5000;
			public static readonly uint SENSOR_BROADCAST_PORT = 8175;
		}

		public static class Production
		{
			public static readonly uint SERVER_PORT = 6000;
			public static readonly uint SENSOR_BROADCAST_PORT = 8257;
		}
	}
}
