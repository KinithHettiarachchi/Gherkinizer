﻿namespace MindMapToGherkin
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            txtGherkin = new RichTextBox();
            label1 = new Label();
            txtLevel = new TextBox();
            txtParent = new TextBox();
            label2 = new Label();
            label3 = new Label();
            drpDomain = new ComboBox();
            txtTSTID = new TextBox();
            label4 = new Label();
            txtSequence = new TextBox();
            label5 = new Label();
            txtRequirements = new TextBox();
            label6 = new Label();
            BtnHelp = new Button();
            txtFeatureTitle = new TextBox();
            label7 = new Label();
            lblTestCasePrefix = new Label();
            chkClearOnDrop = new CheckBox();
            btnCopy = new Button();
            btnLocate = new Button();
            btnWriteToFeatureFile = new Button();
            SuspendLayout();
            // 
            // txtGherkin
            // 
            txtGherkin.BackColor = Color.FromArgb(20, 20, 20);
            txtGherkin.BorderStyle = BorderStyle.FixedSingle;
            txtGherkin.Cursor = Cursors.IBeam;
            txtGherkin.Font = new Font("Cascadia Code", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtGherkin.ForeColor = Color.White;
            txtGherkin.Location = new Point(12, 180);
            txtGherkin.Name = "txtGherkin";
            txtGherkin.ReadOnly = true;
            txtGherkin.Size = new Size(1662, 739);
            txtGherkin.TabIndex = 1;
            txtGherkin.TabStop = false;
            txtGherkin.Text = "";
            txtGherkin.WordWrap = false;
            txtGherkin.TextChanged += txtGherkin_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F);
            label1.ForeColor = Color.White;
            label1.Location = new Point(735, 130);
            label1.Name = "label1";
            label1.Size = new Size(133, 25);
            label1.TabIndex = 2;
            label1.Text = "Scenario Level";
            // 
            // txtLevel
            // 
            txtLevel.BackColor = Color.FromArgb(20, 20, 20);
            txtLevel.BorderStyle = BorderStyle.FixedSingle;
            txtLevel.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtLevel.ForeColor = Color.Lime;
            txtLevel.Location = new Point(883, 122);
            txtLevel.Name = "txtLevel";
            txtLevel.Size = new Size(52, 33);
            txtLevel.TabIndex = 7;
            txtLevel.Text = "1";
            txtLevel.TextAlign = HorizontalAlignment.Right;
            txtLevel.TextChanged += txtLevel_TextChanged;
            txtLevel.KeyPress += txtLevel_KeyPress;
            // 
            // txtParent
            // 
            txtParent.BackColor = Color.FromArgb(20, 20, 20);
            txtParent.BorderStyle = BorderStyle.FixedSingle;
            txtParent.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtParent.ForeColor = Color.Lime;
            txtParent.Location = new Point(242, 128);
            txtParent.Name = "txtParent";
            txtParent.Size = new Size(164, 33);
            txtParent.TabIndex = 5;
            txtParent.Text = "Functional Tests";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F);
            label2.ForeColor = Color.White;
            label2.Location = new Point(12, 136);
            label2.Name = "label2";
            label2.Size = new Size(184, 25);
            label2.TabIndex = 4;
            label2.Text = "Parent Node of Tests";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F);
            label3.ForeColor = Color.White;
            label3.Location = new Point(12, 77);
            label3.Name = "label3";
            label3.Size = new Size(78, 25);
            label3.TabIndex = 8;
            label3.Text = "Domain";
            // 
            // drpDomain
            // 
            drpDomain.BackColor = Color.FromArgb(20, 20, 20);
            drpDomain.Cursor = Cursors.Hand;
            drpDomain.DropDownStyle = ComboBoxStyle.DropDownList;
            drpDomain.FlatStyle = FlatStyle.Flat;
            drpDomain.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            drpDomain.ForeColor = Color.Lime;
            drpDomain.FormattingEnabled = true;
            drpDomain.Items.AddRange(new object[] { "EX", "LG", "OR", "ST", "XL", "IN", "WB" });
            drpDomain.Location = new Point(242, 69);
            drpDomain.Name = "drpDomain";
            drpDomain.Size = new Size(127, 33);
            drpDomain.TabIndex = 3;
            // 
            // txtTSTID
            // 
            txtTSTID.BackColor = Color.FromArgb(20, 20, 20);
            txtTSTID.BorderStyle = BorderStyle.FixedSingle;
            txtTSTID.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtTSTID.ForeColor = Color.Lime;
            txtTSTID.Location = new Point(297, 17);
            txtTSTID.Name = "txtTSTID";
            txtTSTID.Size = new Size(72, 33);
            txtTSTID.TabIndex = 1;
            txtTSTID.TextChanged += txtTSTID_TextChanged;
            txtTSTID.KeyPress += TxtTSTID_KeyPress;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 14.25F);
            label4.ForeColor = Color.White;
            label4.Location = new Point(12, 21);
            label4.Name = "label4";
            label4.Size = new Size(216, 25);
            label4.TabIndex = 10;
            label4.Text = "Testable Functionality ID";
            // 
            // txtSequence
            // 
            txtSequence.BackColor = Color.FromArgb(20, 20, 20);
            txtSequence.BorderStyle = BorderStyle.FixedSingle;
            txtSequence.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtSequence.ForeColor = Color.Lime;
            txtSequence.Location = new Point(612, 122);
            txtSequence.Name = "txtSequence";
            txtSequence.Size = new Size(56, 33);
            txtSequence.TabIndex = 6;
            txtSequence.Text = "1";
            txtSequence.TextAlign = HorizontalAlignment.Right;
            txtSequence.KeyPress += txtSequence_KeyPress;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 14.25F);
            label5.ForeColor = Color.White;
            label5.Location = new Point(466, 130);
            label5.Name = "label5";
            label5.Size = new Size(137, 25);
            label5.TabIndex = 12;
            label5.Text = "Sequence Start";
            // 
            // txtRequirements
            // 
            txtRequirements.BackColor = Color.FromArgb(20, 20, 20);
            txtRequirements.BorderStyle = BorderStyle.FixedSingle;
            txtRequirements.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtRequirements.ForeColor = Color.Lime;
            txtRequirements.Location = new Point(612, 69);
            txtRequirements.Name = "txtRequirements";
            txtRequirements.Size = new Size(998, 33);
            txtRequirements.TabIndex = 4;
            txtRequirements.Text = "REQ-";
            txtRequirements.KeyPress += TxtRequirements_KeyPress;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 14.25F);
            label6.ForeColor = Color.White;
            label6.Location = new Point(466, 77);
            label6.Name = "label6";
            label6.Size = new Size(127, 25);
            label6.TabIndex = 14;
            label6.Text = "Requirements";
            // 
            // BtnHelp
            // 
            BtnHelp.FlatStyle = FlatStyle.System;
            BtnHelp.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            BtnHelp.ForeColor = Color.LightGray;
            BtnHelp.Location = new Point(1635, 9);
            BtnHelp.Name = "BtnHelp";
            BtnHelp.Size = new Size(41, 40);
            BtnHelp.TabIndex = 16;
            BtnHelp.Text = "❓";
            BtnHelp.UseVisualStyleBackColor = true;
            BtnHelp.Click += BtnHelp_Click;
            // 
            // txtFeatureTitle
            // 
            txtFeatureTitle.BackColor = Color.FromArgb(20, 20, 20);
            txtFeatureTitle.BorderStyle = BorderStyle.FixedSingle;
            txtFeatureTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtFeatureTitle.ForeColor = Color.Lime;
            txtFeatureTitle.Location = new Point(612, 16);
            txtFeatureTitle.Name = "txtFeatureTitle";
            txtFeatureTitle.Size = new Size(998, 33);
            txtFeatureTitle.TabIndex = 2;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 14.25F);
            label7.ForeColor = Color.White;
            label7.Location = new Point(466, 21);
            label7.Name = "label7";
            label7.Size = new Size(116, 25);
            label7.TabIndex = 17;
            label7.Text = "Feature Title";
            // 
            // lblTestCasePrefix
            // 
            lblTestCasePrefix.AutoSize = true;
            lblTestCasePrefix.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            lblTestCasePrefix.ForeColor = Color.Lime;
            lblTestCasePrefix.Location = new Point(242, 21);
            lblTestCasePrefix.Name = "lblTestCasePrefix";
            lblTestCasePrefix.Size = new Size(53, 25);
            lblTestCasePrefix.TabIndex = 18;
            lblTestCasePrefix.Text = "TST-";
            // 
            // chkClearOnDrop
            // 
            chkClearOnDrop.AutoSize = true;
            chkClearOnDrop.Checked = true;
            chkClearOnDrop.CheckState = CheckState.Checked;
            chkClearOnDrop.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkClearOnDrop.ForeColor = Color.White;
            chkClearOnDrop.Location = new Point(1525, 145);
            chkClearOnDrop.Name = "chkClearOnDrop";
            chkClearOnDrop.Size = new Size(149, 29);
            chkClearOnDrop.TabIndex = 19;
            chkClearOnDrop.Text = "Clear on Drop";
            chkClearOnDrop.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            btnCopy.FlatStyle = FlatStyle.System;
            btnCopy.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCopy.ForeColor = Color.LightGray;
            btnCopy.Location = new Point(12, 929);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(220, 40);
            btnCopy.TabIndex = 20;
            btnCopy.Text = "Copy Content";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;
            // 
            // btnLocate
            // 
            btnLocate.FlatStyle = FlatStyle.System;
            btnLocate.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLocate.ForeColor = Color.LightGray;
            btnLocate.Location = new Point(258, 929);
            btnLocate.Name = "btnLocate";
            btnLocate.Size = new Size(220, 40);
            btnLocate.TabIndex = 21;
            btnLocate.Text = "Open Feature File";
            btnLocate.UseVisualStyleBackColor = true;
            btnLocate.Click += btnLocate_Click;
            // 
            // btnWriteToFeatureFile
            // 
            btnWriteToFeatureFile.FlatStyle = FlatStyle.System;
            btnWriteToFeatureFile.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnWriteToFeatureFile.ForeColor = Color.LightGray;
            btnWriteToFeatureFile.Location = new Point(505, 929);
            btnWriteToFeatureFile.Name = "btnWriteToFeatureFile";
            btnWriteToFeatureFile.Size = new Size(220, 40);
            btnWriteToFeatureFile.TabIndex = 22;
            btnWriteToFeatureFile.Text = "Write to Feature File";
            btnWriteToFeatureFile.UseVisualStyleBackColor = true;
            btnWriteToFeatureFile.Click += btnWriteToFeatureFile_Click;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(20, 20, 20);
            ClientSize = new Size(1686, 981);
            Controls.Add(btnWriteToFeatureFile);
            Controls.Add(btnLocate);
            Controls.Add(btnCopy);
            Controls.Add(chkClearOnDrop);
            Controls.Add(lblTestCasePrefix);
            Controls.Add(txtFeatureTitle);
            Controls.Add(label7);
            Controls.Add(BtnHelp);
            Controls.Add(txtRequirements);
            Controls.Add(label6);
            Controls.Add(txtSequence);
            Controls.Add(label5);
            Controls.Add(txtTSTID);
            Controls.Add(label4);
            Controls.Add(drpDomain);
            Controls.Add(label3);
            Controls.Add(txtParent);
            Controls.Add(label2);
            Controls.Add(txtLevel);
            Controls.Add(label1);
            Controls.Add(txtGherkin);
            ForeColor = SystemColors.ActiveCaptionText;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "frmMain";
            Opacity = 0.97D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "G H E R K I N I Z E R";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RichTextBox txtGherkin;
        private Label label1;
        private TextBox txtLevel;
        private TextBox txtParent;
        private Label label2;
        private Label label3;
        private ComboBox drpDomain;
        private TextBox txtTSTID;
        private Label label4;
        private TextBox txtSequence;
        private Label label5;
        private TextBox txtRequirements;
        private Label label6;
        private Button BtnHelp;
        private TextBox txtFeatureTitle;
        private Label label7;
        private Label lblTestCasePrefix;
        private CheckBox chkClearOnDrop;
        private Button btnCopy;
        private Button btnLocate;
        private Button btnWriteToFeatureFile;
    }
}
