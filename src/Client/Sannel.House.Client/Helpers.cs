using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sannel.House.Client
{
	public static class Helpers
	{
		public static void RegisterClient(IServiceCollection service, string clientName, string version)
		{
			service.AddHttpClient(clientName, (i) =>
			{
				i.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
				i.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue(clientName, version));
			});
		}
	}
}
