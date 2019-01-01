/* Copyright 2019 Sannel Software, L.L.C.

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
using Xunit;

namespace Sannel.House.Models.Tests
{
	public class ErrorResponseModelTests
	{
		[Fact]
		public void ConstructorTest1()
		{
			var r = new ErrorResponseModel();
			Assert.Equal(200, r.Status);
			Assert.Null(r.Title);
			Assert.NotNull(r.Errors);
			Assert.Empty(r.Errors);
		}

		[Fact]
		public void ConstructorTest2()
		{
			var r = new ErrorResponseModel(3);
			Assert.Equal(3, r.Status);
			Assert.Null(r.Title);
			Assert.NotNull(r.Errors);
			Assert.Empty(r.Errors);

			r = new ErrorResponseModel(HttpStatusCode.Ambiguous);
			Assert.Equal(300, r.Status);
			Assert.Null(r.Title);
			Assert.NotNull(r.Errors);
			Assert.Empty(r.Errors);
		}

		[Fact]
		public void ConstructorTest3()
		{
			var r = new ErrorResponseModel(40, "Title 1");
			Assert.Equal(40, r.Status);
			Assert.Equal("Title 1", r.Title);
			Assert.NotNull(r.Errors);
			Assert.Empty(r.Errors);

			r = new ErrorResponseModel(HttpStatusCode.Conflict, "Title 1");
			Assert.Equal(409, r.Status);
			Assert.Equal("Title 1", r.Title);
			Assert.NotNull(r.Errors);
			Assert.Empty(r.Errors);
		}

		[Fact]
		public void ConstructorTest4()
		{
			var r = new ErrorResponseModel(40, "Title 1", "key", "value");
			Assert.Equal(40, r.Status);
			Assert.Equal("Title 1", r.Title);
			Assert.NotNull(r.Errors);
			Assert.Single(r.Errors);
			Assert.True(r.Errors.ContainsKey("key"));
			Assert.Equal(new string[]{ "value"}, r.Errors["key"]);

			r = new ErrorResponseModel(HttpStatusCode.Continue, "Title 1", "key", "value");
			Assert.Equal(100, r.Status);
			Assert.Equal("Title 1", r.Title);
			Assert.NotNull(r.Errors);
			Assert.Single(r.Errors);
			Assert.True(r.Errors.ContainsKey("key"));
			Assert.Equal(new string[]{ "value"}, r.Errors["key"]);
		}

		[Fact]
		public void ConstructorTest5()
		{
			var r = new ErrorResponseModel("Title 1", "key", "value");
			Assert.Equal(400, r.Status);
			Assert.Equal("Title 1", r.Title);
			Assert.NotNull(r.Errors);
			Assert.Single(r.Errors);
			Assert.True(r.Errors.ContainsKey("key"));
			Assert.Equal(new string[]{ "value"}, r.Errors["key"]);
		}
	}
}
