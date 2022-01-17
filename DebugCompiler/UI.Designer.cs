
namespace DebugCompiler
{
    partial class UI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI));
            this.ZIPSelector = new System.Windows.Forms.OpenFileDialog();
            this.LoadZip = new System.Windows.Forms.Button();
            this.CurrentMenu = new System.Windows.Forms.Label();
            this.InjectButton = new System.Windows.Forms.Button();
            this.FolderLoad = new System.Windows.Forms.Button();
            this.Help = new System.Windows.Forms.HelpProvider();
            this.GmBox = new System.Windows.Forms.ComboBox();
            this.resetGsc = new System.Windows.Forms.Button();
            this.HelpButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ZIPSelector
            // 
            this.ZIPSelector.FileName = "selectZIP";
            // 
            // LoadZip
            // 
            this.LoadZip.Location = new System.Drawing.Point(12, 12);
            this.LoadZip.Name = "LoadZip";
            this.LoadZip.Size = new System.Drawing.Size(109, 23);
            this.LoadZip.TabIndex = 0;
            this.LoadZip.Text = "Load Zip File";
            this.LoadZip.UseVisualStyleBackColor = true;
            this.LoadZip.Click += new System.EventHandler(this.LoadZip_Click);
            // 
            // CurrentMenu
            // 
            this.CurrentMenu.AutoSize = true;
            this.CurrentMenu.BackColor = System.Drawing.Color.Transparent;
            this.CurrentMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentMenu.ForeColor = System.Drawing.Color.White;
            this.CurrentMenu.Location = new System.Drawing.Point(0, 428);
            this.CurrentMenu.Name = "CurrentMenu";
            this.CurrentMenu.Size = new System.Drawing.Size(105, 20);
            this.CurrentMenu.TabIndex = 1;
            this.CurrentMenu.Text = "Selected File:";
            // 
            // InjectButton
            // 
            this.InjectButton.Location = new System.Drawing.Point(679, 415);
            this.InjectButton.Name = "InjectButton";
            this.InjectButton.Size = new System.Drawing.Size(109, 23);
            this.InjectButton.TabIndex = 2;
            this.InjectButton.Text = "Inject";
            this.InjectButton.UseVisualStyleBackColor = true;
            this.InjectButton.Click += new System.EventHandler(this.InjectButton_Click);
            // 
            // FolderLoad
            // 
            this.FolderLoad.Enabled = false;
            this.FolderLoad.Location = new System.Drawing.Point(4, 402);
            this.FolderLoad.Name = "FolderLoad";
            this.FolderLoad.Size = new System.Drawing.Size(109, 23);
            this.FolderLoad.TabIndex = 3;
            this.FolderLoad.Text = "Load From Folder";
            this.FolderLoad.UseVisualStyleBackColor = true;
            this.FolderLoad.Visible = false;
            this.FolderLoad.Click += new System.EventHandler(this.FolderLoad_Click);
            // 
            // GmBox
            // 
            this.GmBox.FormattingEnabled = true;
            this.GmBox.Items.AddRange(new object[] {
            "Multiplayer",
            "Zombies"});
            this.GmBox.Location = new System.Drawing.Point(553, 417);
            this.GmBox.Name = "GmBox";
            this.GmBox.Size = new System.Drawing.Size(121, 21);
            this.GmBox.TabIndex = 4;
            this.GmBox.Text = "Gamemode";
            this.GmBox.Visible = false;
            this.GmBox.SelectedIndexChanged += new System.EventHandler(this.GmBox_SelectedIndexChanged);
            // 
            // resetGsc
            // 
            this.resetGsc.Location = new System.Drawing.Point(680, 390);
            this.resetGsc.Name = "resetGsc";
            this.resetGsc.Size = new System.Drawing.Size(109, 23);
            this.resetGsc.TabIndex = 5;
            this.resetGsc.Text = "Reset GSC";
            this.resetGsc.UseVisualStyleBackColor = true;
            this.resetGsc.Click += new System.EventHandler(this.resetGsc_Click);
            // 
            // HelpButton
            // 
            this.HelpButton.Location = new System.Drawing.Point(126, 11);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(44, 25);
            this.HelpButton.TabIndex = 6;
            this.HelpButton.Text = "Help?";
            this.HelpButton.UseVisualStyleBackColor = true;
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.HelpButton);
            this.Controls.Add(this.resetGsc);
            this.Controls.Add(this.GmBox);
            this.Controls.Add(this.FolderLoad);
            this.Controls.Add(this.InjectButton);
            this.Controls.Add(this.CurrentMenu);
            this.Controls.Add(this.LoadZip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "UI";
            this.Text = "UI";
            this.Load += new System.EventHandler(this.UI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ZIPSelector;
        private System.Windows.Forms.Button LoadZip;
        private System.Windows.Forms.Label CurrentMenu;
        private System.Windows.Forms.Button InjectButton;
        private System.Windows.Forms.Button FolderLoad;
        private System.Windows.Forms.HelpProvider Help;
        private System.Windows.Forms.ComboBox GmBox;
        private System.Windows.Forms.Button resetGsc;
        private System.Windows.Forms.Button HelpButton;
    }
}