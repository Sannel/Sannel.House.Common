using System;
using System.Collections.Generic;
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
		}

		[Fact]
		public void ConstructorTest3()
		{
			var r = new ErrorResponseModel(40, "Title 1");
			Assert.Equal(40, r.Status);
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
		}
	}
}
