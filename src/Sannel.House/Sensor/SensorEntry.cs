using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Sensor
{
	public class SensorEntry
	{
		public virtual Guid Id { get; set; }

		public virtual string SensorType { get; set; }

		public virtual int DeviceId { get; set; }

		public virtual long? DeviceMacAddress
		{
			get;
			set;
		}

		public virtual Guid? DeviceUuid
		{
			get;
			set;
		}

		public virtual DateTime DateCreated { get; set; }

		public virtual IDictionary<string, object> ExtraElements { get; set; }

		public virtual IDictionary<string, float> Values { get; set; } = new Dictionary<string, float>();

	}
}
