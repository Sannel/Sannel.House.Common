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
	public class PagedResponseModelTests
	{
		[Fact]
		public void ConstructorTest1()
		{
			var list = new string[] { "test", "test2" };
			var p = new PagedResponseModel<string>(200, "tmp", list, list.LongLength, 0, 10);
			Assert.Equal(200, p.Status);
			Assert.Equal("tmp", p.Title);
			Assert.Equal(list, p.Data);
			Assert.Equal(list.LongLength, p.TotalCount);
			Assert.Equal(0, p.Page);
			Assert.Equal(10, p.PageSize);
		}

		[Fact]
		public void ConstructorTest2()
		{
			var list = new string[] { "test", "test2" };
			var p = new PagedResponseModel<string>(HttpStatusCode.Ambiguous, "tmp", list, list.LongLength, 0, 10);
			Assert.Equal(300, p.Status);
			Assert.Equal("tmp", p.Title);
			Assert.Equal(list, p.Data);
			Assert.Equal(list.LongLength, p.TotalCount);
			Assert.Equal(0, p.Page);
			Assert.Equal(10, p.PageSize);
		}

		[Fact]
		public void ConstructorTest3()
		{
			var list = new string[] { "test", "test2" };
			var p = new PagedResponseModel<string>("tmp", list, list.LongLength, 0, 10);
			Assert.Equal(200, p.Status);
			Assert.Equal("tmp", p.Title);
			Assert.Equal(list, p.Data);
			Assert.Equal(list.LongLength, p.TotalCount);
			Assert.Equal(0, p.Page);
			Assert.Equal(10, p.PageSize);
		}

		[Fact]
		public void ConstructorTest4()
		{
			var list = new string[] { "test", "test2", "test3" };
			var pr = new MockPagedResults()
			{
				Data = list,
				Page = 2,
				PageSize = 20,
				TotalCount = list.LongLength
			};

			Assert.Throws<ArgumentNullException>("pagedResults", () => new PagedResponseModel<string>("title", (IPagedResponse<string>)null));

			var p = new PagedResponseModel<string>("tmp2", pr);
			Assert.Equal(200, p.Status);
			Assert.Equal("tmp2", p.Title);
			Assert.Equal(list, p.Data);
			Assert.Equal(pr.Page, p.Page);
			Assert.Equal(pr.PageSize, p.PageSize);
			Assert.Equal(pr.TotalCount, p.TotalCount);
		}

	}
}
