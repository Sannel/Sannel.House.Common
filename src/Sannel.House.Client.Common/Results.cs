/* Copyright 2018 Sannel Software, L.L.C.
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at
      http://www.apache.org/licenses/LICENSE-2.0
   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.*/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Client
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Results<T>
		where T : class
	{
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Results{T}"/> is a success.
		/// </summary>
		/// <value>
		///   <c>true</c> if success; otherwise, <c>false</c>.
		/// </value>
		public bool Success { get; set; }
		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>
		/// The message.
		/// </value>
		public string Message { get; set; }
		/// <summary>
		/// Gets or sets the status code.
		/// </summary>
		/// <value>
		/// The status code.
		/// </value>
		public HttpStatusCode Status { get; set; }
		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>
		/// The data.
		/// </value>
		public T Data { get; set; }
		/// <summary>
		/// Gets the errors.
		/// </summary>
		/// <value>
		/// The errors.
		/// </value>
		public Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();
		/// <summary>
		/// Gets or sets the trace identifier.
		/// </summary>
		/// <value>
		/// The trace identifier.
		/// </value>
		public string TraceId { get; set; }

		/// <summary>
		/// Fills this object with data from a asp.net core validation errors.
		/// </summary>
		/// <param name="jsonString">The json string.</param>
		/// <returns></returns>
		public Task<bool> FillWithValidationErrorsAsync(string jsonString) 
			=> Task.Run(() =>
		{
			try
			{
				if(jsonString == null)
				{
					return false;
				}

				var obj = JsonConvert.DeserializeObject<ValidationResult>(jsonString);
				if (!string.IsNullOrWhiteSpace(obj.Title) &&
					obj.Status > 0)
				{
					Message = obj.Title;
					TraceId = obj.TraceId;
					Status = (HttpStatusCode)obj.Status;
					foreach (var kv in obj.Errors)
					{
						Errors.Add(kv.Key, string.Join(",", kv.Value));
					}
					return true;
				}

				return false;
			}
			catch(JsonSerializationException)
			{
				return false;
			}
		});
		/*"errors": {
		"Name": [
			"The Name field is required."
		]
	},
	"title": "One or more validation errors occurred.",
	"status": 400,
	"traceId": "0HLIURDPH1MOO:00000001"*/
	}

	internal class ValidationResult
	{
		public Dictionary<string, string[]> Errors { get; set; }
		public string Title { get; set; }
		public int Status { get; set; }
		public string TraceId { get; set; }
	}
}

