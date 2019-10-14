using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using XpTestBuilder.Common;

namespace XpTestBuilder.Client
{
    //[CallbackBehavior(IncludeExceptionDetailInFaults = true, UseSynchronizationContext = true, ValidateMustUnderstand = true, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public partial class MainF : Form
    {
        public ICommandService Proxy;
        private LoginF _loginF;
        private BindingSource _jobsBs = new BindingSource();
        private string _clientName;

        public MainF()
        {
            InitializeComponent();
            SetupBuildsGrid();

            btnExpandAll.Click += (sender, e) => treeSolutions.ExpandAll();
            btnCollapseAll.Click += (sender, e) => treeSolutions.CollapseAll();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            _loginF = new LoginF();
            _loginF.SetProxy(Proxy);
            _loginF.SetUsername(Environment.MachineName);

            bool useManualUsername;
            if (bool.TryParse(ConfigurationManager.AppSettings["ManualUsername"], out useManualUsername) && useManualUsername)
            {
                var res = _loginF.ShowDialog();
                if (res != DialogResult.OK)
                {
                    Application.Exit();
                }
            }
            else
            {
                _loginF.RegisterClient();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            try
            {
                Proxy.UnregisterClient();
            }
            catch { }
        }

        private void SetupBuildsGrid()
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.DataSource = _jobsBs;
            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.CellClick += DataGridView1_CellClick;
            dataGridView1.CellPainting += DataGridView1_CellPainting;
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;

            var colStatus = new DataGridViewImageColumn();
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Status";
            colStatus.Name = "colStatus";
            colStatus.ImageLayout = DataGridViewImageCellLayout.Zoom;
            colStatus.Width = 60;
            dataGridView1.Columns.Add(colStatus);

            var colSolutionName = new DataGridViewTextBoxColumn();
            colSolutionName.DataPropertyName = "Solution";
            colSolutionName.Name = "colSolutionName";
            colSolutionName.HeaderText = "Solution name";
            colSolutionName.Width = 350;
            dataGridView1.Columns.Add(colSolutionName);

            var colAddedAt = new DataGridViewTextBoxColumn();
            colAddedAt.DataPropertyName = "AddedAt";
            colAddedAt.Name = "colAddedAt";
            colAddedAt.HeaderText = "Added at";
            colAddedAt.Width = 120;
            dataGridView1.Columns.Add(colAddedAt);
            dataGridView1.Columns["colAddedAt"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";

            var colFinishedAt = new DataGridViewTextBoxColumn();
            colFinishedAt.DataPropertyName = "FinishedAt";
            colFinishedAt.Name = "colFinishedAt";
            colFinishedAt.HeaderText = "Finished at";
            colFinishedAt.Width = 120;
            dataGridView1.Columns.Add(colFinishedAt);
            dataGridView1.Columns["colFinishedAt"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";

            var colAddedFrom = new DataGridViewTextBoxColumn();
            colAddedFrom.DataPropertyName = "AddedFrom";
            colAddedFrom.Name = "colAddedFrom";
            colAddedFrom.HeaderText = "Added from";
            colAddedFrom.Width = 100;
            dataGridView1.Columns.Add(colAddedFrom);

            var colShowLog = new DataGridViewButtonColumn();
            colShowLog.HeaderText = "Build log";
            colShowLog.Text = "Build log";
            colShowLog.Name = "colShowLog";
            colShowLog.UseColumnTextForButtonValue = true;
            colAddedFrom.Width = 100;
            dataGridView1.Columns.Add(colShowLog);

            var colCopyToPatch = new DataGridViewButtonColumn();
            colCopyToPatch.HeaderText = "Copy to patch";
            colCopyToPatch.Text = "Copy to patch";
            colCopyToPatch.Name = "colCopyToPatch";
            colCopyToPatch.UseColumnTextForButtonValue = true;
            colAddedFrom.Width = 100;
            dataGridView1.Columns.Add(colCopyToPatch);
        }

        private void RefreshJobs(List<BuildResult> jobs)
        {
            _jobsBs.DataSource = new JobData(jobs).Data;
        }

        private void DataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == 5)
            {
                var jobDataInfo = dataGridView1.Rows[e.RowIndex].DataBoundItem as JobDataInfo;
                if (jobDataInfo == null || jobDataInfo.FinishedAt == null || jobDataInfo.AddedFrom != _clientName)
                {
                    e.PaintBackground(e.ClipBounds, true);
                    e.Handled = true;
                }
            }
            else if (e.ColumnIndex == 6)
            {
                var jobDataInfo = dataGridView1.Rows[e.RowIndex].DataBoundItem as JobDataInfo;
                if (jobDataInfo == null || jobDataInfo.FinishedAt == null || jobDataInfo.Status != JobDataInfoType.Success || jobDataInfo.AddedFrom != _clientName)
                {
                    e.PaintBackground(e.ClipBounds, true);
                    e.Handled = true;
                }
            }
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.Value != null)
            {
                var enumValue = (JobDataInfoType)Enum.Parse(typeof(JobDataInfoType), e.Value.ToString());
                switch (enumValue)
                {
                    case JobDataInfoType.Error:
                        e.Value = Resources.error;
                        break;
                    case JobDataInfoType.Pending:
                        e.Value = Resources.pending;
                        break;
                    case JobDataInfoType.Success:
                        e.Value = Resources.success;
                        break;
                    case JobDataInfoType.BuildStarted:
                        e.Value = Resources.running;
                        break;
                }
            }
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            if (e.ColumnIndex == 5)
            {
                var jobDataInfo = dataGridView1.Rows[e.RowIndex].DataBoundItem as JobDataInfo;
                if (jobDataInfo.FinishedAt == null || jobDataInfo.AddedFrom != _clientName) return;

                var logsF = new LogsF();
                logsF.SetTitle("Build job logs");
                logsF.SetLogs(string.Join(Environment.NewLine, jobDataInfo.Log));
                logsF.ShowDialog();
            }
            else if (e.ColumnIndex == 6)
            {
                var jobDataInfo = dataGridView1.Rows[e.RowIndex].DataBoundItem as JobDataInfo;
                if (jobDataInfo.FinishedAt == null || jobDataInfo.Status != JobDataInfoType.Success || jobDataInfo.AddedFrom != _clientName) return;

                SendToServerCommand(new CopyToPatchesFolderCommand(jobDataInfo.Solution));
            }
        }

        private void FillTreeSolutions(SolutionInfo solutionInfo)
        {
            treeSolutions.Nodes.Clear();

            var rootNode = new TreeNode();
            rootNode.Text = solutionInfo.Name;
            rootNode.Tag = solutionInfo;
            rootNode.ImageIndex = rootNode.SelectedImageIndex = 0;

            WalkSolutionInfo(solutionInfo.Solutions, rootNode);

            treeSolutions.Nodes.Add(rootNode);
        }

        private void WalkSolutionInfo(List<SolutionInfo> solutions, TreeNode rootNode)
        {
            foreach (var solutionInfo in solutions)
            {
                var parentNode = new TreeNode();
                parentNode.Text = solutionInfo.Name;
                parentNode.Tag = solutionInfo;
                parentNode.ImageIndex = parentNode.SelectedImageIndex = solutionInfo.SolutionType == SolutionType.Folder ? 0 : 1;
                rootNode.Nodes.Add(parentNode);

                foreach (var childSolutionInfo in solutionInfo.Solutions)
                {
                    var childNode = new TreeNode();
                    childNode.Text = childSolutionInfo.Name;
                    childNode.Tag = childSolutionInfo;
                    childNode.ImageIndex = childNode.SelectedImageIndex = childSolutionInfo.SolutionType == SolutionType.Folder ? 0 : 1;
                    parentNode.Nodes.Add(childNode);

                    WalkSolutionInfo(childSolutionInfo.Solutions, childNode);
                }
            }
        }

        private void BtnGetAndBuild_Click(object sender, EventArgs e)
        {
            if (treeSolutions.SelectedNode == null) return;

            var solutionInfo = treeSolutions.SelectedNode.Tag as SolutionInfo;
            SendToServerCommand(new BuildSolutionCommand(solutionInfo.Path));
        }

        private void MenuPing_Click(object sender, EventArgs e)
        {
            SendToServerCommand(new PingCommand());
        }

        private void MenuForceDisconnect_Click(object sender, EventArgs e)
        {
            SendToServerCommand(new ForceDisconnectCommand(_loginF.GetClientName()));
        }

        private void SendToServerCommand(ICommand command)
        {
            try
            {
                Proxy.SendToServerCommand(command.Execute());
            }
            catch (Exception ex)
            {
                ShowProxyError(ex);
            }
        }

        private void ShowProxyError(Exception ex)
        {
            var message = ex.Message;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                message = ex.Message;
            }
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

}
