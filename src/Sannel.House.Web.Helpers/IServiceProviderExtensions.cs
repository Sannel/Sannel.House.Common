using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sannel.House.Web
{
	public static class IServiceProviderExtensions
	{
		/// <summary>Installs the certificate.</summary>
		/// <param name="name">The name.</param>
		/// <param name="location">The location.</param>
		/// <param name="fullPath">The full path.</param>
		/// <param name="log">The log.</param>
		private static void installCertificate(StoreName name, StoreLocation location, string fullPath, ILogger log)
		{
			using (var cert = new X509Certificate2(fullPath))
			{
				using (var store = new X509Store(StoreName.AuthRoot, StoreLocation.LocalMachine))
				{
					try
					{
						store.Open(OpenFlags.ReadWrite);
						if (!store.Certificates.Contains(cert))
						{
							log.LogInformation($"Trying to install cert {cert.SubjectName}");
							store.Add(cert);
						}
						else
						{
							log.LogInformation("Cert is already installed");
						}
						store.Close();
					}
					catch (Exception ex)
					{
						log.LogError(ex, "Exception installing Cert");
					}
				}
			}
		}

		/// <summary>Checks if the cert is installed and if not attempts to install it.</summary>
		/// <param name="provider">The provider.</param>
		public static void CheckAndInstallTrustedCertificate(this IServiceProvider provider)
		{
			var config = provider.GetService<IConfiguration>();
			var log = provider.GetService<ILogger<IServiceProvider>>();
			var shouldInstall = config.GetValue<bool?>("Cert:Install");

			if (shouldInstall == true)
			{
				var fullPath = Path.GetFullPath(config["Cert:Crt"]);
				if(!File.Exists(fullPath))
				{
					log.LogError("File not found {0}", fullPath);
					return;
				}

				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					using (var identity = WindowsIdentity.GetCurrent())
					{
						var principle = new WindowsPrincipal(identity);
						if(principle.IsInRole(WindowsBuiltInRole.Administrator))
						{
							installCertificate(StoreName.AuthRoot, StoreLocation.LocalMachine, fullPath, log);
						}
						else
						{
							installCertificate(StoreName.CertificateAuthority, StoreLocation.CurrentUser, fullPath, log);
						}
					}
				}
				else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					installCertificate(StoreName.CertificateAuthority, StoreLocation.CurrentUser, fullPath, log);
				}
				else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				{
					if (Directory.Exists("/etc/ssl/certs"))
					{
						log.LogInformation("/etc/ssl/certs exists check if the cert is installed");
						var fileName = Path.GetFileNameWithoutExtension(fullPath);
						var sslPath = Path.Join("/etc/ssl/certs", $"{fileName}.pem");
						if(!File.Exists(sslPath))
						{
							log.LogInformation($"Installing cert {fullPath}");
							try
							{
								File.Copy(fullPath, sslPath);
								log.LogInformation("Finished installing cert");
							}
							catch(IOException ioe)
							{
								log.LogError(ioe, "Error installing cert");
							}

						}
						else
						{
							log.LogInformation($"Cert is already installed {fileName}");
						}
					}
				}
			}
		}
	}
}
