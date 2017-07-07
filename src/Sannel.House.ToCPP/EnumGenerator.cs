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
			builder.AppendLine("#pragma once");

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
			builder.AppendLine($"enum class {type.Name} : int");
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

			var path = Path.GetFullPath("..\\Sannel_House");
			foreach(var name in namespaces)
			{
				path = Path.Combine(path, name);
			}

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
