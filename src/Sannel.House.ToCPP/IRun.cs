using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Sannel.House.ToCPP
{
    public interface IRun
    {
		void Generate(string license, Assembly sannelHouseAssembly);
    }
}
