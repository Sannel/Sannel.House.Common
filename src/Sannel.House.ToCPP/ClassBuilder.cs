using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sannel.House.ToCPP
{
	public class ClassBuilder : InterfaceBuilder
	{
		protected StringBuilder MethodImplementation = new StringBuilder();
		protected IEnumerable<MethodDeclarationSyntax> Members;
		protected IEnumerable<ConstructorDeclarationSyntax> Constructors;
		public ClassBuilder(Type type) : base(type)
		{
		}

		protected override void AddMethod(MethodInfo mi)
		{
			Code.AppendTabs(Tabs);
			Code.Append($"{GetCppType(mi.ReturnType)} {mi.Name}(");
			MethodImplementation.Append($"{GetCppType(mi.ReturnType)} {ProcessType.Name}::{mi.Name}(");
			var firstRun = true;
			foreach (var v in mi.GetParameters())
			{
				if (!firstRun)
				{
					Code.Append(", ");
					MethodImplementation.Append(", ");
				}

				Code.Append($"{GetCppType(v.ParameterType)} {v.Name}");
				MethodImplementation.Append($"{GetCppType(v.ParameterType)}& {v.Name}");

				firstRun = false;
			}

			if (mi.IsVirtual)
			{
				Code.AppendLine(") override;");
			}
			else
			{
				Code.AppendLine(");");
			}
			MethodImplementation.AppendLine(")");

			var mDeclaration = Members?.FirstOrDefault(i => i.Identifier.Text == mi.Name);
			if (mDeclaration != null)
			{
				MethodImplementation.AppendLine(mDeclaration.Body.ToString());
			}
		}

		protected override void AddField(FieldInfo fi)
		{
			if (fi.Name.StartsWith("<"))
			{
				return;
			}

			if (fi.IsLiteral && !fi.IsInitOnly)
			{
				Defines.AppendLine($"#define {fi.Name} {fi.GetValue(null)}");
			}
			else
			{
				Code.AppendTabs(Tabs);
				if (fi.FieldType.IsInterface || fi.FieldType.IsClass)
				{
					Code.AppendLine($"{GetCppType(fi.FieldType)}* {fi.Name};");
				}
				else
				{
					Code.AppendLine($"{GetCppType(fi.FieldType)} {fi.Name};");
				}
			}
		}

		protected override void AddProperty(PropertyInfo pi)
		{
			Code.AppendTabs(Tabs);
			Code.AppendLine($"{GetCppType(pi.PropertyType)} {pi.Name};");
		}

		protected override void AddConstructor(ConstructorInfo ci)
		{
			Code.AppendTabs(Tabs);
			Code.Append($"{ProcessType.Name}(");
			MethodImplementation.Append($"{ProcessType.Name}::{ProcessType.Name}(");
			var firstRun = true;
			foreach (var v in ci.GetParameters())
			{
				if (!firstRun)
				{
					Code.Append(", ");
					MethodImplementation.Append(", ");
				}

				Code.Append($"{GetCppType(v.ParameterType)}& {v.Name}");
				MethodImplementation.Append($"{GetCppType(v.ParameterType)}& {v.Name}");

				firstRun = false;
			}

			if (ci.IsVirtual)
			{
				Code.AppendLine(") override;");
			}
			else
			{
				Code.AppendLine(");");
			}
			MethodImplementation.AppendLine(")");

			var mConstructor = Constructors?.FirstOrDefault(i => i.ParameterList.Parameters.Count == ci.GetParameters().Length);
			if (mConstructor != null)
			{
				MethodImplementation.AppendLine(mConstructor.Body.ToString());
			}
		}

		protected override bool IsIgnoredMethod(string name)
		{
			switch (name)
			{
				case "Dispose":
				case "ToString":
				case "Equals":
				case "GetHashCode":
				case "GetType":
				case "GetFileName":
					return true;
			}

			return name?.StartsWith("set_") == true 
				|| name?.StartsWith("get_") == true
				|| name?.StartsWith("<") == true;
		}

		protected override void FixImpliments(List<Type> types)
		{
			types.Remove(typeof(object));

			var list = new List<Type>();
			list.AddRange(types);

			foreach (var t in list)
			{
				if (t.BaseType != null && t.BaseType != typeof(object))
				{
					types.Remove(t.BaseType);
				}
				else
				{
					foreach (var i in t.GetInterfaces())
					{
						types.Remove(i);
					}
				}
			}
		}
		public override string GenerateCppHeader(string license)
		{
			var method = ProcessType.GetMethod("GetFileName");
			if (method != null && method.IsStatic)
			{
				var obj = method.Invoke(null, new object[] { });

				var fileName = obj?.ToString();

				if (!string.IsNullOrWhiteSpace(fileName))
				{
					var content = File.ReadAllText(fileName);
					var tree = CSharpSyntaxTree.ParseText(content);

					Members = tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>();
					Constructors = tree.GetRoot().DescendantNodes().OfType<ConstructorDeclarationSyntax>();
				}
			}

			MethodImplementation.Clear();

			var s = base.GenerateCppHeader(license);
			MethodImplementation.Insert(0, $@"#include ""{ProcessType.Name}.h""
using namespace {FullNamespace};
");

			return s;
		}

		public virtual string GetCpp(string license)
		{
			return $"{license}\r\n{MethodImplementation}";
		}
	}
}
