using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using DasMulli.Win32.ServiceUtils;
using System.Linq;

namespace Sybon.Common
{
    public static class StartupHandler<TService> where TService : IService
    {
        private const string RunAsServiceFlag = "--run-as-service";
        private const string RegisterServiceFlag = "--register-service";
        private const string UnregisterServiceFlag = "--unregister-service";

        public static void Handle(TService myService, string[] args)
        {
            var serviceName = myService.ServiceName;
            var serviceDisplayName = $"{serviceName} web api";
            var serviceDescription = $"{serviceName} web api";
            if (args.Contains(RegisterServiceFlag))
            {
                new Win32ServiceManager().CreateService(
                    serviceName,
                    serviceDisplayName,
                    serviceDescription,
                    Process.GetCurrentProcess().MainModule.FileName + " --run-as-service",
                    Win32ServiceCredentials.LocalSystem,
                    true,
                    true
                );
                Console.WriteLine(
                    $@"Successfully registered and started service ""{serviceDisplayName}"" (""{
                            serviceDescription
                        }"")");
            }
            else if (args.Contains(UnregisterServiceFlag))
            {
                new Win32ServiceManager().DeleteService(serviceName);
                Console.WriteLine(
                    $@"Successfully unregistered service ""{serviceDisplayName}"" (""{serviceDescription}"")");
            }
            else if (args.Contains(RunAsServiceFlag))
            {
                Console.WriteLine(Directory.GetCurrentDirectory());
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
                var serviceHost = new Win32ServiceHost(myService);
                serviceHost.Run();
            }
            else
            {
                myService.Start(new string[0], () => { });
                Console.WriteLine("Running interactively, press enter to stop.");
                Console.ReadLine();
                myService.Stop();
            }
        }
    }
}