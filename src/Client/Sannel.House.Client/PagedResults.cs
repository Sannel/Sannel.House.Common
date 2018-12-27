using Sannel.House.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Client
{
	public class PagedResults<T> : Results<IEnumerable<T>>, IPagedResults<T>
	{
		/// <summary>
		/// The Total number of items returned
		/// </summary>
		/// <value>
		/// The total count.
		/// </value>
		public long TotalCount { get; set; }
		/// <summary>
		/// Gets or sets the page.
		/// </summary>
		/// <value>
		/// The page.
		/// </value>
		public long Page { get; set; }
		/// <summary>
		/// Gets or sets the size of the page.
		/// </summary>
		/// <value>
		/// The size of the page.
		/// </value>
		public int PageSize { get; set; }
	}
}
