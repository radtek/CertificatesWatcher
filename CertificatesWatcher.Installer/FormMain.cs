using System;
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

        private void ShowError()
        {
            MessageBox.Show(this,
                            Resources.The_operation_failed,
                            Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void buttonInstall_Click(object sender, EventArgs e)
        {
            try
            {
                _cwServiceState.Install();
            }
            catch
            {
                ShowError();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                _cwServiceState.Stop();
            }
            catch
            {
                ShowError();
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                _cwServiceState.Start();
            }
            catch
            {
                ShowError();
            }
        }

        private void buttonUninstall_Click(object sender, EventArgs e)
        {
            try
            {
                _cwServiceState.Uninstall();
            }
            catch
            {
                ShowError();
            }
        }
    }
}
