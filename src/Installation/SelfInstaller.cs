using System.Configuration.Install;
using System.Reflection;

namespace WindowsService.Installation
{
    public class SelfInstaller
    {
        private static readonly string ExePath = 
            Assembly.GetExecutingAssembly().Location;

        public static bool Install(string serviceName = null, string displayName = null)
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { $"/ServiceName={serviceName}", $"/DisplayName={displayName}", ExePath });
            }
            catch
            {
                return false;   
            }

            return true;
        }

        public static bool Uninstall(string serviceName = null, string displayName = null)
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { $"/ServiceName={serviceName}", $"/DisplayName={displayName}", "/u", ExePath });
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
