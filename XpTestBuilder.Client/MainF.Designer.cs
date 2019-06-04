namespace XpTestBuilder.Client
{
    partial class MainF
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainF));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tab = new System.Windows.Forms.TabControl();
            this.tabBuilds = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabSolutions = new System.Windows.Forms.TabPage();
            this.btnGetAndBuild = new System.Windows.Forms.Button();
            this.treeSolutions = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.menuPing = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuConnectionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuForceDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.tab.SuspendLayout();
            this.tabBuilds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabSolutions.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "icons8-opened-folder-48.png");
            this.imageList1.Images.SetKeyName(1, "icons8-visual-studio-48.png");
            // 
            // tab
            // 
            this.tab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tab.Controls.Add(this.tabBuilds);
            this.tab.Controls.Add(this.tabSolutions);
            this.tab.Location = new System.Drawing.Point(1, 2);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(970, 545);
            this.tab.TabIndex = 5;
            // 
            // tabBuilds
            // 
            this.tabBuilds.Controls.Add(this.dataGridView1);
            this.tabBuilds.Location = new System.Drawing.Point(4, 22);
            this.tabBuilds.Name = "tabBuilds";
            this.tabBuilds.Padding = new System.Windows.Forms.Padding(3);
            this.tabBuilds.Size = new System.Drawing.Size(962, 519);
            this.tabBuilds.TabIndex = 1;
            this.tabBuilds.Text = "Builds";
            this.tabBuilds.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(956, 513);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabSolutions
            // 
            this.tabSolutions.Controls.Add(this.btnGetAndBuild);
            this.tabSolutions.Controls.Add(this.treeSolutions);
            this.tabSolutions.Location = new System.Drawing.Point(4, 22);
            this.tabSolutions.Name = "tabSolutions";
            this.tabSolutions.Padding = new System.Windows.Forms.Padding(3);
            this.tabSolutions.Size = new System.Drawing.Size(962, 519);
            this.tabSolutions.TabIndex = 0;
            this.tabSolutions.Text = "Solutions";
            this.tabSolutions.UseVisualStyleBackColor = true;
            // 
            // btnGetAndBuild
            // 
            this.btnGetAndBuild.Location = new System.Drawing.Point(7, 4);
            this.btnGetAndBuild.Name = "btnGetAndBuild";
            this.btnGetAndBuild.Size = new System.Drawing.Size(109, 26);
            this.btnGetAndBuild.TabIndex = 4;
            this.btnGetAndBuild.Text = "Get latest and build";
            this.btnGetAndBuild.UseVisualStyleBackColor = true;
            this.btnGetAndBuild.Click += new System.EventHandler(this.BtnGetAndBuild_Click);
            // 
            // treeSolutions
            // 
            this.treeSolutions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeSolutions.ImageIndex = 0;
            this.treeSolutions.ImageList = this.imageList1;
            this.treeSolutions.Location = new System.Drawing.Point(3, 32);
            this.treeSolutions.Name = "treeSolutions";
            this.treeSolutions.SelectedImageIndex = 0;
            this.treeSolutions.Size = new System.Drawing.Size(972, 484);
            this.treeSolutions.TabIndex = 3;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1,
            this.toolStripStatusLabel1,
            this.menuConnectionStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 546);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(969, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuForceDisconnect,
            this.menuPing});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(58, 20);
            this.toolStripSplitButton1.Text = "Debug";
            // 
            // menuPing
            // 
            this.menuPing.Name = "menuPing";
            this.menuPing.Size = new System.Drawing.Size(180, 22);
            this.menuPing.Text = "Ping!";
            this.menuPing.Click += new System.EventHandler(this.MenuPing_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(800, 17);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // menuConnectionStatus
            // 
            this.menuConnectionStatus.ForeColor = System.Drawing.Color.Green;
            this.menuConnectionStatus.Name = "menuConnectionStatus";
            this.menuConnectionStatus.Size = new System.Drawing.Size(65, 17);
            this.menuConnectionStatus.Text = "Connected";
            // 
            // menuForceDisconnect
            // 
            this.menuForceDisconnect.Name = "menuForceDisconnect";
            this.menuForceDisconnect.Size = new System.Drawing.Size(180, 22);
            this.menuForceDisconnect.Text = "Force disconnect!";
            this.menuForceDisconnect.Click += new System.EventHandler(this.MenuForceDisconnect_Click);
            // 
            // MainF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 568);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tab);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainF";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XpTest builder v0.1";
            this.tab.ResumeLayout(false);
            this.tabBuilds.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabSolutions.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage tabSolutions;
        private System.Windows.Forms.TreeView treeSolutions;
        private System.Windows.Forms.TabPage tabBuilds;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnGetAndBuild;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem menuPing;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel menuConnectionStatus;
        private System.Windows.Forms.ToolStripMenuItem menuForceDisconnect;
    }
}