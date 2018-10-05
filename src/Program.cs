using System;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using WindowsService.Installation;

namespace WindowsService
{
    public static class Program
    {
        public const string ServiceDefaultName = "ServiceDefaultName";
        public const string ServiceDefaultDisplayName = "Service default display name";
        public const string ServiceDefaultDescription = "Service default desciption.";

        private static void Main(string[] args)
        {
            if (!Environment.UserInteractive)
                using (var service = new Service())
                    ServiceBase.Run(service);
            else
            {
                Console.CancelKeyPress += (x, y) => { y.Cancel = true; };

                if (args != null && (args.Contains("/i") || args.Contains("/u")))
                {
                    HandleInstall(args);
                    return;
                }

                Start(args);

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);

                Stop();
            }
        }

        private static void Start(string[] args)
        {
            try
            {
                var serviceName = GetServiceName();

                // OnStart code here
            }
            catch (Exception e)
            {
                // Logging here
                throw;
            }
        }

        private static void Stop()
        {
            try
            {
                // OnStop code here
            }
            catch (Exception e)
            {
                // Logging here
            }
        }

        //

        private static void HandleInstall(string[] args)
        {
            var serviceName = args.FirstOrDefault(x => x.IndexOf("/ServiceName", StringComparison.OrdinalIgnoreCase) >= 0)?.Split('=')[1];
            var displayName = args.FirstOrDefault(x => x.IndexOf("/DisplayName", StringComparison.OrdinalIgnoreCase) >= 0)?.Split('=')[1];

            var install = args.Contains("/i", StringComparer.OrdinalIgnoreCase);
            var uninstall = args.Contains("/u", StringComparer.OrdinalIgnoreCase);

            if (install)
                SelfInstaller.Install(serviceName, displayName);
            else if (uninstall)
                SelfInstaller.Uninstall(serviceName, displayName);
        }

        private static string GetServiceName()
        {
            if (Environment.UserInteractive)
                return Program.ServiceDefaultName;

            var processId = System.Diagnostics.Process.GetCurrentProcess().Id;
            var query = "SELECT * FROM Win32_Service where ProcessId = " + processId;
            var searcher = new ManagementObjectSearcher(query);

            foreach (var mo in searcher.Get())
            {
                var queryObj = (ManagementObject)mo;
                return queryObj["Name"].ToString();
            }

            throw new Exception("Unable to get the ServiceName.");
        }

        #region Nested WindowsService

        private class Service : ServiceBase
        {
            protected override void OnStart(string[] args)
            {
                Program.Start(args);
            }

            protected override void OnStop()
            {
                Program.Stop();
            }
        }

        #endregion
    }
}
