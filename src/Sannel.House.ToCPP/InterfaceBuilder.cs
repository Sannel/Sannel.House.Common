using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

namespace Sannel.House.ToCPP
{
	public class InterfaceBuilder
	{
		protected int Tabs = 0;
		protected readonly StringBuilder Includes = new StringBuilder();
		protected readonly StringBuilder Defines = new StringBuilder();
		protected readonly StringBuilder Code = new StringBuilder();
		protected string FullNamespace = "";

		public Type ProcessType
		{
			get;
			set;
		}

		public InterfaceBuilder(Type type)
			=> this.ProcessType = type;

		protected virtual string GetCppType(Type t)
		{
			if (t == typeof(void))
			{
				return "void";
			}
			else if (t == typeof(byte))
			{
				return "uint8_t";
			}
			else if(t == typeof(ushort))
			{
				return "uint16_t";
			}
			else if(t == typeof(short))
			{
				return "int16_t";
			}
			else if(t == typeof(int))
			{
				return "int";
			}
			else if (t == typeof(double))
			{
				return "double";
			}
			else if (t == typeof(byte[]) || t.Name == "Byte[]&")
			{
				return "uint8_t*";
			}
			else if(t == typeof(long))
			{
				return "int64_t";
			}
			else if(t == typeof(ulong))
			{
				return "uint64_t";
			}

			return t.Name;
		}

		protected virtual void AddNamespace()
		{
			var namespaces = ProcessType.Namespace.Split('.');
			FullNamespace = string.Join("::", namespaces);
			foreach (var n in namespaces)
			{
				Code.AppendTabs(Tabs);
				Code.AppendLine($"namespace {n}");
				Code.AppendTabs(Tabs);
				Code.AppendLine("{");
				Tabs++;
			}
		}

		protected virtual bool IsIgnoredMethod(string name)
		{
			return false;
		}

		protected virtual void AddMethod(MethodInfo mi)
		{
			Code.AppendTabs(Tabs);
			Code.Append($"virtual {GetCppType(mi.ReturnType)} {mi.Name}(");
			var firstRun = true;
			foreach (var v in mi.GetParameters())
			{
				if (!firstRun)
				{
					Code.Append(", ");
				}

				Code.Append($"{GetCppType(v.ParameterType)} {v.Name}");

				firstRun = false;
			}
			Code.AppendLine(") = 0;");
		}

		protected virtual void AddField(FieldInfo fi)
		{

		}
		protected virtual void AddProperty(PropertyInfo pi)
		{

		}

		protected virtual void AddPublicMethods()
		{
			foreach (var m in ProcessType.GetMethods().Where(i => i.IsPublic))
			{
				if (!IsIgnoredMethod(m.Name))
				{
					AddMethod(m);
				}
			}
		}


		protected virtual void AddPublicFields()
		{
			foreach(var f in ProcessType.GetRuntimeFields().Where(i => i.IsPublic))
			{
				AddField(f);
			}
		}

		protected virtual void AddProperties()
		{
			foreach(var p in ProcessType.GetRuntimeProperties())
			{
				AddProperty(p);
			}
		}

		protected virtual void AddPrivateMethods()
		{
			foreach (var m in ProcessType.GetMethods().Where(i => i.IsPrivate))
			{
				if (!IsIgnoredMethod(m.Name))
				{
					AddMethod(m);
				}
			}
		}

		protected virtual void AddPrivateFields()
		{
			foreach(var f in ProcessType.GetRuntimeFields().Where(i => i.IsPrivate))
			{
				AddField(f);
			}
		}

		protected virtual void AddConstructor(ConstructorInfo ci)
		{

		}

		protected virtual void AddConstructors()
		{
			foreach(var c in ProcessType.GetConstructors())
			{
				AddConstructor(c);
			}
		}

		protected virtual void FixImpliments(List<Type> types)
		{

		}

		protected virtual void AddClass()
		{
			Code.AppendTabs(Tabs);
			Code.Append($"class {ProcessType.Name}");

			var implments = new List<Type>();
			if (ProcessType.BaseType != null)
			{
				implments.Add(ProcessType.BaseType);
			}
			implments.AddRange(ProcessType.GetInterfaces().Where(i => i != typeof(IDisposable)));

			FixImpliments(implments);

			var firstSent = false;
			if (implments.Count > 0)
			{

				Code.Append(" : ");
				foreach (var i in implments)
				{
					if (firstSent)
					{
						Code.Append(", ");
					}
					Code.Append($"public {i.Name}");
					firstSent = true;
				}
			}
			Code.AppendLine();
			Code.AppendTabs(Tabs);
			Code.AppendLine("{");
			Code.AppendTabs(Tabs);
			Code.AppendLine("public:");
			Tabs++;

			AddConstructors();
			AddPublicFields();
			AddPublicMethods();
			AddProperties();

			Tabs--;
			Code.AppendTabs(Tabs);
			Code.AppendLine("private:");
			Tabs++;

			AddPrivateMethods();
			AddPrivateFields();

			Code.AppendTabs(--Tabs);
			Code.AppendLine("};");


		}

		public virtual string GenerateCppHeader(string license)
		{
			Tabs = 0;
			Includes.Clear();
			Defines.Clear();
			Code.Clear();
			Includes.AppendLine(license);

			Includes.AppendLine($"#ifndef _{ProcessType.Name.ToUpper()}_H_");
			Includes.AppendLine($"#define _{ProcessType.Name.ToUpper()}_H_");
			Includes.AppendLine();

			Includes.AppendLine(@"#if defined(ARDUINO) && ARDUINO >= 100
	#include ""Arduino.h""
#else
	#include ""WProgram.h""
#endif");
			Includes.AppendLine();

			var att = ProcessType.GetCustomAttribute<Exportable>();
			if (!string.IsNullOrWhiteSpace(att.Includes))
			{
				Includes.AppendLine(att.Includes);
				Includes.AppendLine();
			}


			AddNamespace();
			AddClass();

			for (var q = Tabs - 1; q >= 0; q--)
			{
				Code.AppendTabs(q);
				Code.AppendLine("}");
			}

			Includes.AppendLine(Defines.ToString());
			Includes.AppendLine(Code.ToString());

			Includes.AppendLine("#endif");

			return Includes.ToString();
		}
	}
}
