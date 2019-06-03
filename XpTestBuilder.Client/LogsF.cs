using System.Windows.Forms;

namespace XpTestBuilder.Client
{
    public partial class LogsF : Form
    {
        public LogsF()
        {
            InitializeComponent();
        }

        public void SetLogs(string logs)
        {
            txtLogs.Text = logs;
        }
    }
}
