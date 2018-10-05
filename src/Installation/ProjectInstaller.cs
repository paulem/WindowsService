using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace WindowsService.Installation
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private readonly ServiceInstaller _serviceInstaller;

        public ProjectInstaller()
        {
            var processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem,
            };

            _serviceInstaller = new ServiceInstaller
            {
                ServiceName = Program.ServiceDefaultName,
                DisplayName = Program.ServiceDefaultDisplayName,
                Description = Program.ServiceDefaultDescription,

                StartType = ServiceStartMode.Automatic,

                // Set service dependencies

                //ServicesDependedOn = new[]
                //{
                //    "Winmgmt"
                //}
            };

            Installers.Add(processInstaller);
            Installers.Add(_serviceInstaller);
        }

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            SetCustomServiceName();
            base.OnBeforeInstall(savedState);
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            SetCustomServiceName();
            base.OnBeforeUninstall(savedState);
        }

        private void SetCustomServiceName()
        {
            if (Context.Parameters.ContainsKey("ServiceName"))
            {
                var serviceName = Context.Parameters["ServiceName"];

                if (!string.IsNullOrEmpty(serviceName))
                    _serviceInstaller.ServiceName = serviceName;
            }

            if (Context.Parameters.ContainsKey("DisplayName"))
            {
                var displayName = Context.Parameters["DisplayName"];

                if (!string.IsNullOrEmpty(displayName))
                    _serviceInstaller.DisplayName = displayName;
            }
        }
    }
}
