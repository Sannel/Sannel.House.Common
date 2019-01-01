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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sannel.House.Data
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck" />
	public class DbHealthCheck<T> : IHealthCheck
		where T : DbContext
	{
		private IServiceProvider provider;
		private Func<T, Task> query;

		/// <summary>
		/// Gets or sets the degraded threshold in seconds.
		/// </summary>
		/// <value>
		/// The degraded threshold.
		/// </value>
		public int DegradedThreshold { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DbHealthCheck{T}"/> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="query">The query.</param>
		/// <exception cref="ArgumentNullException">
		/// provider
		/// or
		/// query
		/// </exception>
		public DbHealthCheck(IServiceProvider provider, Func<T, Task> query)
		{
			this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
			this.query = query ?? throw new ArgumentNullException(nameof(query));
			this.DegradedThreshold = 20;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DbHealthCheck{T}"/> class.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="query">The query.</param>
		/// <param name="degradedThreshold">The degradation threshold in seconds.</param>
		/// <exception cref="ArgumentException">degradedThreashold</exception>
		public DbHealthCheck(IServiceProvider provider, Func<T, Task> query, int degradedThreshold)
			: this(provider, query)
		{
			if(degradedThreshold < 0)
			{
				throw new ArgumentException($"{nameof(degradedThreshold)} must be greater then 0", nameof(degradedThreshold));
			}

			this.DegradedThreshold = degradedThreshold;
		}

		/// <summary>Runs the health check, returning the status of the component being checked.</summary>
		/// <param name="context">A context object associated with the current execution.</param>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> that can be used to cancel the health check.</param>
		/// <returns>A <see cref="T:System.Threading.Tasks.Task`1"/> that completes when the health check has finished, yielding the status of the component being checked.</returns>
		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var scope = provider.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetService<T>();
				if(dbContext == null)
				{
					throw new NullReferenceException($"Cannot find a DbContext of type {typeof(T).FullName}");
				}
				try
				{
					var watch = new Stopwatch();
					watch.Start();
					await query(dbContext);
					watch.Stop();

					if(watch.Elapsed < TimeSpan.FromSeconds(DegradedThreshold))
					{
						return HealthCheckResult.Healthy();
					}
					else
					{
						return HealthCheckResult.Degraded($"Query took {watch.ElapsedMilliseconds} milliseconds to execute");
					}
				}
				catch(Exception ex)
				{
					return HealthCheckResult.Unhealthy("Exception during query", ex);
				}
			}
		}
	}
}
