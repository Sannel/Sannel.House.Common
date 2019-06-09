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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sannel.House.Client
{
	public abstract class ClientBase
	{
		protected readonly IHttpClientFactory factory=null;
		protected readonly ILogger logger;
		protected readonly HttpClient client = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="DevicesClient" /> class.
		/// </summary>
		/// <param name="factory">The HttpClientFactory.</param>
		/// <param name="logger">The logger.</param>
		/// <exception cref="ArgumentNullException">
		/// factory
		/// or
		/// logger
		/// </exception>
		protected ClientBase(IHttpClientFactory factory, ILogger logger)
		{
			this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ClientBase"/> class.
		/// </summary>
		/// <param name="client">The client.</param>
		/// <param name="logger">The logger.</param>
		/// <exception cref="ArgumentNullException">
		/// client
		/// or
		/// logger
		/// </exception>
		protected ClientBase(HttpClient client, ILogger logger)
		{
			this.client = client ?? throw new ArgumentNullException(nameof(client));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <summary>
		/// Gets or sets the bearer authentication token.
		/// </summary>
		/// <value>
		/// The authentication token.
		/// </value>
		public virtual string AuthToken
		{
			get;
			set;
		}

		/// <summary>
		/// Deserializes if supported code asynchronous.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		protected virtual async Task<T> DeserializeIfSupportedCodeAsync<T>(HttpResponseMessage message)
			where T : IResults, new()
		{
			switch (message.StatusCode)
			{
				case System.Net.HttpStatusCode.OK:
				case System.Net.HttpStatusCode.NotFound:
				case System.Net.HttpStatusCode.BadRequest:
					var data = await message.Content.ReadAsStringAsync();
					var obj = await Task.Run(() => JsonConvert.DeserializeObject<T>(data));
					obj.Success = message.StatusCode == System.Net.HttpStatusCode.OK;
					return obj;

				default:
					var err = new T
					{
						Success = false,
						Status = (int)message.StatusCode,
					};
					if (message.Content != null && message.Content.Headers.ContentLength > 0)
					{
						err.Title = await message.Content.ReadAsStringAsync();
					}
					return err;
			};
		}

		/// <summary>
		/// Adds the authorization header.
		/// </summary>
		/// <param name="message">The message.</param>
		protected virtual void AddAuthorizationHeader(HttpRequestMessage message)
			=> message.Headers.Authorization
				= new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AuthToken);


		/// <summary>
		/// Gets the HttpClient from the factory.
		/// </summary>
		/// <returns></returns>
		protected abstract HttpClient GetClient();

		/// <summary>
		/// Does a get call to the provided url
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns></returns>
		protected virtual async Task<T> GetAsync<T>(string url)
			where T : IResults, new()
		{
			var client = GetClient();
			try
			{
				using (var message = new HttpRequestMessage(HttpMethod.Get, url))
				{
					AddAuthorizationHeader(message);
					if (logger.IsEnabled(LogLevel.Debug))
					{
						logger.LogDebug("RequestUri: {0}", message.RequestUri);
						logger.LogDebug("AuthHeader: {0}", message.Headers.Authorization);
					}
					var response = await client.SendAsync(message);
					return await DeserializeIfSupportedCodeAsync<T>(response);
				}
			}
			catch (Exception ex) when (ex is HttpRequestException || ex is JsonException)
			{
				return new T()
				{
					Status = 444,
					Title = "Exception",
					Success = false,
					Exception = ex
				};
			}
		}

		/// <summary>
		/// Posts the object asynchronous.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="url">The URL.</param>
		/// <param name="obj">The object.</param>
		/// <returns></returns>
		protected virtual async Task<T> PostAsync<T>(string url, object obj)
			where T : IResults, new()
		{
			var client = GetClient();
			try
			{
				using (var message = new HttpRequestMessage(HttpMethod.Post, url))
				{
					AddAuthorizationHeader(message);
					if (logger.IsEnabled(LogLevel.Debug))
					{
						logger.LogDebug("RequestUri: {0}", message.RequestUri);
						logger.LogDebug("AuthHeader: {0}", message.Headers.Authorization);
					}
					message.Content = new StringContent(
							await Task.Run(() => JsonConvert.SerializeObject(obj)),
							System.Text.Encoding.UTF8,
							"application/json");
					var response = await client.SendAsync(message);
					return await DeserializeIfSupportedCodeAsync<T>(response);
				}
			}
			catch (Exception ex) when (ex is HttpRequestException || ex is JsonException)
			{
				return new T()
				{
					Status = 444,
					Title = "Exception",
					Success = false,
					Exception = ex
				};
			}
		}

		/// <summary>
		/// Puts the object asynchronous.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="url">The URL.</param>
		/// <param name="obj">The object.</param>
		/// <returns></returns>
		protected virtual async Task<T> PutAsync<T>(string url, object obj)
			where T : IResults, new()
		{
			var client = GetClient();
			try
			{
				using (var message = new HttpRequestMessage(HttpMethod.Put, url))
				{
					AddAuthorizationHeader(message);
					if (logger.IsEnabled(LogLevel.Debug))
					{
						logger.LogDebug("RequestUri: {0}", message.RequestUri);
						logger.LogDebug("AuthHeader: {0}", message.Headers.Authorization);
					}
					message.Content = new StringContent(
							await Task.Run(() => JsonConvert.SerializeObject(obj)),
							System.Text.Encoding.UTF8,
							"application/json");
					var response = await client.SendAsync(message);
					return await DeserializeIfSupportedCodeAsync<T>(response);
				}
			}
			catch (Exception ex) when (ex is HttpRequestException || ex is JsonException)
			{
				return new T()
				{
					Status = 444,
					Title = "Exception",
					Success = false,
					Exception = ex
				};
			}
		}

		/// <summary>
		/// Deletes the  asynchronous.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="url">The URL.</param>
		/// <returns></returns>
		protected virtual async Task<T> DeleteAsync<T>(string url)
			where T : IResults, new()
		{
			var client = GetClient();
			try
			{
				using (var message = new HttpRequestMessage(HttpMethod.Delete, url))
				{
					AddAuthorizationHeader(message);
					if (logger.IsEnabled(LogLevel.Debug))
					{
						logger.LogDebug("RequestUri: {0}", message.RequestUri);
						logger.LogDebug("AuthHeader: {0}", message.Headers.Authorization);
					}
					var response = await client.SendAsync(message);
					return await DeserializeIfSupportedCodeAsync<T>(response);
				}
			}
			catch (Exception ex) when (ex is HttpRequestException || ex is JsonException)
			{
				return new T()
				{
					Status = 444,
					Title = "Exception",
					Success = false,
					Exception = ex
				};
			}
		}
	}
}
