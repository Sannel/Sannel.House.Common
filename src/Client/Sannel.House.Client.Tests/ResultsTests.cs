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
		public async Task CreateFromJsonExceptionsTest()
		{
			Assert.Throws<ArgumentNullException>("json", () => Results<string>.CreateFromJson(null));
			await Assert.ThrowsAsync<ArgumentNullException>("json", () => Results<string>.CreateFromJsonAsync(null));

			var results = Results<object>.CreateFromJson("{");
			Assert.False(results.Success);
			Assert.IsType<JsonSerializationException>(results.Exception);

			results = await Results<object>.CreateFromJsonAsync("{");
			Assert.False(results.Success);
			Assert.IsType<JsonSerializationException>(results.Exception);
		}

		[Fact]
		public async Task CreateFromJsonTest()
		{
			Results<object> results;
			results = Results<object>.CreateFromJson(@"
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

			Assert.True(results.Success);
			Assert.Equal(HttpStatusCode.BadRequest, results.StatusCode);
			Assert.Equal("0HLIURDPH1MOO:00000001", results.TraceId);
			Assert.Single(results.Errors);
			Assert.True(results.Errors.ContainsKey("Name"));
			Assert.Equal("The Name field is required.,Name does not contain test.", string.Join(',',results.Errors["Name"]));

			results = await Results<object>.CreateFromJsonAsync(@"
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

			Assert.True(results.Success);
			Assert.Equal(HttpStatusCode.BadRequest, results.StatusCode);
			Assert.Equal("0HLIURDPH1MOO:00000001", results.TraceId);
			Assert.Single(results.Errors);
			Assert.True(results.Errors.ContainsKey("Name"));
			Assert.Equal("The Name field is required.,Name does not contain test.", string.Join(',',results.Errors["Name"]));

		}
	}
}
