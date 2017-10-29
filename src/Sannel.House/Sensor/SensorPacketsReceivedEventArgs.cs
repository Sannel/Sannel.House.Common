/* Copyright 2017 Sannel Software, L.L.C.

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
	public class SensorPacketsReceivedEventArgs : EventArgs
	{
		/// <summary>
		/// Gets or sets the mac address.
		/// </summary>
		/// <value>
		/// The mac address.
		/// </value>
		public long MacAddress { get; set; }
		/// <summary>
		/// Gets the packets.
		/// </summary>
		/// <value>
		/// The packets.
		/// </value>
		public IList<SensorPacket> Packets { get; } = new List<SensorPacket>();
	}
}