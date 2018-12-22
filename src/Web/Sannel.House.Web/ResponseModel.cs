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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Web
{
	/// <summary>
	/// The base response model for api responses
	/// </summary>
	public class ResponseModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ResponseModel"/> class.
		/// </summary>
		public ResponseModel()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ResponseModel"/> class.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		public ResponseModel(int statusCode) 
			=> Status = statusCode;

		/// <summary>
		/// Initializes a new instance of the <see cref="ResponseModel"/> class.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		/// <param name="title">The title.</param>
		public ResponseModel(int statusCode, string title) : this(statusCode) 
			=> Title = title;

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>
		/// The status.
		/// </value>
		[JsonProperty("status")]
		public int Status { get; set; } = 200;

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		[JsonProperty("title")]
		public string Title { get; set; }
	}

	/// <summary>
	/// The base response model with data api responses
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ResponseModel<T> : ResponseModel
		where T : class
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ResponseModel{T}"/> class.
		/// </summary>
		public ResponseModel()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ResponseModel{T}"/> class.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		public ResponseModel(int statusCode) : base(statusCode)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ResponseModel{T}"/> class.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		/// <param name="title">The title.</param>
		public ResponseModel(int statusCode, string title) : base(statusCode, title)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ResponseModel{T}"/> class.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		/// <param name="title">The title.</param>
		/// <param name="data">The data.</param>
		public ResponseModel(int statusCode, string title, T data) : base(statusCode, title)
			=> Data = data;


		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>
		/// The data.
		/// </value>
		[JsonProperty("data")]
		public T Data { get; set; }
	}
}
