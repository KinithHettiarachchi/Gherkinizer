namespace MindMapToGherkin
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
            components = new System.ComponentModel.Container();
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
            btnLoadMindMap = new Button();
            statusStrip1 = new StatusStrip();
            lblFilePath = new ToolStripStatusLabel();
            btnReload = new Button();
            btnOpenMindmap = new Button();
            bttValidate = new Button();
            toolTip1 = new ToolTip(components);
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            panel1 = new Panel();
            btnCreateMindMap = new Button();
            chkOpenMindMapFile = new CheckBox();
            chkOpenMindMapFolder = new CheckBox();
            txtMindMapPath = new TextBox();
            tree = new TreeView();
            label10 = new Label();
            txtMindMapTitle = new TextBox();
            txtMindMapTaskID = new TextBox();
            lblMindMapTaskPrefix = new Label();
            label9 = new Label();
            txtMindMapEpicTitle = new TextBox();
            txtMindMapMainEpicID = new TextBox();
            lblMindMapMainEpicPrefix = new Label();
            label8 = new Label();
            tabPage3 = new TabPage();
            txtMindMap = new RichTextBox();
            statusStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            panel1.SuspendLayout();
            tabPage3.SuspendLayout();
            SuspendLayout();
            // 
            // txtGherkin
            // 
            txtGherkin.BackColor = Color.White;
            txtGherkin.BorderStyle = BorderStyle.FixedSingle;
            txtGherkin.Cursor = Cursors.IBeam;
            txtGherkin.Font = new Font("Cascadia Code", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtGherkin.ForeColor = Color.Black;
            txtGherkin.Location = new Point(8, 184);
            txtGherkin.Name = "txtGherkin";
            txtGherkin.ReadOnly = true;
            txtGherkin.Size = new Size(1662, 703);
            txtGherkin.TabIndex = 1;
            txtGherkin.TabStop = false;
            txtGherkin.Text = "";
            txtGherkin.WordWrap = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.ButtonFace;
            label1.Font = new Font("Segoe UI", 14.25F);
            label1.ForeColor = SystemColors.ActiveCaptionText;
            label1.Location = new Point(797, 137);
            label1.Name = "label1";
            label1.Size = new Size(157, 25);
            label1.TabIndex = 2;
            label1.Text = "7️⃣ Scenario Level";
            // 
            // txtLevel
            // 
            txtLevel.BackColor = Color.White;
            txtLevel.BorderStyle = BorderStyle.FixedSingle;
            txtLevel.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtLevel.ForeColor = SystemColors.ActiveCaptionText;
            txtLevel.Location = new Point(960, 129);
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
            txtParent.BackColor = Color.White;
            txtParent.BorderStyle = BorderStyle.FixedSingle;
            txtParent.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtParent.ForeColor = SystemColors.ActiveCaptionText;
            txtParent.Location = new Point(265, 129);
            txtParent.Name = "txtParent";
            txtParent.Size = new Size(164, 33);
            txtParent.TabIndex = 5;
            txtParent.Text = "Functional Tests";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.ButtonFace;
            label2.Font = new Font("Segoe UI", 14.25F);
            label2.ForeColor = SystemColors.ActiveCaptionText;
            label2.Location = new Point(8, 137);
            label2.Name = "label2";
            label2.Size = new Size(208, 25);
            label2.TabIndex = 4;
            label2.Text = "5️⃣ Parent Node of Tests";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.ButtonFace;
            label3.Font = new Font("Segoe UI", 14.25F);
            label3.ForeColor = SystemColors.ActiveCaptionText;
            label3.Location = new Point(8, 78);
            label3.Name = "label3";
            label3.Size = new Size(102, 25);
            label3.TabIndex = 8;
            label3.Text = "3️⃣ Domain";
            // 
            // drpDomain
            // 
            drpDomain.BackColor = SystemColors.Control;
            drpDomain.Cursor = Cursors.Hand;
            drpDomain.DropDownStyle = ComboBoxStyle.DropDownList;
            drpDomain.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            drpDomain.ForeColor = SystemColors.ActiveCaptionText;
            drpDomain.FormattingEnabled = true;
            drpDomain.Items.AddRange(new object[] { " " });
            drpDomain.Location = new Point(265, 70);
            drpDomain.Name = "drpDomain";
            drpDomain.Size = new Size(164, 33);
            drpDomain.TabIndex = 3;
            // 
            // txtTSTID
            // 
            txtTSTID.BackColor = Color.White;
            txtTSTID.BorderStyle = BorderStyle.FixedSingle;
            txtTSTID.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtTSTID.ForeColor = SystemColors.ActiveCaptionText;
            txtTSTID.Location = new Point(320, 18);
            txtTSTID.Name = "txtTSTID";
            txtTSTID.Size = new Size(109, 33);
            txtTSTID.TabIndex = 1;
            txtTSTID.TextChanged += txtTSTID_TextChanged;
            txtTSTID.KeyPress += TxtTSTID_KeyPress;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = SystemColors.ButtonFace;
            label4.Font = new Font("Segoe UI", 14.25F);
            label4.ForeColor = SystemColors.ActiveCaptionText;
            label4.Location = new Point(8, 22);
            label4.Name = "label4";
            label4.Size = new Size(240, 25);
            label4.TabIndex = 10;
            label4.Text = "1️⃣ Testable Functionality ID";
            // 
            // txtSequence
            // 
            txtSequence.BackColor = Color.White;
            txtSequence.BorderStyle = BorderStyle.FixedSingle;
            txtSequence.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtSequence.ForeColor = SystemColors.ActiveCaptionText;
            txtSequence.Location = new Point(675, 129);
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
            label5.BackColor = SystemColors.ButtonFace;
            label5.Font = new Font("Segoe UI", 14.25F);
            label5.ForeColor = SystemColors.ActiveCaptionText;
            label5.Location = new Point(497, 137);
            label5.Name = "label5";
            label5.Size = new Size(161, 25);
            label5.TabIndex = 12;
            label5.Text = "6️⃣ Sequence Start";
            // 
            // txtRequirements
            // 
            txtRequirements.BackColor = Color.White;
            txtRequirements.BorderStyle = BorderStyle.FixedSingle;
            txtRequirements.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtRequirements.ForeColor = SystemColors.ActiveCaptionText;
            txtRequirements.Location = new Point(675, 78);
            txtRequirements.Name = "txtRequirements";
            txtRequirements.Size = new Size(994, 33);
            txtRequirements.TabIndex = 4;
            txtRequirements.Text = "REQ-";
            txtRequirements.KeyPress += TxtRequirements_KeyPress;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = SystemColors.ButtonFace;
            label6.Font = new Font("Segoe UI", 14.25F);
            label6.ForeColor = SystemColors.ActiveCaptionText;
            label6.Location = new Point(497, 78);
            label6.Name = "label6";
            label6.Size = new Size(151, 25);
            label6.TabIndex = 14;
            label6.Text = "4️⃣ Requirements";
            // 
            // BtnHelp
            // 
            BtnHelp.BackColor = SystemColors.ButtonFace;
            BtnHelp.FlatStyle = FlatStyle.System;
            BtnHelp.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            BtnHelp.ForeColor = SystemColors.ActiveCaptionText;
            BtnHelp.Location = new Point(1629, 128);
            BtnHelp.Name = "BtnHelp";
            BtnHelp.Size = new Size(41, 40);
            BtnHelp.TabIndex = 16;
            BtnHelp.Text = "❓";
            toolTip1.SetToolTip(BtnHelp, "Help");
            BtnHelp.UseVisualStyleBackColor = false;
            BtnHelp.Click += BtnHelp_Click;
            // 
            // txtFeatureTitle
            // 
            txtFeatureTitle.BackColor = Color.White;
            txtFeatureTitle.BorderStyle = BorderStyle.FixedSingle;
            txtFeatureTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtFeatureTitle.ForeColor = SystemColors.ActiveCaptionText;
            txtFeatureTitle.Location = new Point(675, 17);
            txtFeatureTitle.Name = "txtFeatureTitle";
            txtFeatureTitle.Size = new Size(994, 33);
            txtFeatureTitle.TabIndex = 2;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = SystemColors.ButtonFace;
            label7.Font = new Font("Segoe UI", 14.25F);
            label7.ForeColor = SystemColors.ActiveCaptionText;
            label7.Location = new Point(497, 22);
            label7.Name = "label7";
            label7.Size = new Size(140, 25);
            label7.TabIndex = 17;
            label7.Text = "2️⃣ Feature Title";
            // 
            // lblTestCasePrefix
            // 
            lblTestCasePrefix.AutoSize = true;
            lblTestCasePrefix.BackColor = SystemColors.ButtonFace;
            lblTestCasePrefix.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            lblTestCasePrefix.ForeColor = SystemColors.ActiveCaptionText;
            lblTestCasePrefix.Location = new Point(265, 22);
            lblTestCasePrefix.Name = "lblTestCasePrefix";
            lblTestCasePrefix.Size = new Size(53, 25);
            lblTestCasePrefix.TabIndex = 18;
            lblTestCasePrefix.Text = "TST-";
            // 
            // chkClearOnDrop
            // 
            chkClearOnDrop.AutoSize = true;
            chkClearOnDrop.BackColor = SystemColors.ButtonFace;
            chkClearOnDrop.Checked = true;
            chkClearOnDrop.CheckState = CheckState.Checked;
            chkClearOnDrop.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkClearOnDrop.ForeColor = SystemColors.ActiveCaptionText;
            chkClearOnDrop.Location = new Point(1077, 133);
            chkClearOnDrop.Name = "chkClearOnDrop";
            chkClearOnDrop.Size = new Size(157, 29);
            chkClearOnDrop.TabIndex = 19;
            chkClearOnDrop.Text = "Clear Text Area";
            chkClearOnDrop.UseVisualStyleBackColor = false;
            // 
            // btnCopy
            // 
            btnCopy.BackColor = SystemColors.ButtonFace;
            btnCopy.FlatStyle = FlatStyle.System;
            btnCopy.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCopy.ForeColor = SystemColors.ActiveCaptionText;
            btnCopy.Location = new Point(930, 893);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(240, 40);
            btnCopy.TabIndex = 20;
            btnCopy.Text = "📋 Copy Content";
            btnCopy.UseVisualStyleBackColor = false;
            btnCopy.Click += btnCopy_Click;
            // 
            // btnLocate
            // 
            btnLocate.BackColor = SystemColors.ButtonFace;
            btnLocate.FlatStyle = FlatStyle.System;
            btnLocate.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLocate.ForeColor = SystemColors.ActiveCaptionText;
            btnLocate.Location = new Point(1180, 893);
            btnLocate.Name = "btnLocate";
            btnLocate.Size = new Size(240, 40);
            btnLocate.TabIndex = 21;
            btnLocate.Text = "📂 Open Feature File";
            btnLocate.UseVisualStyleBackColor = false;
            btnLocate.Click += btnLocate_Click;
            // 
            // btnWriteToFeatureFile
            // 
            btnWriteToFeatureFile.BackColor = SystemColors.ButtonFace;
            btnWriteToFeatureFile.FlatStyle = FlatStyle.System;
            btnWriteToFeatureFile.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnWriteToFeatureFile.ForeColor = SystemColors.ActiveCaptionText;
            btnWriteToFeatureFile.Location = new Point(1429, 893);
            btnWriteToFeatureFile.Name = "btnWriteToFeatureFile";
            btnWriteToFeatureFile.Size = new Size(240, 40);
            btnWriteToFeatureFile.TabIndex = 22;
            btnWriteToFeatureFile.Text = "✍️ Write to Feature File";
            btnWriteToFeatureFile.UseVisualStyleBackColor = false;
            btnWriteToFeatureFile.Click += btnWriteToFeatureFile_Click;
            // 
            // btnLoadMindMap
            // 
            btnLoadMindMap.BackColor = SystemColors.ButtonFace;
            btnLoadMindMap.FlatStyle = FlatStyle.System;
            btnLoadMindMap.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLoadMindMap.ForeColor = SystemColors.ActiveCaptionText;
            btnLoadMindMap.Location = new Point(8, 893);
            btnLoadMindMap.Name = "btnLoadMindMap";
            btnLoadMindMap.Size = new Size(240, 40);
            btnLoadMindMap.TabIndex = 23;
            btnLoadMindMap.Text = "🔄 Load Mind Map...";
            btnLoadMindMap.UseVisualStyleBackColor = false;
            btnLoadMindMap.Click += btnLoadMindMap_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = SystemColors.ButtonFace;
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblFilePath });
            statusStrip1.Location = new Point(0, 979);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.RenderMode = ToolStripRenderMode.Professional;
            statusStrip1.Size = new Size(1686, 22);
            statusStrip1.TabIndex = 24;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblFilePath
            // 
            lblFilePath.BackColor = SystemColors.ButtonFace;
            lblFilePath.ForeColor = SystemColors.ControlDarkDark;
            lblFilePath.Name = "lblFilePath";
            lblFilePath.Size = new Size(84, 17);
            lblFilePath.Text = "No file loaded!";
            // 
            // btnReload
            // 
            btnReload.BackColor = SystemColors.ButtonFace;
            btnReload.FlatStyle = FlatStyle.System;
            btnReload.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnReload.ForeColor = SystemColors.ActiveCaptionText;
            btnReload.Location = new Point(1458, 128);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(118, 40);
            btnReload.TabIndex = 25;
            btnReload.Text = "🔄 Reload";
            toolTip1.SetToolTip(btnReload, "Reload Mind Map File");
            btnReload.UseVisualStyleBackColor = false;
            btnReload.Click += btnReload_Click;
            // 
            // btnOpenMindmap
            // 
            btnOpenMindmap.BackColor = SystemColors.ButtonFace;
            btnOpenMindmap.FlatStyle = FlatStyle.System;
            btnOpenMindmap.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnOpenMindmap.ForeColor = SystemColors.ActiveCaptionText;
            btnOpenMindmap.Location = new Point(418, 893);
            btnOpenMindmap.Name = "btnOpenMindmap";
            btnOpenMindmap.Size = new Size(240, 40);
            btnOpenMindmap.TabIndex = 26;
            btnOpenMindmap.Text = "↗ Open in Freeplane";
            btnOpenMindmap.UseVisualStyleBackColor = false;
            btnOpenMindmap.Click += btnOpenMindmap_Click;
            // 
            // bttValidate
            // 
            bttValidate.BackColor = SystemColors.ButtonFace;
            bttValidate.FlatStyle = FlatStyle.System;
            bttValidate.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            bttValidate.ForeColor = SystemColors.ActiveCaptionText;
            bttValidate.Location = new Point(1582, 128);
            bttValidate.Name = "bttValidate";
            bttValidate.Size = new Size(41, 40);
            bttValidate.TabIndex = 27;
            bttValidate.Text = "📝";
            toolTip1.SetToolTip(bttValidate, "Validate Syntax");
            bttValidate.UseVisualStyleBackColor = false;
            bttValidate.Click += bttValidate_Click;
            // 
            // toolTip1
            // 
            toolTip1.BackColor = SystemColors.ButtonFace;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Location = new Point(0, 2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1686, 969);
            tabControl1.TabIndex = 28;
            tabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = SystemColors.Control;
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(bttValidate);
            tabPage1.Controls.Add(txtGherkin);
            tabPage1.Controls.Add(btnOpenMindmap);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(btnReload);
            tabPage1.Controls.Add(txtLevel);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(btnLoadMindMap);
            tabPage1.Controls.Add(txtParent);
            tabPage1.Controls.Add(btnWriteToFeatureFile);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(btnLocate);
            tabPage1.Controls.Add(drpDomain);
            tabPage1.Controls.Add(btnCopy);
            tabPage1.Controls.Add(txtTSTID);
            tabPage1.Controls.Add(chkClearOnDrop);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(lblTestCasePrefix);
            tabPage1.Controls.Add(txtSequence);
            tabPage1.Controls.Add(txtFeatureTitle);
            tabPage1.Controls.Add(label6);
            tabPage1.Controls.Add(label7);
            tabPage1.Controls.Add(txtRequirements);
            tabPage1.Controls.Add(BtnHelp);
            tabPage1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1678, 941);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Mind Map to Gherkin";
            // 
            // tabPage2
            // 
            tabPage2.BackColor = SystemColors.ButtonFace;
            tabPage2.Controls.Add(panel1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1678, 941);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Generate Mind Map";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ButtonFace;
            panel1.Controls.Add(btnCreateMindMap);
            panel1.Controls.Add(chkOpenMindMapFile);
            panel1.Controls.Add(chkOpenMindMapFolder);
            panel1.Controls.Add(txtMindMapPath);
            panel1.Controls.Add(tree);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(txtMindMapTitle);
            panel1.Controls.Add(txtMindMapTaskID);
            panel1.Controls.Add(lblMindMapTaskPrefix);
            panel1.Controls.Add(label9);
            panel1.Controls.Add(txtMindMapEpicTitle);
            panel1.Controls.Add(txtMindMapMainEpicID);
            panel1.Controls.Add(lblMindMapMainEpicPrefix);
            panel1.Controls.Add(label8);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1672, 935);
            panel1.TabIndex = 0;
            // 
            // btnCreateMindMap
            // 
            btnCreateMindMap.BackColor = SystemColors.ButtonFace;
            btnCreateMindMap.FlatStyle = FlatStyle.System;
            btnCreateMindMap.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCreateMindMap.ForeColor = SystemColors.ActiveCaptionText;
            btnCreateMindMap.Location = new Point(1393, 877);
            btnCreateMindMap.Name = "btnCreateMindMap";
            btnCreateMindMap.Size = new Size(240, 40);
            btnCreateMindMap.TabIndex = 31;
            btnCreateMindMap.Text = "✍️ Create Mind map";
            btnCreateMindMap.UseVisualStyleBackColor = false;
            btnCreateMindMap.Click += btnCreateMindMap_Click;
            // 
            // chkOpenMindMapFile
            // 
            chkOpenMindMapFile.AutoSize = true;
            chkOpenMindMapFile.BackColor = SystemColors.ButtonFace;
            chkOpenMindMapFile.Checked = true;
            chkOpenMindMapFile.CheckState = CheckState.Checked;
            chkOpenMindMapFile.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkOpenMindMapFile.ForeColor = SystemColors.ActiveCaptionText;
            chkOpenMindMapFile.Location = new Point(1101, 885);
            chkOpenMindMapFile.Name = "chkOpenMindMapFile";
            chkOpenMindMapFile.Size = new Size(234, 29);
            chkOpenMindMapFile.TabIndex = 30;
            chkOpenMindMapFile.Text = "Open File After Creation";
            chkOpenMindMapFile.UseVisualStyleBackColor = false;
            // 
            // chkOpenMindMapFolder
            // 
            chkOpenMindMapFolder.AutoSize = true;
            chkOpenMindMapFolder.BackColor = SystemColors.ButtonFace;
            chkOpenMindMapFolder.Checked = true;
            chkOpenMindMapFolder.CheckState = CheckState.Checked;
            chkOpenMindMapFolder.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkOpenMindMapFolder.ForeColor = SystemColors.ActiveCaptionText;
            chkOpenMindMapFolder.Location = new Point(774, 885);
            chkOpenMindMapFolder.Name = "chkOpenMindMapFolder";
            chkOpenMindMapFolder.Size = new Size(258, 29);
            chkOpenMindMapFolder.TabIndex = 29;
            chkOpenMindMapFolder.Text = "Open Folder After Creation";
            chkOpenMindMapFolder.UseVisualStyleBackColor = false;
            // 
            // txtMindMapPath
            // 
            txtMindMapPath.BackColor = Color.White;
            txtMindMapPath.BorderStyle = BorderStyle.FixedSingle;
            txtMindMapPath.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtMindMapPath.ForeColor = Color.Black;
            txtMindMapPath.Location = new Point(265, 162);
            txtMindMapPath.Multiline = true;
            txtMindMapPath.Name = "txtMindMapPath";
            txtMindMapPath.Size = new Size(1368, 70);
            txtMindMapPath.TabIndex = 28;
            // 
            // tree
            // 
            tree.BackColor = Color.White;
            tree.Font = new Font("Consolas", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tree.ForeColor = SystemColors.ActiveCaptionText;
            tree.LineColor = Color.Gainsboro;
            tree.Location = new Point(265, 238);
            tree.Name = "tree";
            tree.Size = new Size(1368, 620);
            tree.TabIndex = 27;
            tree.AfterSelect += tree_AfterSelect;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = SystemColors.ButtonFace;
            label10.Font = new Font("Segoe UI", 14.25F);
            label10.ForeColor = SystemColors.ActiveCaptionText;
            label10.Location = new Point(33, 170);
            label10.Name = "label10";
            label10.Size = new Size(108, 25);
            label10.TabIndex = 26;
            label10.Text = "3️⃣ Location";
            // 
            // txtMindMapTitle
            // 
            txtMindMapTitle.BackColor = Color.White;
            txtMindMapTitle.BorderStyle = BorderStyle.FixedSingle;
            txtMindMapTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtMindMapTitle.ForeColor = Color.Black;
            txtMindMapTitle.Location = new Point(445, 97);
            txtMindMapTitle.Name = "txtMindMapTitle";
            txtMindMapTitle.Size = new Size(1188, 33);
            txtMindMapTitle.TabIndex = 25;
            // 
            // txtMindMapTaskID
            // 
            txtMindMapTaskID.BackColor = Color.White;
            txtMindMapTaskID.BorderStyle = BorderStyle.FixedSingle;
            txtMindMapTaskID.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtMindMapTaskID.ForeColor = Color.Black;
            txtMindMapTaskID.Location = new Point(320, 97);
            txtMindMapTaskID.Name = "txtMindMapTaskID";
            txtMindMapTaskID.Size = new Size(109, 33);
            txtMindMapTaskID.TabIndex = 23;
            txtMindMapTaskID.KeyPress += TxtRequirements_KeyPress;
            // 
            // lblMindMapTaskPrefix
            // 
            lblMindMapTaskPrefix.AutoSize = true;
            lblMindMapTaskPrefix.BackColor = SystemColors.ButtonFace;
            lblMindMapTaskPrefix.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            lblMindMapTaskPrefix.ForeColor = Color.Black;
            lblMindMapTaskPrefix.Location = new Point(265, 101);
            lblMindMapTaskPrefix.Name = "lblMindMapTaskPrefix";
            lblMindMapTaskPrefix.Size = new Size(57, 25);
            lblMindMapTaskPrefix.TabIndex = 24;
            lblMindMapTaskPrefix.Text = "DEV-";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = SystemColors.ButtonFace;
            label9.Font = new Font("Segoe UI", 14.25F);
            label9.ForeColor = SystemColors.ActiveCaptionText;
            label9.Location = new Point(33, 105);
            label9.Name = "label9";
            label9.Size = new Size(163, 25);
            label9.TabIndex = 22;
            label9.Text = "2️⃣ Mind Map Task";
            // 
            // txtMindMapEpicTitle
            // 
            txtMindMapEpicTitle.BackColor = Color.White;
            txtMindMapEpicTitle.BorderStyle = BorderStyle.FixedSingle;
            txtMindMapEpicTitle.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtMindMapEpicTitle.ForeColor = Color.Black;
            txtMindMapEpicTitle.Location = new Point(445, 24);
            txtMindMapEpicTitle.Name = "txtMindMapEpicTitle";
            txtMindMapEpicTitle.Size = new Size(1188, 33);
            txtMindMapEpicTitle.TabIndex = 21;
            // 
            // txtMindMapMainEpicID
            // 
            txtMindMapMainEpicID.BackColor = Color.White;
            txtMindMapMainEpicID.BorderStyle = BorderStyle.FixedSingle;
            txtMindMapMainEpicID.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            txtMindMapMainEpicID.ForeColor = Color.Black;
            txtMindMapMainEpicID.Location = new Point(320, 24);
            txtMindMapMainEpicID.Name = "txtMindMapMainEpicID";
            txtMindMapMainEpicID.Size = new Size(109, 33);
            txtMindMapMainEpicID.TabIndex = 19;
            txtMindMapMainEpicID.KeyPress += TxtRequirements_KeyPress;
            // 
            // lblMindMapMainEpicPrefix
            // 
            lblMindMapMainEpicPrefix.AutoSize = true;
            lblMindMapMainEpicPrefix.BackColor = SystemColors.ButtonFace;
            lblMindMapMainEpicPrefix.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            lblMindMapMainEpicPrefix.ForeColor = Color.Black;
            lblMindMapMainEpicPrefix.Location = new Point(265, 28);
            lblMindMapMainEpicPrefix.Name = "lblMindMapMainEpicPrefix";
            lblMindMapMainEpicPrefix.Size = new Size(57, 25);
            lblMindMapMainEpicPrefix.TabIndex = 20;
            lblMindMapMainEpicPrefix.Text = "DEV-";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = SystemColors.ButtonFace;
            label8.Font = new Font("Segoe UI", 14.25F);
            label8.ForeColor = SystemColors.ActiveCaptionText;
            label8.Location = new Point(33, 32);
            label8.Name = "label8";
            label8.Size = new Size(119, 25);
            label8.TabIndex = 11;
            label8.Text = "1️⃣ Main Epic";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(txtMindMap);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1678, 941);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Gherkin to Mindmap";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtMindMap
            // 
            txtMindMap.BackColor = Color.White;
            txtMindMap.BorderStyle = BorderStyle.FixedSingle;
            txtMindMap.Cursor = Cursors.IBeam;
            txtMindMap.Font = new Font("Cascadia Code", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMindMap.ForeColor = Color.Black;
            txtMindMap.Location = new Point(8, 119);
            txtMindMap.Name = "txtMindMap";
            txtMindMap.ReadOnly = true;
            txtMindMap.Size = new Size(1662, 703);
            txtMindMap.TabIndex = 2;
            txtMindMap.TabStop = false;
            txtMindMap.Text = "";
            txtMindMap.WordWrap = false;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(1686, 1001);
            Controls.Add(tabControl1);
            Controls.Add(statusStrip1);
            ForeColor = SystemColors.ActiveCaptionText;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "frmMain";
            Opacity = 0.97D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "G H E R K I N I Z E R";
            Load += Form1_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tabPage3.ResumeLayout(false);
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
        private Button btnLoadMindMap;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblFilePath;
        private Button btnReload;
        private Button btnOpenMindmap;
        private Button bttValidate;
        private ToolTip toolTip1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Panel panel1;
        private Label label8;
        private TextBox txtMindMapMainEpicID;
        private Label lblMindMapMainEpicPrefix;
        private TextBox txtMindMapEpicTitle;
        private Label label9;
        private TextBox txtMindMapTitle;
        private TextBox txtMindMapTaskID;
        private Label lblMindMapTaskPrefix;
        private Label label10;
        private TreeView tree;
        private TextBox txtMindMapPath;
        private CheckBox chkOpenMindMapFile;
        private CheckBox chkOpenMindMapFolder;
        private Button btnCreateMindMap;
        private TabPage tabPage3;
        private RichTextBox txtMindMap;
    }
}
