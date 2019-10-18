/* Copyright 2019 Sannel Software, L.L.C.
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at
      http://www.apache.org/licenses/LICENSE-2.0
   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.*/
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Sannel.House.Tests
{
	public abstract class BaseTests : IDisposable
	{
		private ILoggerFactory loggerFactory;
		private SqliteConnection connection;

		/// <summary>
		/// Creates the logger.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public ILogger<T> CreateLogger<T>()
		{
			var l = loggerFactory ?? (loggerFactory = new LoggerFactory());
			return l.CreateLogger<T>();
		}

		/// <summary>
		/// Opens the connection be sure to dispose it.
		/// </summary>
		/// <returns></returns>
		protected SqliteConnection OpenConnection()
		{
			if (connection == null)
			{
				connection = new SqliteConnection("DataSource=:memory:");
				connection.Open();
			}
			return connection;
		}

		public void Dispose() 
			=> connection?.Dispose();
	}

	public abstract class BaseTests<T> : BaseTests
		where T : DbContext
	{
		/// <summary>
		/// Returns a Type from the migration assembly so we can get the assembly name
		/// </summary>
		/// <value>
		/// The type of the migration assembly.
		/// </value>
		public abstract Type MigrationAssemblyType
		{
			get;
		}

		/// <summary>
		/// Creates the database context with the given options.
		/// </summary>
		/// <param name="options">The options.</param>
		/// <returns></returns>
		public abstract T CreateDbContext(DbContextOptions options);

		public T GetTestDB(SqliteConnection connection)
		{
			var d = new DbContextOptionsBuilder();
			d.UseSqlite(connection, i => i.MigrationsAssembly(MigrationAssemblyType.Assembly.GetName().FullName));

			var context = CreateDbContext(d.Options);
			context.Database.Migrate();
			return context;
		}

		/// <summary>
		/// Creates the test database.
		/// </summary>
		/// <returns></returns>
		public T CreateTestDB() 
			=> GetTestDB(OpenConnection());
	}
}
