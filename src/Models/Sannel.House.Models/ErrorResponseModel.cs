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

namespace Sannel.House.Models
{
	/// <summary>
	/// The Error Response Model
	/// </summary>
	/// <seealso cref="Sannel.House.Models.ResponseModel" />
	public class ErrorResponseModel : ResponseModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorResponseModel"/> class.
		/// </summary>
		public ErrorResponseModel()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorResponseModel"/> class.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		public ErrorResponseModel(int statusCode) : base(statusCode)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorResponseModel"/> class.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		/// <param name="title">The title.</param>
		public ErrorResponseModel(int statusCode, string title): base(statusCode, title)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorResponseModel"/> class.
		/// </summary>
		/// <param name="statusCode">The status code.</param>
		/// <param name="title">The title.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public ErrorResponseModel(int statusCode, string title, string key, string value) : base(statusCode, title) 
			=> Errors.Add(key, new string[] { value });


		[JsonProperty("errors")]
		public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();

	}
}
