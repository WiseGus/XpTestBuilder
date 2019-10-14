using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using XpTestBuilder.Common;

namespace XpTestBuilder.Client
{
    partial class MainF : ICommandCallback
    {
        public void SendToClientCommand(CommandData data)
        {
            switch (data.Command)
            {
                case CommandsIndex.PONG:
                    MessageBox.Show("Pong");
                    break;
                case CommandsIndex.CLIENT_REGISTER_OK:
                    _loginF.DialogResult = DialogResult.OK;
                    var clientRegistration = new JavaScriptSerializer().Deserialize<ClientRegistration>(data.Payload);
                    _clientName = clientRegistration.ClientName;
                    menuConnectionStatus.Text += string.Format(" - {0}", _clientName);
                    Text += string.Format(" - {0}", clientRegistration.ServerName);
                    break;
                case CommandsIndex.CLIENT_NAME_EXISTS:
                    _loginF.EnableControls();
                    MessageBox.Show("Client name already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case CommandsIndex.GET_SOLUTIONS:
                    var solutionInfo = new JavaScriptSerializer { MaxJsonLength = int.MaxValue }.Deserialize<SolutionInfo>(data.Payload);
                    FillTreeSolutions(solutionInfo);
                    break;
                case CommandsIndex.GET_JOBS:
                    {
                        var jobs = new JavaScriptSerializer { MaxJsonLength = int.MaxValue }.Deserialize<List<BuildResult>>(data.Payload);
                        RefreshJobs(jobs);
                    }
                    break;
                case CommandsIndex.JOB_STATUS:
                    {
                        var jobStatus = new JavaScriptSerializer().Deserialize<JobStatus>(data.Payload);
                        var job = (_jobsBs.DataSource as List<JobDataInfo>).Find(p => p.JobID == jobStatus.JobID.ToString());
                        if (job != null)
                        {
                            job.Status = JobDataInfoType.BuildStarted;
                            dataGridView1.Refresh();
                        }
                    }
                    break;
            }
        }
    }
}
