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
using System.IO;
using System.Reflection;
using System.Text;

namespace Sannel.House.ToCPP
{
	public class GenerateSannelDefines : IRun
	{
		public void Generate(string license, Assembly sannelHouseAssembly)
		{
			var builder = new StringBuilder(license);
			builder.AppendLine();
			builder.AppendLine(@"#ifndef _SANNELDEFINES_
#define _SANNELDEFINES_

#ifdef DEBUG");
			var t = typeof(Defaults.Development);

			foreach(var field in t.GetFields())
			{
				builder.AppendLine($"#define {field.Name} {field.GetValue(null)}");
			}

			builder.AppendLine("#else");

			t = typeof(Defaults.Production);

			foreach(var field in t.GetFields())
			{
				builder.AppendLine($"#define {field.Name} {field.GetValue(null)}");
			}

			builder.AppendLine("#endif");
			builder.AppendLine("#endif");

			var path = Path.GetFullPath("..\\..\\..\\..\\Sannel\\src\\SannelDefines.h");
			File.WriteAllText(path, builder.ToString());
		}
	}
}
