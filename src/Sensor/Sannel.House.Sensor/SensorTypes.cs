/* Copyright 2019 Sannel Software, L.L.C.

Licensed under the Apache License, Version 2.0 (the ""License"");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an ""AS IS"" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Sensor
{
	/// <summary>
	/// Sensor Types
	/// </summary>
	public enum SensorTypes
	{
		/// <summary>
		/// Unknown
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// Temperature
		/// </summary>
		Temperature = 1,
		/// <summary>
		/// Humidity
		/// </summary>
		Humidity = 2,
		/// <summary>
		/// Pressure
		/// </summary>
		Pressure = 3,
		/// <summary>
		/// Wind speed
		/// </summary>
		WindSpeed = 4,
		/// <summary>
		/// Wind direction
		/// </summary>
		WindDirection = 5,
		/// <summary>
		/// Soil moisture
		/// </summary>
		SoilMoisture = 6,
		/// <summary>
		/// Rain
		/// </summary>
		Rain = 7,
		/// <summary>
		/// Lux
		/// </summary>
		Lux = 8,
		/// <summary>
		/// Soil Temperature
		/// </summary>
		SoilTemperature = 9,
	}
}
