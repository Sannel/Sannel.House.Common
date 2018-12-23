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
using Xunit;

namespace Sannel.House.Models.Tests
{
	public class ResponseModelTests
	{
		[Fact]
		public void ConstructorTest1()
		{
			var r = new ResponseModel();
			Assert.Equal(200, r.Status);
			Assert.Null(r.Title);
		}

		[Fact]
		public void ConstructorTest2()
		{
			var r = new ResponseModel(300);
			Assert.Equal(300, r.Status);
			Assert.Null(r.Title);

			r = new ResponseModel(HttpStatusCode.Gone);
			Assert.Equal(410, r.Status);
			Assert.Equal(HttpStatusCode.Gone, r.StatusCode);
			Assert.Null(r.Title);
		}
		
		[Fact]
		public void ConstructorTest3()
		{
			var r = new ResponseModel(400, "Title 1");
			Assert.Equal(400, r.Status);
			Assert.Equal("Title 1", r.Title);

			r = new ResponseModel(HttpStatusCode.LengthRequired, "Title 1");
			Assert.Equal(411, r.Status);
			Assert.Equal("Title 1", r.Title);
		}

		[Fact]
		public void ConstructorTest4()
		{
			var r = new ResponseModel<string>();
			Assert.Equal(200, r.Status);
			Assert.Null(r.Title);
			Assert.Null(r.Data);

			var r1 = new ResponseModel<int>();
			Assert.Equal(200, r1.Status);
			Assert.Null(r1.Title);
			Assert.Equal(default(int), r1.Data);
		}

		[Fact]
		public void ConstructorTest5()
		{
			var r = new ResponseModel<string>(300);
			Assert.Equal(300, r.Status);
			Assert.Null(r.Title);
			Assert.Null(r.Data);

			r = new ResponseModel<string>(HttpStatusCode.MultiStatus);
			Assert.Equal(207, r.Status);
			Assert.Null(r.Title);
			Assert.Null(r.Data);
		}
		
		[Fact]
		public void ConstructorTest6()
		{
			var r = new ResponseModel<string>(400, "Title 1");
			Assert.Equal(400, r.Status);
			Assert.Equal("Title 1", r.Title);
			Assert.Null(r.Data);

			r = new ResponseModel<string>(HttpStatusCode.NetworkAuthenticationRequired, "Title 1");
			Assert.Equal(511, r.Status);
			Assert.Equal("Title 1", r.Title);
			Assert.Null(r.Data);
		}
		
		[Fact]
		public void ConstructorTest7()
		{
			var r = new ResponseModel<string>(400, "Title 1", "lemon");
			Assert.Equal(400, r.Status);
			Assert.Equal("Title 1", r.Title);
			Assert.Equal("lemon", r.Data);

			var r1 = new ResponseModel<int>(HttpStatusCode.NoContent, "Title 2", 22);
			Assert.Equal(204, r1.Status);
			Assert.Equal("Title 2", r1.Title);
			Assert.Equal(22, r1.Data);
		}

		[Fact]
		public void ConstructorTest8()
		{
			var r = new ResponseModel<int>("Test 1", 23);
			Assert.Equal(200, r.Status);
			Assert.Equal("Test 1", r.Title);
			Assert.Equal(23, r.Data);
		}

		[Fact]
		public void ConstructorTest9()
		{
			var r = new ResponseModel<long>(300L);
			Assert.Equal(200, r.Status);
			Assert.Null(r.Title);
			Assert.Equal(300, r.Data);
		}
	}
}
