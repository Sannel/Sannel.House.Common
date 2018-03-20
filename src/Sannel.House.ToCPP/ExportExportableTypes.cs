using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Sannel.House.ToCPP
{
	public class ExportExportableTypes : IRun
	{
		private InterfaceBuilder interfaceBuilder = new InterfaceBuilder(typeof(string));
		private ClassBuilder classBuilder = new ClassBuilder(typeof(string));
		public void Generate(string license, Assembly sannelHouseAssembly)
		{
			foreach(var type in sannelHouseAssembly.GetTypes())
			{
				if(type.GetCustomAttribute<Exportable>() != null)
				{
					if (type.IsInterface)
					{
						interfaceBuilder.ProcessType = type;
						var code = interfaceBuilder.GenerateCppHeader(license);
						var path = Path.GetFullPath($"..\\..\\..\\..\\Sannel\\src\\{type.Name}.h");
						File.WriteAllText(path, code);
					}
					else if (type.IsClass)
					{
						classBuilder.ProcessType = type;
						var code = classBuilder.GenerateCppHeader(license);
						var path = Path.GetFullPath($"..\\..\\..\\..\\Sannel\\src\\{type.Name}.h");
						File.WriteAllText(path, code);
						//path = Path.GetFullPath($"..\\Sannel\\src\\{type.Name}.cpp");
						//File.WriteAllText(path, classBuilder.GetCpp(license));
					}
				}
			}
		}
	}
}
