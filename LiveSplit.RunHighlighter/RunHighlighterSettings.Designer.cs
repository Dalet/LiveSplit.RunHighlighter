namespace LiveSplit.RunHighlighter
{
    partial class RunHighlighterSettings
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.gbStartSplits = new System.Windows.Forms.GroupBox();
            this.tlpStartSplits = new System.Windows.Forms.TableLayoutPanel();
            this.numLeeway = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.numMaxHistoryLength = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.picClockSyncHelp = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBoxTitle = new System.Windows.Forms.TextBox();
            this.txtBoxDescription = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnHLDetailsRestoreDefault = new System.Windows.Forms.Button();
            this.btnVariableHelp = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rbTruncateTimes = new System.Windows.Forms.RadioButton();
            this.rbRoundTimes = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tlpMain.SuspendLayout();
            this.gbStartSplits.SuspendLayout();
            this.tlpStartSplits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLeeway)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxHistoryLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClockSyncHelp)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.AutoSize = true;
            this.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.gbStartSplits, 0, 0);
            this.tlpMain.Controls.Add(this.groupBox1, 0, 1);
            this.tlpMain.Controls.Add(this.groupBox2, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpMain.Location = new System.Drawing.Point(7, 7);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(462, 324);
            this.tlpMain.TabIndex = 0;
            // 
            // gbStartSplits
            // 
            this.gbStartSplits.AutoSize = true;
            this.gbStartSplits.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbStartSplits.Controls.Add(this.tlpStartSplits);
            this.gbStartSplits.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbStartSplits.Location = new System.Drawing.Point(3, 190);
            this.gbStartSplits.Name = "gbStartSplits";
            this.gbStartSplits.Size = new System.Drawing.Size(456, 48);
            this.gbStartSplits.TabIndex = 5;
            this.gbStartSplits.TabStop = false;
            this.gbStartSplits.Text = "Buffer time (seconds)";
            // 
            // tlpStartSplits
            // 
            this.tlpStartSplits.AutoSize = true;
            this.tlpStartSplits.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpStartSplits.BackColor = System.Drawing.Color.Transparent;
            this.tlpStartSplits.ColumnCount = 2;
            this.tlpStartSplits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpStartSplits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpStartSplits.Controls.Add(this.numLeeway, 1, 0);
            this.tlpStartSplits.Controls.Add(this.label1, 0, 0);
            this.tlpStartSplits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpStartSplits.Location = new System.Drawing.Point(3, 16);
            this.tlpStartSplits.Name = "tlpStartSplits";
            this.tlpStartSplits.RowCount = 1;
            this.tlpStartSplits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tlpStartSplits.Size = new System.Drawing.Size(450, 29);
            this.tlpStartSplits.TabIndex = 4;
            // 
            // numLeeway
            // 
            this.numLeeway.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numLeeway.Location = new System.Drawing.Point(253, 4);
            this.numLeeway.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numLeeway.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numLeeway.Name = "numLeeway";
            this.numLeeway.Size = new System.Drawing.Size(50, 20);
            this.numLeeway.TabIndex = 0;
            this.numLeeway.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(247, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "At the start and end of highlights (7 recommended):";
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 244);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 77);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Run History";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.numMaxHistoryLength, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.picClockSyncHelp, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(450, 58);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // numMaxHistoryLength
            // 
            this.numMaxHistoryLength.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numMaxHistoryLength.Location = new System.Drawing.Point(170, 4);
            this.numMaxHistoryLength.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numMaxHistoryLength.Name = "numMaxHistoryLength";
            this.numMaxHistoryLength.Size = new System.Drawing.Size(50, 20);
            this.numMaxHistoryLength.TabIndex = 0;
            this.numMaxHistoryLength.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Maximum number of runs shown:";
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBox1, 2);
            this.checkBox1.Location = new System.Drawing.Point(3, 35);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(245, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Hide runs recorded without internet clock sync";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // picClockSyncHelp
            // 
            this.picClockSyncHelp.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.picClockSyncHelp.Cursor = System.Windows.Forms.Cursors.Help;
            this.picClockSyncHelp.Image = global::LiveSplit.RunHighlighter.Properties.Resources.help;
            this.picClockSyncHelp.Location = new System.Drawing.Point(248, 33);
            this.picClockSyncHelp.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.picClockSyncHelp.Name = "picClockSyncHelp";
            this.picClockSyncHelp.Size = new System.Drawing.Size(20, 20);
            this.picClockSyncHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picClockSyncHelp.TabIndex = 3;
            this.picClockSyncHelp.TabStop = false;
            this.toolTip1.SetToolTip(this.picClockSyncHelp, "To ensure accuracy, timestamps are synchronized with a time server;\r\nif contactin" +
        "g this server failed, the highlight\'s Start and End Times\r\nmight be inaccurate d" +
        "ue to your computer\'s clock drift.");
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(456, 181);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Highlight pre-filled fields";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtBoxTitle, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtBoxDescription, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(450, 162);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Title:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 31);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Description:";
            // 
            // txtBoxTitle
            // 
            this.txtBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxTitle.Location = new System.Drawing.Point(84, 3);
            this.txtBoxTitle.Name = "txtBoxTitle";
            this.txtBoxTitle.Size = new System.Drawing.Size(363, 20);
            this.txtBoxTitle.TabIndex = 2;
            this.txtBoxTitle.Text = "$game $category speedrun in $gametime[RT!=GT] ($realtime RTA)[/RT!=GT]";
            // 
            // txtBoxDescription
            // 
            this.txtBoxDescription.AcceptsReturn = true;
            this.txtBoxDescription.AcceptsTab = true;
            this.txtBoxDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtBoxDescription.Location = new System.Drawing.Point(84, 29);
            this.txtBoxDescription.Multiline = true;
            this.txtBoxDescription.Name = "txtBoxDescription";
            this.txtBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBoxDescription.Size = new System.Drawing.Size(363, 66);
            this.txtBoxDescription.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.btnPreview);
            this.flowLayoutPanel1.Controls.Add(this.btnHLDetailsRestoreDefault);
            this.flowLayoutPanel1.Controls.Add(this.btnVariableHelp);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 130);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(276, 29);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // btnPreview
            // 
            this.btnPreview.AutoSize = true;
            this.btnPreview.Location = new System.Drawing.Point(3, 3);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 6;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnHLDetailsRestoreDefault
            // 
            this.btnHLDetailsRestoreDefault.AutoSize = true;
            this.btnHLDetailsRestoreDefault.Location = new System.Drawing.Point(84, 3);
            this.btnHLDetailsRestoreDefault.Name = "btnHLDetailsRestoreDefault";
            this.btnHLDetailsRestoreDefault.Size = new System.Drawing.Size(94, 23);
            this.btnHLDetailsRestoreDefault.TabIndex = 8;
            this.btnHLDetailsRestoreDefault.Text = "Restore default";
            this.btnHLDetailsRestoreDefault.UseVisualStyleBackColor = true;
            this.btnHLDetailsRestoreDefault.Click += new System.EventHandler(this.btnHLDetailsRestoreDefault_Click);
            // 
            // btnVariableHelp
            // 
            this.btnVariableHelp.AutoSize = true;
            this.btnVariableHelp.Location = new System.Drawing.Point(184, 3);
            this.btnVariableHelp.Name = "btnVariableHelp";
            this.btnVariableHelp.Size = new System.Drawing.Size(89, 23);
            this.btnVariableHelp.TabIndex = 7;
            this.btnVariableHelp.Text = "Formatting help";
            this.btnVariableHelp.UseVisualStyleBackColor = true;
            this.btnVariableHelp.Click += new System.EventHandler(this.btnVariableHelp_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 106);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Time variables:";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.rbTruncateTimes, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.rbRoundTimes, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(84, 101);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(160, 23);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // rbTruncateTimes
            // 
            this.rbTruncateTimes.AutoSize = true;
            this.rbTruncateTimes.Location = new System.Drawing.Point(83, 3);
            this.rbTruncateTimes.Name = "rbTruncateTimes";
            this.rbTruncateTimes.Size = new System.Drawing.Size(74, 17);
            this.rbTruncateTimes.TabIndex = 1;
            this.rbTruncateTimes.Text = "Truncated";
            this.rbTruncateTimes.UseVisualStyleBackColor = true;
            // 
            // rbRoundTimes
            // 
            this.rbRoundTimes.AutoSize = true;
            this.rbRoundTimes.Checked = true;
            this.rbRoundTimes.Location = new System.Drawing.Point(3, 3);
            this.rbRoundTimes.Name = "rbRoundTimes";
            this.rbRoundTimes.Size = new System.Drawing.Size(69, 17);
            this.rbRoundTimes.TabIndex = 0;
            this.rbRoundTimes.TabStop = true;
            this.rbRoundTimes.Text = "Rounded";
            this.rbRoundTimes.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 32767;
            this.toolTip1.InitialDelay = 25;
            this.toolTip1.ReshowDelay = 0;
            this.toolTip1.UseAnimation = false;
            this.toolTip1.UseFading = false;
            // 
            // RunHighlighterSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpMain);
            this.Name = "RunHighlighterSettings";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.Size = new System.Drawing.Size(476, 487);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.gbStartSplits.ResumeLayout(false);
            this.gbStartSplits.PerformLayout();
            this.tlpStartSplits.ResumeLayout(false);
            this.tlpStartSplits.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLeeway)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxHistoryLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClockSyncHelp)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.GroupBox gbStartSplits;
        private System.Windows.Forms.TableLayoutPanel tlpStartSplits;
        private System.Windows.Forms.NumericUpDown numLeeway;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown numMaxHistoryLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBoxTitle;
        private System.Windows.Forms.TextBox txtBoxDescription;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton rbRoundTimes;
        private System.Windows.Forms.RadioButton rbTruncateTimes;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnVariableHelp;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnHLDetailsRestoreDefault;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.PictureBox picClockSyncHelp;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
