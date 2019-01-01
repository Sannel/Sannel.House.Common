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
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Sannel.House.Data
{
	public static class Extensions
	{
		/// <summary>
		/// Adds the database health check.
		/// </summary>
		/// <typeparam name="T">The DbContext your using in your app</typeparam>
		/// <param name="builder">The builder IHealthCheckBuilder.</param>
		/// <param name="name">The name for AddCheck</param>
		/// <param name="queryAsync">The query asynchronous. i.e. context.Devices.Take(1).CountAsync()</param>
		/// <returns></returns>
		public static IHealthChecksBuilder AddDbHealthCheck<T>(this IHealthChecksBuilder builder, string name, Func<T, Task> queryAsync)
			where T : DbContext
		{
			builder.Services.AddTransient(p => new DbHealthCheck<T>(p, queryAsync ?? throw new ArgumentNullException(nameof(queryAsync))));
			return builder.AddCheck<DbHealthCheck<T>>(name);
		}

		/// <summary>
		/// Adds the database health check.
		/// </summary>
		/// <typeparam name="T">The DbContext your using in your app</typeparam>
		/// <param name="builder">The builder IHealthCheckBuilder.</param>
		/// <param name="name">The name for AddCheck</param>
		/// <param name="queryAsync">The query asynchronous. i.e. context.Devices.Take(1).CountAsync()</param>
		/// <param name="degradedThreshold">The degraded threshold. The number of milliseconds that a query takes to mean the connection to the db is degraded</param>
		/// <returns></returns>
		public static IHealthChecksBuilder AddDbHealthCheck<T>(this IHealthChecksBuilder builder, string name, Func<T, Task> queryAsync, int degradedThreshold)
			where T : DbContext
		{
			builder.Services.AddTransient(p => new DbHealthCheck<T>(p, queryAsync ?? throw new ArgumentNullException(nameof(queryAsync)), degradedThreshold));
			return builder.AddCheck<DbHealthCheck<T>>("DbHealthCheck");
		}
	}
}
