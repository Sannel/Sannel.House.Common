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
	public class EnumGenerator : IRun
	{
		private void generateEnum(string license, Type type)
		{
			var names = Enum.GetNames(type);
			var values = Enum.GetValues(type);

			var builder = new StringBuilder();
			builder.AppendLine(license);
			builder.AppendLine();
			builder.AppendLine($"#ifndef _{type.Name.ToUpper()}_");
			builder.AppendLine($"#define _{type.Name.ToUpper()}_");

			var namespaces = type.Namespace.Split('.');

			var tabs = 0;
			foreach (var n in namespaces)
			{
				builder.AppendTabs(tabs);

				builder.AppendLine($"namespace {n}");

				builder.AppendTabs(tabs);
				builder.AppendLine("{");

				tabs++;
			}

			builder.AppendTabs(tabs);
			builder.AppendLine($"enum class {type.Name} : long");
			builder.AppendTabs(tabs);
			builder.AppendLine("{");
			tabs++;

			for(var i = 0; i < names.Length; i++)
			{
				builder.AppendTabs(tabs);
				builder.Append($"{names[i]} = {(int)values.GetValue(i)}");

				if(i + 1 < names.Length)
				{
					builder.AppendLine(",");
				}
				else
				{
					builder.AppendLine();
				}
			}

			tabs--;
			builder.AppendTabs(tabs);
			builder.AppendLine("};");
			tabs--;
			
			for(;tabs >= 0; tabs--)
			{
				builder.AppendTabs(tabs);
				builder.AppendLine("}");
			}

			builder.AppendLine("#endif");

			var path = Path.GetFullPath("..\\Sannel\\src");

			Directory.CreateDirectory(path);

			File.WriteAllText(Path.Combine(path, $"{type.Name}.h"), builder.ToString());

		}

		public void Generate(string license, Assembly assembly)
		{
			var en = typeof(Enum);
			foreach(var type in assembly.ExportedTypes)
			{
				if (en.IsAssignableFrom(type))
				{
					generateEnum(license, type);
				}
			}

		}
	}
}
