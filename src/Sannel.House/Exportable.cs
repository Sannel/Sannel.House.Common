using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House
{
	/// <summary>
	/// Attribute to tell the ToCPP code to export the class or interface to c++
	/// </summary>
	/// <seealso cref="System.Attribute" />
	public class Exportable : Attribute
	{
		public string Includes { get; set; }
	}
}
