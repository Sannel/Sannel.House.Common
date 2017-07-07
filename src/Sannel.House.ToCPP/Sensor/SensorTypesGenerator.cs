using Sannel.House.Sensor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.ToCPP.Sensor
{
	public class SensorTypesGenerator : IRun
	{
		public void Generate(string license)
		{
			var builder = new StringBuilder();
			builder.AppendLine(license);

			var q = typeof(SensorTypes);
			var namespaces = q.Namespace.Split('.');

			var tabs = 0;
			foreach(var n in namespaces)
			{
				for(var i = 0; i < tabs; i++)
				{
					builder.Append('\t');
				}

				builder.AppendLine($"namespace {n}");

				for(var i = 0; i < tabs; i++)
				{
					builder.Append('\t');
				}
				builder.AppendLine("{");
				tabs++;
			}
			
			for(var i = 0; i < tabs; i++)
			{
				builder.Append('\t');
			}

		}
	}
}
