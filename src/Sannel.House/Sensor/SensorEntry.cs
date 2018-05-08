/* Copyright 2018 Sannel Software, L.L.C.

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
	public class SensorEntry
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public virtual Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the type of the sensor.
		/// </summary>
		/// <value>
		/// The type of the sensor.
		/// </value>
		public virtual string SensorType { get; set; }

		/// <summary>
		/// Sets the SensorType from the passed SensorTypes
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public virtual SensorTypes Type
		{
			set => SensorType = value.ToString();
		}

		/// <summary>
		/// Gets or sets the device identifier.
		/// </summary>
		/// <value>
		/// The device identifier.
		/// </value>
		public virtual int DeviceId { get; set; }

		/// <summary>
		/// Gets or sets the device mac address.
		/// </summary>
		/// <value>
		/// The device mac address.
		/// </value>
		public virtual long? DeviceMacAddress
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the device UUID.
		/// </summary>
		/// <value>
		/// The device UUID.
		/// </value>
		public virtual Guid? DeviceUuid
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the date created.
		/// </summary>
		/// <value>
		/// The date created.
		/// </value>
		public virtual DateTime DateCreated { get; set; }

		/// <summary>
		/// Gets or sets the extra elements.
		/// </summary>
		/// <value>
		/// The extra elements.
		/// </value>
		public virtual IDictionary<string, object> ExtraElements { get; set; }

		/// <summary>
		/// Gets or sets the values.
		/// </summary>
		/// <value>
		/// The values.
		/// </value>
		public virtual IDictionary<string, float> Values { get; set; } = new Dictionary<string, float>();

	}
}
