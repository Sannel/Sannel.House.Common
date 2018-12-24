using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Models
{
	public interface IPagedResults<T>
	{
		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>
		/// The data.
		/// </value>
		IEnumerable<T> Data { get; set; }
		/// <summary>
		/// The Total number of items returned by the filter
		/// </summary>
		/// <value>
		/// The total count.
		/// </value>
		long TotalCount { get; set; }
		/// <summary>
		/// Gets or sets the page.
		/// </summary>
		/// <value>
		/// The page.
		/// </value>
		long Page { get; set; }
		/// <summary>
		/// Gets or sets the size of the page.
		/// </summary>
		/// <value>
		/// The size of the page.
		/// </value>
		int PageSize { get; set; }
	}
}
