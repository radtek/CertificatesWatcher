using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using CertificatesWatcher.Installer.Properties;

namespace CertificatesWatcher.Installer
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private readonly CWServiceState _cwServiceState = new CWServiceState();

        private void FormMain_Load(object sender, EventArgs e)
        {
            cWServiceStateBindingSource.DataSource = new[] {_cwServiceState};
        }

        private void ShowError(Exception e)
        {
            MessageBox.Show(this,
                            Resources.The_operation_failed + Environment.NewLine + e.Message,
                            Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void buttonInstall_Click(object sender, EventArgs e)
        {
            await ExecuteTask(() => _cwServiceState.Install());
        }

        private async void buttonUninstall_Click(object sender, EventArgs e)
        {
            await ExecuteTask(() => _cwServiceState.UninstallAsync());
        }

        private async void buttonStop_Click(object sender, EventArgs e)
        {
            await ExecuteTask(() => _cwServiceState.StopAsync());
        }

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            await ExecuteTask(() => _cwServiceState.StartAsync());
        }

        private async Task ExecuteTask(Func<Task> action)
        {
            try
            {
                progressBar.Visible = true;
                await action();
            }
            catch (Exception exception)
            {
                ShowError(exception);
                Validate(); //Проверяем состояние службы после неудачи
            }
            finally
            {
                progressBar.Visible = false;
            }
        }
    }
}
