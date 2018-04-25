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
	[Exportable]
	public interface IWireDevice : IDisposable
	{
		void Write(byte b);
		void Write(byte b1, byte b2);
		void Write(byte b1, byte b2, byte b3);
		void Write(ref byte[] bytes, int length);
		byte WriteRead(byte write);
		void WriteRead(byte write, ref byte[] read, int length);
		uint Read(ref byte[] read, int length);
		byte ReadByte();
	}
}