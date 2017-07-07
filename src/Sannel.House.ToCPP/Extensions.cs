using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.ToCPP
{
    public static class Extensions
    {
		public static StringBuilder AppendTabs(this StringBuilder builder, int count)
		{
			for(var i = 0; i < count; i++)
			{
				builder.Append('\t');
			}
			return builder;
		}
    }
}
