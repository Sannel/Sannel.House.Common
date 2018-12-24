using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Models.Tests
{
	public class MockPagedResults : IPagedResults<string>
	{
		public IEnumerable<string> Data { get; set; }
		public long TotalCount { get; set; }
		public long Page { get; set; }
		public int PageSize { get; set; }
	}
}
