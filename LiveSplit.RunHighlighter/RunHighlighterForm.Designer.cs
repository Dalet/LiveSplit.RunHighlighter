namespace LiveSplit.RunHighlighter
{
    partial class RunHighlighterForm
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
            _vidManager?.Dispose();

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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxTwitchUsername = new System.Windows.Forms.TextBox();
            this.gbVideo = new System.Windows.Forms.GroupBox();
            this.tlpVideo = new System.Windows.Forms.TableLayoutPanel();
            this.txtBoxVidUrl = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.picStartTime = new System.Windows.Forms.PictureBox();
            this.txtBoxStartTime = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.picEndTime = new System.Windows.Forms.PictureBox();
            this.txtBoxEndTime = new System.Windows.Forms.TextBox();
            this.btnHighlight = new System.Windows.Forms.Button();
            this.chkAutomateHighlight = new System.Windows.Forms.CheckBox();
            this.gbRunHistory = new System.Windows.Forms.GroupBox();
            this.tlpRunHistory = new System.Windows.Forms.TableLayoutPanel();
            this.lstRunHistory = new System.Windows.Forms.ListBox();
            this.tooltipOutOfVid = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipUnreliableTime = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.gbVideo.SuspendLayout();
            this.tlpVideo.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStartTime)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEndTime)).BeginInit();
            this.gbRunHistory.SuspendLayout();
            this.tlpRunHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.15493F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.84507F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtBoxTwitchUsername, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbVideo, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.gbRunHistory, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(284, 262);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Twitch Username:";
            // 
            // txtBoxTwitchUsername
            // 
            this.txtBoxTwitchUsername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxTwitchUsername.Location = new System.Drawing.Point(100, 4);
            this.txtBoxTwitchUsername.Name = "txtBoxTwitchUsername";
            this.txtBoxTwitchUsername.Size = new System.Drawing.Size(181, 20);
            this.txtBoxTwitchUsername.TabIndex = 0;
            this.txtBoxTwitchUsername.TextChanged += new System.EventHandler(this.txtBoxTwitchUsername_TextChanged);
            this.txtBoxTwitchUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxTwitchUsername_KeyPress);
            // 
            // gbVideo
            // 
            this.gbVideo.AutoSize = true;
            this.gbVideo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.gbVideo, 2);
            this.gbVideo.Controls.Add(this.tlpVideo);
            this.gbVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbVideo.Location = new System.Drawing.Point(3, 130);
            this.gbVideo.Name = "gbVideo";
            this.gbVideo.Size = new System.Drawing.Size(278, 129);
            this.gbVideo.TabIndex = 2;
            this.gbVideo.TabStop = false;
            this.gbVideo.Text = "Video";
            // 
            // tlpVideo
            // 
            this.tlpVideo.AutoSize = true;
            this.tlpVideo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpVideo.ColumnCount = 2;
            this.tlpVideo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpVideo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpVideo.Controls.Add(this.txtBoxVidUrl, 0, 0);
            this.tlpVideo.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tlpVideo.Controls.Add(this.btnHighlight, 0, 2);
            this.tlpVideo.Controls.Add(this.chkAutomateHighlight, 1, 2);
            this.tlpVideo.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpVideo.Enabled = false;
            this.tlpVideo.Location = new System.Drawing.Point(3, 16);
            this.tlpVideo.Name = "tlpVideo";
            this.tlpVideo.RowCount = 3;
            this.tlpVideo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tlpVideo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpVideo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tlpVideo.Size = new System.Drawing.Size(272, 110);
            this.tlpVideo.TabIndex = 0;
            // 
            // txtBoxVidUrl
            // 
            this.txtBoxVidUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpVideo.SetColumnSpan(this.txtBoxVidUrl, 2);
            this.txtBoxVidUrl.Location = new System.Drawing.Point(3, 4);
            this.txtBoxVidUrl.Name = "txtBoxVidUrl";
            this.txtBoxVidUrl.ReadOnly = true;
            this.txtBoxVidUrl.Size = new System.Drawing.Size(266, 20);
            this.txtBoxVidUrl.TabIndex = 0;
            this.txtBoxVidUrl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBoxVidUrl.Click += new System.EventHandler(this.txtBox_SelectAll);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tlpVideo.SetColumnSpan(this.tableLayoutPanel3, 2);
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox3, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 32);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(266, 46);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.picStartTime);
            this.groupBox2.Controls.Add(this.txtBoxStartTime);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(127, 40);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Start Time";
            // 
            // picStartTime
            // 
            this.picStartTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.picStartTime.Location = new System.Drawing.Point(6, 18);
            this.picStartTime.Margin = new System.Windows.Forms.Padding(0);
            this.picStartTime.Name = "picStartTime";
            this.picStartTime.Size = new System.Drawing.Size(16, 16);
            this.picStartTime.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picStartTime.TabIndex = 1;
            this.picStartTime.TabStop = false;
            // 
            // txtBoxStartTime
            // 
            this.txtBoxStartTime.BackColor = System.Drawing.SystemColors.Control;
            this.txtBoxStartTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtBoxStartTime.Location = new System.Drawing.Point(3, 16);
            this.txtBoxStartTime.Name = "txtBoxStartTime";
            this.txtBoxStartTime.Size = new System.Drawing.Size(121, 20);
            this.txtBoxStartTime.TabIndex = 0;
            this.txtBoxStartTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBoxStartTime.Click += new System.EventHandler(this.txtBox_SelectAll);
            this.txtBoxStartTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxEndTime_KeyPress);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.picEndTime);
            this.groupBox3.Controls.Add(this.txtBoxEndTime);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(136, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(127, 40);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "End Time";
            // 
            // picEndTime
            // 
            this.picEndTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.picEndTime.Location = new System.Drawing.Point(6, 18);
            this.picEndTime.Margin = new System.Windows.Forms.Padding(0);
            this.picEndTime.Name = "picEndTime";
            this.picEndTime.Size = new System.Drawing.Size(16, 16);
            this.picEndTime.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picEndTime.TabIndex = 1;
            this.picEndTime.TabStop = false;
            // 
            // txtBoxEndTime
            // 
            this.txtBoxEndTime.BackColor = System.Drawing.SystemColors.Control;
            this.txtBoxEndTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtBoxEndTime.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtBoxEndTime.Location = new System.Drawing.Point(3, 16);
            this.txtBoxEndTime.Name = "txtBoxEndTime";
            this.txtBoxEndTime.Size = new System.Drawing.Size(121, 20);
            this.txtBoxEndTime.TabIndex = 0;
            this.txtBoxEndTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBoxEndTime.Click += new System.EventHandler(this.txtBox_SelectAll);
            this.txtBoxEndTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxEndTime_KeyPress);
            // 
            // btnHighlight
            // 
            this.btnHighlight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHighlight.Location = new System.Drawing.Point(3, 84);
            this.btnHighlight.Name = "btnHighlight";
            this.btnHighlight.Size = new System.Drawing.Size(182, 23);
            this.btnHighlight.TabIndex = 3;
            this.btnHighlight.Text = "Open Highlighter...";
            this.btnHighlight.UseVisualStyleBackColor = true;
            this.btnHighlight.Click += new System.EventHandler(this.btnHighlight_Click);
            // 
            // chkAutomateHighlight
            // 
            this.chkAutomateHighlight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAutomateHighlight.AutoSize = true;
            this.chkAutomateHighlight.Location = new System.Drawing.Point(191, 87);
            this.chkAutomateHighlight.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.chkAutomateHighlight.Name = "chkAutomateHighlight";
            this.chkAutomateHighlight.Size = new System.Drawing.Size(81, 17);
            this.chkAutomateHighlight.TabIndex = 4;
            this.chkAutomateHighlight.Text = "Auto-create";
            this.chkAutomateHighlight.UseVisualStyleBackColor = true;
            // 
            // gbRunHistory
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.gbRunHistory, 2);
            this.gbRunHistory.Controls.Add(this.tlpRunHistory);
            this.gbRunHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRunHistory.Location = new System.Drawing.Point(3, 32);
            this.gbRunHistory.Name = "gbRunHistory";
            this.gbRunHistory.Size = new System.Drawing.Size(278, 92);
            this.gbRunHistory.TabIndex = 2;
            this.gbRunHistory.TabStop = false;
            this.gbRunHistory.Text = "Run History for Current Splits";
            // 
            // tlpRunHistory
            // 
            this.tlpRunHistory.ColumnCount = 1;
            this.tlpRunHistory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRunHistory.Controls.Add(this.lstRunHistory, 0, 0);
            this.tlpRunHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRunHistory.Location = new System.Drawing.Point(3, 16);
            this.tlpRunHistory.Name = "tlpRunHistory";
            this.tlpRunHistory.RowCount = 1;
            this.tlpRunHistory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRunHistory.Size = new System.Drawing.Size(272, 73);
            this.tlpRunHistory.TabIndex = 0;
            // 
            // lstRunHistory
            // 
            this.lstRunHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstRunHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstRunHistory.FormattingEnabled = true;
            this.lstRunHistory.Items.AddRange(new object[] {
            "10:50/0:5074 minutes ago",
            "0:50/0:50 74 minutes ago",
            "0:50/0:50 74 minutes ago",
            "0:50/0:50 74 minutes ago",
            "0:50/0:50 74 minutes ago",
            "0:50/0:50 74 minutes ago",
            "0:50/0:50 74 minutes ago",
            "0:50/0:50 74 minutes ago"});
            this.lstRunHistory.Location = new System.Drawing.Point(3, 3);
            this.lstRunHistory.Name = "lstRunHistory";
            this.lstRunHistory.Size = new System.Drawing.Size(266, 67);
            this.lstRunHistory.TabIndex = 2;
            this.lstRunHistory.SelectedIndexChanged += new System.EventHandler(this.lstRunHistory_SelectedIndexChanged);
            // 
            // tooltipOutOfVid
            // 
            this.tooltipOutOfVid.AutoPopDelay = 32767;
            this.tooltipOutOfVid.InitialDelay = 25;
            this.tooltipOutOfVid.ReshowDelay = 0;
            this.tooltipOutOfVid.ShowAlways = true;
            this.tooltipOutOfVid.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.tooltipOutOfVid.ToolTipTitle = "Time possibly out of the video\'s range at the moment";
            // 
            // toolTipUnreliableTime
            // 
            this.toolTipUnreliableTime.AutoPopDelay = 32767;
            this.toolTipUnreliableTime.InitialDelay = 25;
            this.toolTipUnreliableTime.ReshowDelay = 0;
            this.toolTipUnreliableTime.ShowAlways = true;
            this.toolTipUnreliableTime.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.toolTipUnreliableTime.ToolTipTitle = "Potentially imprecise time";
            // 
            // RunHighlighterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 270);
            this.Name = "RunHighlighterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Run Highlighter";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.gbVideo.ResumeLayout(false);
            this.gbVideo.PerformLayout();
            this.tlpVideo.ResumeLayout(false);
            this.tlpVideo.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStartTime)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEndTime)).EndInit();
            this.gbRunHistory.ResumeLayout(false);
            this.tlpRunHistory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxTwitchUsername;
        private System.Windows.Forms.GroupBox gbVideo;
        private System.Windows.Forms.TableLayoutPanel tlpVideo;
        private System.Windows.Forms.TextBox txtBoxVidUrl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtBoxStartTime;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtBoxEndTime;
        private System.Windows.Forms.GroupBox gbRunHistory;
        private System.Windows.Forms.TableLayoutPanel tlpRunHistory;
        private System.Windows.Forms.ListBox lstRunHistory;
        private System.Windows.Forms.ToolTip tooltipOutOfVid;
        private System.Windows.Forms.Button btnHighlight;
        private System.Windows.Forms.CheckBox chkAutomateHighlight;
        private System.Windows.Forms.ToolTip toolTipUnreliableTime;
        private System.Windows.Forms.PictureBox picStartTime;
        private System.Windows.Forms.PictureBox picEndTime;
    }
}