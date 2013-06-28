using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;
using CertificatesWatcher.Installer.Annotations;
using CertficatesWatcher;

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

        public void Install()
        {
            ServiceInstaller.InstallAndStart(ServiceName, ServiceName, typeof(ProjectInstaller).Assembly.Location);
            OnPropertyChanged("IsInstalled");
            OnPropertyChanged("IsNotInstalled");
            OnPropertyChanged("Status");
        }

        public void Uninstall()
        {
            ServiceInstaller.Uninstall(ServiceName);
            OnPropertyChanged("IsInstalled");
            OnPropertyChanged("IsNotInstalled");
            OnPropertyChanged("Status");
        }

        public void Stop()
        {
            CWService.Stop();
            OnPropertyChanged("IsRunning");
            OnPropertyChanged("IsIsInstalledAndRunning");
            OnPropertyChanged("IsInstalledAndNotRunning");
            OnPropertyChanged("Status");
        }

        public void Start()
        {
            CWService.Start();
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
