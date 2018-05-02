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
using System.Linq;
using System.Reflection;

namespace Sannel.House.ToCPP
{
	class Program
	{
		static void Main(string[] args)
		{
			var license = $@"/* Copyright {DateTime.Now.Year} Sannel Software, L.L.C.

   Licensed under the Apache License, Version 2.0 (the ""License"");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

	   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an ""AS IS"" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.*/
/* This is generated code so probably best not to edit it */";

			var assem = new Assembly[] { Assembly.Load(new AssemblyName("Sannel.House")), Assembly.Load(new AssemblyName("Sannel.House.Sensor.Devices")) };
			var ass = System.Reflection.Assembly.GetEntryAssembly();
			var irun = typeof(IRun);

			foreach (var ti in ass.DefinedTypes)
			{
				if (ti.ImplementedInterfaces.Contains(irun))
				{
					var r = ass.CreateInstance(ti.FullName) as IRun;
					foreach (var a in assem)
					{
						r?.Generate(license, a);
					}
				}
			}
		}
	}
}