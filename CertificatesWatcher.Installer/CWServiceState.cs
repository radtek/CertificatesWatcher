using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using CertificatesWatcher.Installer.Annotations;

namespace CertificatesWatcher.Installer
{
    public class CWServiceState : INotifyPropertyChanged
    {
        private readonly ProjectInstaller _cwProjectInstaller = new ProjectInstaller();

        public string ServiceName
        {
            get { return _cwProjectInstaller.ServiceInstaller.ServiceName; }
        }

        private ServiceController CWService
        {
            get { return ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == ServiceName); }
        }

        public bool IsNotInstalled
        {
            get { return !IsInstalled; }
        }

        public bool IsInstalled
        {
            get { return CWService != null; }
        }

        public bool IsRunning
        {
            get { return CWService.Status == ServiceControllerStatus.Running; }
        }

        public bool IsIsInstalledAndRunning
        {
            get { return IsInstalled && IsRunning; }
        }

        public bool IsInstalledAndNotRunning
        {
            get { return IsInstalled && !IsRunning; }
        }

        public string Status
        {
            get { return IsInstalled ? CWService.Status.ToString() : "Is not installed"; }
        }

        public async Task Install()
        {
            await Task.Run(() =>
            {
                ServiceInstaller.InstallAndStart(ServiceName, ServiceName, typeof(ProjectInstaller).Assembly.Location);
            }).ConfigureAwait(false);

            OnPropertyChanged("IsInstalled");
            OnPropertyChanged("IsNotInstalled");
            OnPropertyChanged("Status");
        }

        public async Task UninstallAsync()
        {
            await Task.Run(() =>
            {
                ServiceInstaller.Uninstall(ServiceName);
            }).ConfigureAwait(false);

            OnPropertyChanged("IsInstalled");
            OnPropertyChanged("IsNotInstalled");
            OnPropertyChanged("Status");
        }

        public async Task StopAsync()
        {
            await Task.Run(() =>
            {
                CWService.Stop();
                CWService.WaitForStatus(ServiceControllerStatus.Stopped);
            }).ConfigureAwait(false);

            OnPropertyChanged("IsRunning");
            OnPropertyChanged("IsIsInstalledAndRunning");
            OnPropertyChanged("IsInstalledAndNotRunning");
            OnPropertyChanged("Status");
        }

        public async Task StartAsync()
        {
            await Task.Run(() =>
            {
                CWService.Start();
                CWService.WaitForStatus(ServiceControllerStatus.Running);
            }).ConfigureAwait(false);

            
            OnPropertyChanged("IsRunning");
            OnPropertyChanged("IsIsInstalledAndRunning");
            OnPropertyChanged("IsInstalledAndNotRunning");
            OnPropertyChanged("Status");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(/*[CallerMemberName]*/ string propertyName /*= null*/)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
