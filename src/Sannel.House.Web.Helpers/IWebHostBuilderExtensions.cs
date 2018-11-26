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
