using System;
using System.Windows.Forms;
using XpTestBuilder.Common;

namespace XpTestBuilder.Client
{
    public partial class LoginF : Form
    {
        public ICommandService Proxy;

        public LoginF()
        {
            InitializeComponent();
            txtUsername.Text = Environment.MachineName;
        }

        private void BtnRegisterClient_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Invalid client name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Proxy.RegisterClient(txtUsername.Text);
                DisableControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
    }
}
