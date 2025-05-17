namespace MindMapToGherkin
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            txtGherkin = new RichTextBox();
            label1 = new Label();
            txtLevel = new TextBox();
            txtParent = new TextBox();
            label2 = new Label();
            btnClear = new Button();
            label3 = new Label();
            drpDomain = new ComboBox();
            txtTSTID = new TextBox();
            label4 = new Label();
            txtSequence = new TextBox();
            label5 = new Label();
            txtRequirements = new TextBox();
            label6 = new Label();
            BtnHelp = new Button();
            SuspendLayout();
            // 
            // txtGherkin
            // 
            txtGherkin.BackColor = SystemColors.Control;
            txtGherkin.Font = new Font("Consolas", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtGherkin.ForeColor = SystemColors.ActiveCaptionText;
            txtGherkin.Location = new Point(12, 56);
            txtGherkin.Name = "txtGherkin";
            txtGherkin.ReadOnly = true;
            txtGherkin.Size = new Size(1662, 863);
            txtGherkin.TabIndex = 1;
            txtGherkin.Text = "";
            txtGherkin.WordWrap = false;
            txtGherkin.TextChanged += txtGherkin_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(1286, 24);
            label1.Name = "label1";
            label1.Size = new Size(120, 21);
            label1.TabIndex = 2;
            label1.Text = "Scenario Level";
            // 
            // txtLevel
            // 
            txtLevel.Font = new Font("Segoe UI", 12F);
            txtLevel.Location = new Point(1419, 16);
            txtLevel.Name = "txtLevel";
            txtLevel.Size = new Size(33, 29);
            txtLevel.TabIndex = 3;
            txtLevel.Text = "1";
            txtLevel.TextAlign = HorizontalAlignment.Right;
            // 
            // txtParent
            // 
            txtParent.Font = new Font("Segoe UI", 12F);
            txtParent.Location = new Point(185, 20);
            txtParent.Name = "txtParent";
            txtParent.Size = new Size(140, 29);
            txtParent.TabIndex = 5;
            txtParent.Text = "Functional Tests";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 22);
            label2.Name = "label2";
            label2.Size = new Size(167, 21);
            label2.TabIndex = 4;
            label2.Text = "Parent Node of Tests";
            // 
            // btnClear
            // 
            btnClear.Font = new Font("Segoe UI", 12F);
            btnClear.ForeColor = Color.Black;
            btnClear.Location = new Point(1476, 9);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 40);
            btnClear.TabIndex = 6;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label3.Location = new Point(333, 22);
            label3.Name = "label3";
            label3.Size = new Size(71, 21);
            label3.TabIndex = 8;
            label3.Text = "Domain";
            // 
            // drpDomain
            // 
            drpDomain.DropDownStyle = ComboBoxStyle.DropDownList;
            drpDomain.Font = new Font("Segoe UI", 12F);
            drpDomain.FormattingEnabled = true;
            drpDomain.Items.AddRange(new object[] { "EX", "LG", "OR", "ST", "XL", "IN", "WB" });
            drpDomain.Location = new Point(405, 16);
            drpDomain.Name = "drpDomain";
            drpDomain.Size = new Size(66, 29);
            drpDomain.TabIndex = 9;
            // 
            // txtTSTID
            // 
            txtTSTID.Font = new Font("Segoe UI", 12F);
            txtTSTID.Location = new Point(552, 17);
            txtTSTID.Name = "txtTSTID";
            txtTSTID.Size = new Size(56, 29);
            txtTSTID.TabIndex = 11;
            txtTSTID.Text = "00000";
            txtTSTID.TextAlign = HorizontalAlignment.Right;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label4.Location = new Point(492, 20);
            label4.Name = "label4";
            label4.Size = new Size(58, 21);
            label4.TabIndex = 10;
            label4.Text = "TST ID";
            // 
            // txtSequence
            // 
            txtSequence.Font = new Font("Segoe UI", 12F);
            txtSequence.Location = new Point(760, 17);
            txtSequence.Name = "txtSequence";
            txtSequence.Size = new Size(56, 29);
            txtSequence.TabIndex = 13;
            txtSequence.Text = "1";
            txtSequence.TextAlign = HorizontalAlignment.Right;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label5.Location = new Point(638, 22);
            label5.Name = "label5";
            label5.Size = new Size(124, 21);
            label5.TabIndex = 12;
            label5.Text = "Sequence Start";
            // 
            // txtRequirements
            // 
            txtRequirements.Font = new Font("Segoe UI", 12F);
            txtRequirements.Location = new Point(959, 17);
            txtRequirements.Name = "txtRequirements";
            txtRequirements.Size = new Size(312, 29);
            txtRequirements.TabIndex = 15;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label6.Location = new Point(840, 22);
            label6.Name = "label6";
            label6.Size = new Size(116, 21);
            label6.TabIndex = 14;
            label6.Text = "Requirements";
            // 
            // BtnHelp
            // 
            BtnHelp.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
            BtnHelp.ForeColor = Color.Black;
            BtnHelp.Location = new Point(1633, 9);
            BtnHelp.Name = "BtnHelp";
            BtnHelp.Size = new Size(41, 40);
            BtnHelp.TabIndex = 16;
            BtnHelp.Text = "?";
            BtnHelp.UseVisualStyleBackColor = true;
            BtnHelp.Click += BtnHelp_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1686, 929);
            Controls.Add(BtnHelp);
            Controls.Add(txtRequirements);
            Controls.Add(label6);
            Controls.Add(txtSequence);
            Controls.Add(label5);
            Controls.Add(txtTSTID);
            Controls.Add(label4);
            Controls.Add(drpDomain);
            Controls.Add(label3);
            Controls.Add(btnClear);
            Controls.Add(txtParent);
            Controls.Add(label2);
            Controls.Add(txtLevel);
            Controls.Add(label1);
            Controls.Add(txtGherkin);
            ForeColor = SystemColors.ActiveCaptionText;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Opacity = 0.97D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mind Map to Gherkin";
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
        private Button btnClear;
        private Label label3;
        private ComboBox drpDomain;
        private TextBox txtTSTID;
        private Label label4;
        private TextBox txtSequence;
        private Label label5;
        private TextBox txtRequirements;
        private Label label6;
        private Button BtnHelp;
    }
}
