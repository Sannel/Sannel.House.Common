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

				if (args.Length < 1)
				{
					log.LogCritical("You must pass in the path to the crt");
					return;
				}

				log.LogInformation("Trying to install {0}", args[0]);

				IServiceProviderExtensions.InstallCertificate(System.Security.Cryptography.X509Certificates.StoreName.AuthRoot,
				System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine, args[0], log);
			}
		}
	}
}
