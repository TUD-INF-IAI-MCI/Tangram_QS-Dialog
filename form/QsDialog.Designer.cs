namespace tud.mci.tangram.qsdialog
{
    partial class QsDialog
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
            this.treeView_Steps = new System.Windows.Forms.TreeView();
            this.panel_CriteriaContent = new System.Windows.Forms.Panel();
            this.panel_DataPanel = new System.Windows.Forms.Panel();
            this.flowLayoutPanel_Data = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox_Help = new System.Windows.Forms.GroupBox();
            this.webBrowser_Help = new System.Windows.Forms.WebBrowser();
            this.groupBox_Rating = new System.Windows.Forms.GroupBox();
            this.groupBox_Description = new System.Windows.Forms.GroupBox();
            this.growLabel_description = new GrowLabel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label_priority = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_catTitle = new System.Windows.Forms.Label();
            this.label_critTitle = new System.Windows.Forms.Label();
            this.panel_NavButtons = new System.Windows.Forms.Panel();
            this.button_Prev = new System.Windows.Forms.Button();
            this.button_Next = new System.Windows.Forms.Button();
            this.panel_CriteriaContent.SuspendLayout();
            this.panel_DataPanel.SuspendLayout();
            this.flowLayoutPanel_Data.SuspendLayout();
            this.groupBox_Help.SuspendLayout();
            this.groupBox_Description.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel_NavButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView_Steps
            // 
            this.treeView_Steps.AccessibleDescription = "Steps to go throu the guied dialog. The current step is marked";
            this.treeView_Steps.AccessibleName = "QS-step list view";
            this.treeView_Steps.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.treeView_Steps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView_Steps.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView_Steps.FullRowSelect = true;
            this.treeView_Steps.Location = new System.Drawing.Point(0, 0);
            this.treeView_Steps.MinimumSize = new System.Drawing.Size(150, 2);
            this.treeView_Steps.Name = "treeView_Steps";
            this.treeView_Steps.Size = new System.Drawing.Size(168, 570);
            this.treeView_Steps.TabIndex = 0;
            this.treeView_Steps.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_Steps_AfterSelect);
            // 
            // panel_CriteriaContent
            // 
            this.panel_CriteriaContent.AccessibleDescription = "contente panel for the criteria information";
            this.panel_CriteriaContent.AccessibleName = "criteria_panel";
            this.panel_CriteriaContent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel_CriteriaContent.Controls.Add(this.panel_DataPanel);
            this.panel_CriteriaContent.Controls.Add(this.panel4);
            this.panel_CriteriaContent.Controls.Add(this.panel_NavButtons);
            this.panel_CriteriaContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_CriteriaContent.Location = new System.Drawing.Point(168, 0);
            this.panel_CriteriaContent.Name = "panel_CriteriaContent";
            this.panel_CriteriaContent.Size = new System.Drawing.Size(593, 570);
            this.panel_CriteriaContent.TabIndex = 2;
            // 
            // panel_DataPanel
            // 
            this.panel_DataPanel.Controls.Add(this.flowLayoutPanel_Data);
            this.panel_DataPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_DataPanel.Location = new System.Drawing.Point(0, 70);
            this.panel_DataPanel.Name = "panel_DataPanel";
            this.panel_DataPanel.Size = new System.Drawing.Size(593, 441);
            this.panel_DataPanel.TabIndex = 22;
            // 
            // flowLayoutPanel_Data
            // 
            this.flowLayoutPanel_Data.AutoScroll = true;
            this.flowLayoutPanel_Data.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel_Data.Controls.Add(this.groupBox_Help);
            this.flowLayoutPanel_Data.Controls.Add(this.groupBox_Rating);
            this.flowLayoutPanel_Data.Controls.Add(this.groupBox_Description);
            this.flowLayoutPanel_Data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel_Data.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel_Data.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel_Data.Name = "flowLayoutPanel_Data";
            this.flowLayoutPanel_Data.Size = new System.Drawing.Size(593, 441);
            this.flowLayoutPanel_Data.TabIndex = 2;
            // 
            // groupBox_Help
            // 
            this.groupBox_Help.AccessibleDescription = "Detailed hints and tips";
            this.groupBox_Help.AccessibleName = "help-group";
            this.groupBox_Help.Controls.Add(this.webBrowser_Help);
            this.groupBox_Help.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Help.Location = new System.Drawing.Point(3, 3);
            this.groupBox_Help.MinimumSize = new System.Drawing.Size(420, 180);
            this.groupBox_Help.Name = "groupBox_Help";
            this.groupBox_Help.Padding = new System.Windows.Forms.Padding(7);
            this.groupBox_Help.Size = new System.Drawing.Size(420, 180);
            this.groupBox_Help.TabIndex = 13;
            this.groupBox_Help.TabStop = false;
            this.groupBox_Help.Text = "help";
            // 
            // webBrowser_Help
            // 
            this.webBrowser_Help.AccessibleDescription = "detailed help text or hints";
            this.webBrowser_Help.AccessibleName = "help-text";
            this.webBrowser_Help.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.webBrowser_Help.AllowWebBrowserDrop = false;
            this.webBrowser_Help.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser_Help.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser_Help.Location = new System.Drawing.Point(7, 20);
            this.webBrowser_Help.MinimumSize = new System.Drawing.Size(400, 150);
            this.webBrowser_Help.Name = "webBrowser_Help";
            this.webBrowser_Help.ScriptErrorsSuppressed = true;
            this.webBrowser_Help.Size = new System.Drawing.Size(406, 153);
            this.webBrowser_Help.TabIndex = 1;
            this.webBrowser_Help.WebBrowserShortcutsEnabled = false;
            // 
            // groupBox_Rating
            // 
            this.groupBox_Rating.AccessibleDescription = "rate the current criteria";
            this.groupBox_Rating.AccessibleName = "rating-group";
            this.groupBox_Rating.AutoSize = true;
            this.groupBox_Rating.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Rating.Location = new System.Drawing.Point(3, 189);
            this.groupBox_Rating.Name = "groupBox_Rating";
            this.groupBox_Rating.Padding = new System.Windows.Forms.Padding(7);
            this.groupBox_Rating.Size = new System.Drawing.Size(420, 13);
            this.groupBox_Rating.TabIndex = 14;
            this.groupBox_Rating.TabStop = false;
            this.groupBox_Rating.Text = "rating";
            // 
            // groupBox_Description
            // 
            this.groupBox_Description.AccessibleDescription = "Further description to the current category or criteria";
            this.groupBox_Description.AccessibleName = "description-group";
            this.groupBox_Description.AutoSize = true;
            this.groupBox_Description.Controls.Add(this.growLabel_description);
            this.groupBox_Description.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Description.Location = new System.Drawing.Point(3, 208);
            this.groupBox_Description.Name = "groupBox_Description";
            this.groupBox_Description.Padding = new System.Windows.Forms.Padding(7);
            this.groupBox_Description.Size = new System.Drawing.Size(420, 73);
            this.groupBox_Description.TabIndex = 12;
            this.groupBox_Description.TabStop = false;
            this.groupBox_Description.Text = "description";
            // 
            // growLabel_description
            // 
            this.growLabel_description.AccessibleDescription = "detailed description";
            this.growLabel_description.AccessibleName = "description";
            this.growLabel_description.AutoSize = true;
            this.growLabel_description.Location = new System.Drawing.Point(4, 40);
            this.growLabel_description.Name = "growLabel_description";
            this.growLabel_description.Size = new System.Drawing.Size(62, 13);
            this.growLabel_description.TabIndex = 1;
            this.growLabel_description.Text = "growLabel1";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.panel1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(593, 70);
            this.panel4.TabIndex = 21;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label_priority);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(491, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(102, 70);
            this.panel5.TabIndex = 24;
            // 
            // label_priority
            // 
            this.label_priority.AccessibleDescription = "priority of the current criteria";
            this.label_priority.Location = new System.Drawing.Point(11, 25);
            this.label_priority.Name = "label_priority";
            this.label_priority.Size = new System.Drawing.Size(88, 23);
            this.label_priority.TabIndex = 17;
            this.label_priority.Text = "Priority: A++";
            this.label_priority.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.label_catTitle);
            this.panel1.Controls.Add(this.label_critTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(593, 70);
            this.panel1.TabIndex = 23;
            // 
            // label_catTitle
            // 
            this.label_catTitle.AccessibleDescription = "Name of the current category";
            this.label_catTitle.AccessibleName = "category-title";
            this.label_catTitle.AutoSize = true;
            this.label_catTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_catTitle.Location = new System.Drawing.Point(3, 6);
            this.label_catTitle.MaximumSize = new System.Drawing.Size(340, 20);
            this.label_catTitle.Name = "label_catTitle";
            this.label_catTitle.Size = new System.Drawing.Size(166, 17);
            this.label_catTitle.TabIndex = 18;
            this.label_catTitle.Text = "Title of the Catoegory";
            // 
            // label_critTitle
            // 
            this.label_critTitle.AccessibleDescription = "Title of the current criteria";
            this.label_critTitle.AccessibleName = "criteria-title";
            this.label_critTitle.AutoSize = true;
            this.label_critTitle.Location = new System.Drawing.Point(3, 32);
            this.label_critTitle.MaximumSize = new System.Drawing.Size(400, 30);
            this.label_critTitle.Name = "label_critTitle";
            this.label_critTitle.Size = new System.Drawing.Size(92, 13);
            this.label_critTitle.TabIndex = 19;
            this.label_critTitle.Text = "Title of the Criteria";
            // 
            // panel_NavButtons
            // 
            this.panel_NavButtons.AccessibleDescription = "pannel containing navigation elements";
            this.panel_NavButtons.AccessibleName = "navigantion panel";
            this.panel_NavButtons.Controls.Add(this.button_Prev);
            this.panel_NavButtons.Controls.Add(this.button_Next);
            this.panel_NavButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_NavButtons.Location = new System.Drawing.Point(0, 511);
            this.panel_NavButtons.Name = "panel_NavButtons";
            this.panel_NavButtons.Size = new System.Drawing.Size(593, 59);
            this.panel_NavButtons.TabIndex = 20;
            // 
            // button_Prev
            // 
            this.button_Prev.AccessibleDescription = "go to previous step";
            this.button_Prev.AccessibleName = "previous-button";
            this.button_Prev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_Prev.Location = new System.Drawing.Point(6, 18);
            this.button_Prev.Name = "button_Prev";
            this.button_Prev.Size = new System.Drawing.Size(96, 25);
            this.button_Prev.TabIndex = 17;
            this.button_Prev.Text = "Previous";
            this.button_Prev.UseVisualStyleBackColor = true;
            this.button_Prev.Click += new System.EventHandler(this.button_Prev_Click);
            // 
            // button_Next
            // 
            this.button_Next.AccessibleDescription = "go to next step";
            this.button_Next.AccessibleName = "next-button";
            this.button_Next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Next.Location = new System.Drawing.Point(491, 18);
            this.button_Next.Name = "button_Next";
            this.button_Next.Size = new System.Drawing.Size(96, 25);
            this.button_Next.TabIndex = 18;
            this.button_Next.Text = "Next";
            this.button_Next.UseVisualStyleBackColor = true;
            this.button_Next.Click += new System.EventHandler(this.button_Next_Click);
            // 
            // QsDialog
            // 
            this.AccessibleDescription = "Dialog for a guied quality checking";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 570);
            this.Controls.Add(this.panel_CriteriaContent);
            this.Controls.Add(this.treeView_Steps);
            this.Name = "QsDialog";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.QsDialog_Load);
            this.panel_CriteriaContent.ResumeLayout(false);
            this.panel_DataPanel.ResumeLayout(false);
            this.flowLayoutPanel_Data.ResumeLayout(false);
            this.flowLayoutPanel_Data.PerformLayout();
            this.groupBox_Help.ResumeLayout(false);
            this.groupBox_Description.ResumeLayout(false);
            this.groupBox_Description.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel_NavButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView_Steps;
        private System.Windows.Forms.Panel panel_CriteriaContent;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label_priority;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_catTitle;
        private System.Windows.Forms.Label label_critTitle;
        private System.Windows.Forms.Panel panel_NavButtons;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_Data;
        private System.Windows.Forms.GroupBox groupBox_Description;
        private System.Windows.Forms.GroupBox groupBox_Help;
        private System.Windows.Forms.WebBrowser webBrowser_Help;
        private System.Windows.Forms.GroupBox groupBox_Rating;
        private System.Windows.Forms.Button button_Prev;
        private System.Windows.Forms.Button button_Next;
        private System.Windows.Forms.Panel panel_DataPanel;
        private GrowLabel growLabel_description;
    }
}