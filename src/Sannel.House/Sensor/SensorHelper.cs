using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Sensor
{
	public static class SensorHelper
	{
		public const string TEMPERATURE = "Temperature";
		public const string HUMIDITY = "Humidity";
		public const string PRESSURE = "Pressure";
		public const string WIND_SPEED = "WindSpeed";
		public const string WIND_DIRECTION = "WindDirection";
		public const string SOIL_MOISTURE = "SoilMoisture";
		public const string RAIN = "Rain";
		public const string LUX = "Lux";

		/// <summary>
		/// Creates a <typeparamref name="T"/> and fills in some basic information
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static T Create<T>(SensorTypes type)
			where T : SensorEntry, new()
		{
			var s = new T
			{
				DateCreated = DateTime.UtcNow,
				Id = Guid.NewGuid(),
				Type = type
			};
			return s;
		}

		/// <summary>
		/// Creates a <typeparamref name="T"/> and fills in some basic information
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type">The type.</param>
		/// <param name="macAddress">The mac address.</param>
		/// <returns></returns>
		public static T Create<T>(SensorTypes type, long macAddress)
			where T : SensorEntry, new()
		{
			var s = new T
			{
				DateCreated = DateTime.UtcNow,
				Id = Guid.NewGuid(),
				Type = type,
				DeviceMacAddress = macAddress
			};
			return s;
		}

		/// <summary>
		/// Creates a <typeparamref name="T"/> and fills in some basic information
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type">The type.</param>
		/// <param name="uuid">The UUID.</param>
		/// <returns></returns>
		public static T Create<T>(SensorTypes type, Guid uuid)
			where T : SensorEntry, new()
		{
			var s = new T
			{
				DateCreated = DateTime.UtcNow,
				Id = Guid.NewGuid(),
				Type = type,
				DeviceUuid = uuid
			};
			return s;
		}

		#region Temperature
		public static class Temperature
		{
			/// <summary>
			/// Creates a <typeparamref name="T"/> and fills in its Temperature Value
			/// </summary>
			/// <typeparam name="T">some type of SensorEntry</typeparam>
			/// <param name="temperature">The temperature.</param>
			/// <returns></returns>
			public static T Create<T>(float temperature)
				where T : SensorEntry, new()
			{
				var t = SensorHelper.Create<T>(SensorTypes.Temperature);
				t.Values[TEMPERATURE] = temperature;
				return t;
			}

			/// <summary>
			/// Creates a <typeparamref name="T"/> and fills in its Temperature Value
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="temperature">The temperature.</param>
			/// <param name="macAddress">The mac address.</param>
			/// <returns></returns>
			public static T Create<T>(float temperature, long macAddress)
				where T : SensorEntry, new()
			{

				var t = SensorHelper.Create<T>(SensorTypes.Temperature, macAddress);
				t.Values[TEMPERATURE] = temperature;
				return t;
			}

			/// <summary>
			/// Creates a <typeparamref name="T"/> and fills in its Temperature Value
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="temperature">The temperature.</param>
			/// <param name="uuid">The UUID.</param>
			/// <returns></returns>
			public static T Create<T>(float temperature, Guid uuid)
							where T : SensorEntry, new()
			{

				var t = SensorHelper.Create<T>(SensorTypes.Temperature, uuid);
				t.Values[TEMPERATURE] = temperature;
				return t;
			}

			/// <summary>
			/// Creates a SensorEntry and fill in its Temperature Value
			/// </summary>
			/// <param name="temperature">The temperature.</param>
			/// <returns></returns>
			public static SensorEntry Create(float temperature)
				=> Create<SensorEntry>(temperature);

			/// <summary>
			/// Creates a SensorEntry and fill in its Temperature Value
			/// </summary>
			/// <param name="temperature">The temperature.</param>
			/// <param name="macAddress">The mac address.</param>
			/// <returns></returns>
			public static SensorEntry Create(float temperature, long macAddress)
				=> Create<SensorEntry>(temperature, macAddress);

			/// <summary>
			/// Creates a SensorEntry and fill in its Temperature Value
			/// </summary>
			/// <param name="temperature">The temperature.</param>
			/// <param name="uuid">The UUID.</param>
			/// <returns></returns>
			public static SensorEntry Create(float temperature, Guid uuid)
				=> Create<SensorEntry>(temperature, uuid);
		}
		#endregion

		public class THP
		{
			/// <summary>
			/// Creates <typeparamref name="T"/> and fills in the THP info
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="temperature">The temperature.</param>
			/// <param name="humidity">The humidity.</param>
			/// <param name="pressure">The pressure.</param>
			/// <returns></returns>
			public static T Create<T>(float temperature, float humidity, float pressure)
				where T : SensorEntry, new()
			{
				var t = SensorHelper.Create<T>(SensorTypes.TemperatureHumidityPressure);
				t.Values[TEMPERATURE] = temperature;
				t.Values[HUMIDITY] = humidity;
				t.Values[PRESSURE] = pressure;
				return t;
			}

			/// <summary>
			/// Creates <typeparamref name="T"/> and fills in the THP info
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="temperature">The temperature.</param>
			/// <param name="humidity">The humidity.</param>
			/// <param name="pressure">The pressure.</param>
			/// <param name="macAddress">The mac address.</param>
			/// <returns></returns>
			public static T Create<T>(float temperature, float humidity, float pressure, long macAddress)
				where T : SensorEntry, new()
			{
				var t = SensorHelper.Create<T>(SensorTypes.TemperatureHumidityPressure, macAddress);
				t.Values[TEMPERATURE] = temperature;
				t.Values[HUMIDITY] = humidity;
				t.Values[PRESSURE] = pressure;
				return t;
			}

			/// <summary>
			/// Creates <typeparamref name="T"/> and fills in the THP info
			/// </summary>
			/// <typeparam name="T"></typeparam>
			/// <param name="temperature">The temperature.</param>
			/// <param name="humidity">The humidity.</param>
			/// <param name="pressure">The pressure.</param>
			/// <param name="uuid">The UUID.</param>
			/// <returns></returns>
			public static T Create<T>(float temperature, float humidity, float pressure, Guid uuid)
				where T : SensorEntry, new()
			{
				var t = SensorHelper.Create<T>(SensorTypes.TemperatureHumidityPressure, uuid);
				t.Values[TEMPERATURE] = temperature;
				t.Values[HUMIDITY] = humidity;
				t.Values[PRESSURE] = pressure;
				return t;
			}

			/// <summary>
			/// Creates a SensorEntry and fills in the THP info
			/// </summary>
			/// <param name="temperature">The temperature.</param>
			/// <param name="humidity">The humidity.</param>
			/// <param name="pressure">The pressure.</param>
			/// <returns></returns>
			public static SensorEntry Create(float temperature, float humidity, float pressure)
				=> Create<SensorEntry>(temperature, humidity, pressure);

			/// <summary>
			/// Creates a SensorEntry and fills in the THP info
			/// </summary>
			/// <param name="temperature">The temperature.</param>
			/// <param name="humidity">The humidity.</param>
			/// <param name="pressure">The pressure.</param>
			/// <param name="macAddress">The mac address.</param>
			/// <returns></returns>
			public static SensorEntry Create(float temperature, float humidity, float pressure, long macAddress)
				=> Create<SensorEntry>(temperature, humidity, pressure, macAddress);

			/// <summary>
			/// Creates a SensorEntry and fills in the THP info
			/// </summary>
			/// <param name="temperature">The temperature.</param>
			/// <param name="humidity">The humidity.</param>
			/// <param name="pressure">The pressure.</param>
			/// <param name="uuid">The UUID.</param>
			/// <returns></returns>
			public static SensorEntry Create(float temperature, float humidity, float pressure, Guid uuid)
				=> Create<SensorEntry>(temperature, humidity, pressure, uuid);
		}
	}
}
