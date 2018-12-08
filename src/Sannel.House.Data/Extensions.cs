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
