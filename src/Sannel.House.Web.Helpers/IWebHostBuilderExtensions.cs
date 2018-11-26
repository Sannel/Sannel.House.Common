using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sannel.House.Web
{
	public static class IWebHostBuilderExtensions
	{
		/// <summary>Configure the Web Host Builder to include app_config/<paramref name="appSettingsFileName"/></summary>
		/// <param name="builder">The Builder</param>
		/// <param name="appSettingsFileName">the file in the app_config directory</param>
		/// <param name="optional">Is this config optional</param>
		/// <param name="reloadOnChange">Auto reload on Change</param>
		public static IWebHostBuilder ConfigureAppConfig(this IWebHostBuilder builder, 
				string appSettingsFileName="appsettings.json",
				bool optional=false,
				bool reloadOnChange=true)
			=> builder.ConfigureAppConfiguration(c =>
			{
				c.AddJsonFile(Path.Combine("app_config", appSettingsFileName), optional, reloadOnChange);
			});
	}
}
