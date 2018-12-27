/* Copyright 2018 Sannel Software, L.L.C.

   Licensed under the Apache License, Version 2.0 (the ""License"");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

	   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an ""AS IS"" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.*/
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Sannel.House.Models
{
	public class PagedResponseModel<T> : ResponseModel<IEnumerable<T>>, IPagedResults<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PagedResponseModel{T}"/> class.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		/// <param name="title">The title.</param>
		/// <param name="data">The data.</param>
		/// <param name="totalCount">The total count.</param>
		/// <param name="page">The page.</param>
		/// <param name="pageSize">Size of the page.</param>
		public PagedResponseModel(int statusCode, 
			string title, 
			IEnumerable<T> data, 
			long totalCount, 
			long page, 
			int pageSize) : base(statusCode, title, data)
		{
			TotalCount = totalCount;
			Page = page;
			PageSize = pageSize;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PagedResponseModel{T}"/> class.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="title">The title.</param>
		/// <param name="data">The data.</param>
		/// <param name="totalCount">The total count.</param>
		/// <param name="page">The page.</param>
		/// <param name="pageSize">Size of the page.</param>
		public PagedResponseModel(HttpStatusCode code,
			string title,
			IEnumerable<T> data,
			long totalCount,
			long page,
			int pageSize) : this((int)code, title, data, totalCount, page, pageSize)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PagedResponseModel{T}"/> class.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="data">The data.</param>
		/// <param name="totalCount">The total count.</param>
		/// <param name="page">The page.</param>
		/// <param name="pageSize">Size of the page.</param>
		public PagedResponseModel(string title, 
			IEnumerable<T> data, 
			long totalCount, 
			long page, 
			int pageSize) : this(HttpStatusCode.OK, title, data, totalCount, page, pageSize)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PagedResponseModel{T}"/> class.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		/// <param name="title">The title.</param>
		/// <param name="pagedResults">The paged results.</param>
		public PagedResponseModel(int statusCode, string title, IPagedResults<T> pagedResults)
			: this(statusCode, 
				title, 
				(pagedResults ?? throw new ArgumentNullException(nameof(pagedResults))).Data, 
				pagedResults.TotalCount, 
				pagedResults.Page, 
				pagedResults.PageSize)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PagedResponseModel{T}"/> class.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="title">The title.</param>
		/// <param name="pagedResults">The paged results.</param>
		public PagedResponseModel(HttpStatusCode code, string title, IPagedResults<T> pagedResults)
			: this((int)code, title, pagedResults)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PagedResponseModel{T}"/> class.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="pagedResults">The paged results.</param>
		public PagedResponseModel(string title, IPagedResults<T> pagedResults) 
			: this(HttpStatusCode.OK, 
				title,
				pagedResults)
		{

		}
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
