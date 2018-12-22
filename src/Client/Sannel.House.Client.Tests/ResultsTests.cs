using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sannel.House.Client.Tests
{
	public class ResultsTests
	{
		[Fact]
		public async Task FillWithValidationErrorsAsyncTest()
		{
			var results = new Results<object>();
			var success = await results.FillWithValidationErrorsAsync(@"
{
	""errors"": {
		""Name"": [
			""The Name field is required."",
			""Name does not contain test.""
		]
	},
	""title"": ""One or more validation errors occurred."",
	""status"": 400,
	""traceId"": ""0HLIURDPH1MOO:00000001""}");

			Assert.True(success);
			Assert.Equal(HttpStatusCode.BadRequest, results.Status);
			Assert.Equal("0HLIURDPH1MOO:00000001", results.TraceId);
			Assert.Single(results.Errors);
			Assert.True(results.Errors.ContainsKey("Name"));
			Assert.Equal("The Name field is required.,Name does not contain test.", results.Errors["Name"]);

			results = new Results<object>();

			success = await results.FillWithValidationErrorsAsync(@"{""message"":""error""}");
			Assert.False(success);
			Assert.Equal(default(HttpStatusCode), results.Status);

			success = await results.FillWithValidationErrorsAsync(@"{");
			Assert.False(success);

			success = await results.FillWithValidationErrorsAsync(null);
			Assert.False(success);
		}

		public class TestObject
		{
			public int Prop1 { get; set; }
			public string Prop2 { get; set; }
		}

		[Fact]
		public async Task SafelyDeserializeAndSetDataAsyncTest()
		{
			var results = new Results<TestObject>();
			var r = await results.SafelyDeserializeAndSetDataAsync(null);
			Assert.Null(r);
			Assert.Null(results.Data);
			Assert.Null(results.Exception);

			r = await results.SafelyDeserializeAndSetDataAsync("{");
			Assert.Null(r);
			Assert.Null(results.Data);
			Assert.IsType<JsonSerializationException>(results.Exception);
			results.Exception = null;

			r = await results.SafelyDeserializeAndSetDataAsync("{}");
			Assert.NotNull(r);
			Assert.Equal(default(int), r.Prop1);
			Assert.Equal(default(string), r.Prop2);
			Assert.Null(results.Exception);
			Assert.NotNull(results.Data);

			r = await results.SafelyDeserializeAndSetDataAsync(@"{""Prop1"": 2}");
			Assert.NotNull(r);
			Assert.Equal(2, r.Prop1);
			Assert.Equal(default(string), r.Prop2);
			Assert.Null(results.Exception);
			Assert.NotNull(results.Data);

			r = await results.SafelyDeserializeAndSetDataAsync(@"{""Prop1"": 2, ""Prop2"":""test""}");
			Assert.NotNull(r);
			Assert.Equal(2, r.Prop1);
			Assert.Equal("test", r.Prop2);
			Assert.Null(results.Exception);
			Assert.Equal(2, results.Data.Prop1);
			Assert.Equal("test", results.Data.Prop2);
			Assert.NotNull(results.Data);

			r = await results.SafelyDeserializeAndSetDataAsync("");
			Assert.Null(r);
			Assert.Null(results.Data);
		}
	}
}
