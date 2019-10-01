using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sannel.House.Web;
using System;

namespace Sannel.House.Utilities.Certs
{
	class Program
	{
		static void Main(string[] args)
		{
			var service = new ServiceCollection();
			service.AddLogging(i =>
			{
				i.AddConsole();
			});

			using (var provider = service.BuildServiceProvider())
			{

				var log = provider.GetService<ILogger<Program>>();

				if (args.Length < 2)
				{
					log.LogCritical("You must pass in the path to the crt and the store location i.e. (c == current user my, s == local computer my, r == local computer trusted root authority");
					return;
				}

				log.LogInformation("Trying to install {0}", args[0]);

				string password = null;

				if(args.Length >= 2 && !string.IsNullOrEmpty(args[2]))
				{
					password = args[2];
				}

				if(args[1] == "c")
				{
					log.LogInformation("Current User My");
					IServiceProviderExtensions.InstallCertificate(System.Security.Cryptography.X509Certificates.StoreName.My, System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser, args[0], log, password);
				}
				else if(args[1] == "cr")
				{
					log.LogInformation("Current User Auth Root");
					IServiceProviderExtensions.InstallCertificate(System.Security.Cryptography.X509Certificates.StoreName.AuthRoot, System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser, args[0], log, password);
				}
				else if(args[1] == "s")
				{
					log.LogInformation("System My");
					IServiceProviderExtensions.InstallCertificate(System.Security.Cryptography.X509Certificates.StoreName.My, System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine, args[0], log, password);
				}
				else if(args[1] == "r")
				{
					log.LogInformation("System Auth Root");
					IServiceProviderExtensions.InstallCertificate(System.Security.Cryptography.X509Certificates.StoreName.AuthRoot, System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine, args[0], log, password);
				}
				else
				{
					log.LogInformation("Invalid argument passed {0}", args[1]);
				}
			}
		}
	}
}
