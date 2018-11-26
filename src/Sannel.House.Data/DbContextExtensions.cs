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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Sannel.House.Data
{
	public static class DbContextExtensions
	{
		/// <summary>Waits for server.</summary>
		/// <param name="context">The context.</param>
		/// <param name="logger">The logger.</param>
		/// <param name="delayBetweenRetry">The delay between retry.</param>
		/// <param name="retryCount">The retry count.</param>
		/// <returns>True if it was able to connect false if it was not able to.</returns>
		public static bool WaitForServer(this DbContext context, ILogger logger, int delayBetweenRetry=1000, int retryCount=100)
		{
			var retry = 0;
			var connection = context.Database.GetDbConnection();
			while (connection.State == System.Data.ConnectionState.Closed && retry <= retryCount)
			{
				try
				{
					connection.Open();
				}
				catch (DbException ex)
				{
					logger.LogError(ex, "Exception connecting to server Delaying and trying again");
					retry++;
					Task.Delay(delayBetweenRetry).Wait();
				}
			}
			if (retry >= retryCount)
			{
				logger.LogCritical("Unable to establish connectiono to server");
				return false;
			}
			else
			{
				logger.LogInformation("Was able to establish connection to server");
				return true;
			}
		}
	}
}
