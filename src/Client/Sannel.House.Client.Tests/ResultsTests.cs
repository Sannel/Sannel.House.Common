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
	}
}
