using System;
using System.Windows.Forms;
using XpTestBuilder.Common;

namespace XpTestBuilder.Client
{
    public partial class LoginF : Form
    {
        private ICommandService _proxy;

        public LoginF()
        {
            InitializeComponent();

        }

        private void BtnRegisterClient_Click(object sender, System.EventArgs e)
        {
            RegisterClient();
        }

        public void EnableControls()
        {
            txtUsername.Enabled = true;
            btnRegisterClient.Enabled = true;
        }

        public void DisableControls()
        {
            txtUsername.Enabled = false;
            btnRegisterClient.Enabled = false;
        }

        public void RegisterClient()
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Invalid client name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _proxy.RegisterClient(txtUsername.Text);
                DisableControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetProxy(ICommandService proxy)
        {
            _proxy = proxy;
        }

        public void SetUsername(string username)
        {
            txtUsername.Text = username;
        }
    }
}
