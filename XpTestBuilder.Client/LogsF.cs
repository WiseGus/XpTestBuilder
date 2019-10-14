using System.Windows.Forms;

namespace XpTestBuilder.Client
{
    public partial class LogsF : Form
    {
        public LogsF()
        {
            InitializeComponent();
        }

        public void SetTitle(string title)
        {
            Text = title;
        }

        public void SetLogs(string logs)
        {
            txtLogs.Text = logs;
        }
    }
}
