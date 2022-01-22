
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
            this.CurrentMenu = new System.Windows.Forms.Label();
            this.Help = new System.Windows.Forms.HelpProvider();
            this.Compile = new System.Windows.Forms.Button();
            this.FileOpen = new System.Windows.Forms.Button();
            this.HelpBut = new System.Windows.Forms.Button();
            this.Reset = new System.Windows.Forms.Button();
            this.Zombies = new System.Windows.Forms.Button();
            this.MultiPlayer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Folder = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CheckUpdate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ZIPSelector
            // 
            this.ZIPSelector.FileName = "selectZIP";
            // 
            // CurrentMenu
            // 
            this.CurrentMenu.AutoSize = true;
            this.CurrentMenu.BackColor = System.Drawing.Color.Transparent;
            this.CurrentMenu.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentMenu.ForeColor = System.Drawing.Color.DodgerBlue;
            this.CurrentMenu.Location = new System.Drawing.Point(104, 548);
            this.CurrentMenu.Name = "CurrentMenu";
            this.CurrentMenu.Size = new System.Drawing.Size(0, 20);
            this.CurrentMenu.TabIndex = 1;
            this.CurrentMenu.Click += new System.EventHandler(this.CurrentMenu_Click);
            // 
            // Compile
            // 
            this.Compile.BackColor = System.Drawing.Color.Transparent;
            this.Compile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Compile.BackgroundImage")));
            this.Compile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Compile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Compile.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.Compile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.Compile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Compile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Compile.Location = new System.Drawing.Point(-1, 490);
            this.Compile.Name = "Compile";
            this.Compile.Size = new System.Drawing.Size(99, 87);
            this.Compile.TabIndex = 14;
            this.Compile.UseVisualStyleBackColor = false;
            this.Compile.Click += new System.EventHandler(this.Compile_Click);
            // 
            // FileOpen
            // 
            this.FileOpen.BackColor = System.Drawing.Color.Transparent;
            this.FileOpen.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("FileOpen.BackgroundImage")));
            this.FileOpen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.FileOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FileOpen.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.FileOpen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.FileOpen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.FileOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FileOpen.Location = new System.Drawing.Point(-1, 304);
            this.FileOpen.Name = "FileOpen";
            this.FileOpen.Size = new System.Drawing.Size(99, 87);
            this.FileOpen.TabIndex = 15;
            this.FileOpen.UseVisualStyleBackColor = false;
            this.FileOpen.Click += new System.EventHandler(this.FileOpen_Click);
            // 
            // HelpBut
            // 
            this.HelpBut.BackColor = System.Drawing.Color.Transparent;
            this.HelpBut.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("HelpBut.BackgroundImage")));
            this.HelpBut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.HelpBut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HelpBut.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.HelpBut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.HelpBut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.HelpBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HelpBut.Location = new System.Drawing.Point(0, 0);
            this.HelpBut.Name = "HelpBut";
            this.HelpBut.Size = new System.Drawing.Size(99, 87);
            this.HelpBut.TabIndex = 16;
            this.HelpBut.UseVisualStyleBackColor = false;
            this.HelpBut.Click += new System.EventHandler(this.HelpBut_Click);
            // 
            // Reset
            // 
            this.Reset.BackColor = System.Drawing.Color.Transparent;
            this.Reset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Reset.BackgroundImage")));
            this.Reset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Reset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Reset.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.Reset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.Reset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Reset.Location = new System.Drawing.Point(0, 397);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(99, 87);
            this.Reset.TabIndex = 17;
            this.Reset.UseVisualStyleBackColor = false;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // Zombies
            // 
            this.Zombies.BackColor = System.Drawing.Color.Transparent;
            this.Zombies.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Zombies.BackgroundImage")));
            this.Zombies.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Zombies.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Zombies.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.Zombies.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.Zombies.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Zombies.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Zombies.Location = new System.Drawing.Point(936, 490);
            this.Zombies.Name = "Zombies";
            this.Zombies.Size = new System.Drawing.Size(99, 87);
            this.Zombies.TabIndex = 18;
            this.Zombies.UseVisualStyleBackColor = false;
            this.Zombies.Click += new System.EventHandler(this.Zombies_Click);
            // 
            // MultiPlayer
            // 
            this.MultiPlayer.BackColor = System.Drawing.Color.Transparent;
            this.MultiPlayer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("MultiPlayer.BackgroundImage")));
            this.MultiPlayer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.MultiPlayer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MultiPlayer.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.MultiPlayer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.MultiPlayer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MultiPlayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MultiPlayer.Location = new System.Drawing.Point(840, 490);
            this.MultiPlayer.Name = "MultiPlayer";
            this.MultiPlayer.Size = new System.Drawing.Size(99, 87);
            this.MultiPlayer.TabIndex = 19;
            this.MultiPlayer.UseVisualStyleBackColor = false;
            this.MultiPlayer.Click += new System.EventHandler(this.MultiPlayer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 561);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 16);
            this.label1.TabIndex = 22;
            this.label1.Text = "Compile/Inject";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(13, 478);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 16);
            this.label2.TabIndex = 23;
            this.label2.Text = "Reset Gsc";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(16, 378);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 16);
            this.label3.TabIndex = 24;
            this.label3.Text = "Select Zip";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(33, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 16);
            this.label4.TabIndex = 25;
            this.label4.Text = "Help";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(-23, -46);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Folder
            // 
            this.Folder.BackColor = System.Drawing.Color.Transparent;
            this.Folder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Folder.BackgroundImage")));
            this.Folder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Folder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Folder.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.Folder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.Folder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Folder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Folder.Location = new System.Drawing.Point(0, 224);
            this.Folder.Name = "Folder";
            this.Folder.Size = new System.Drawing.Size(99, 87);
            this.Folder.TabIndex = 27;
            this.Folder.UseVisualStyleBackColor = false;
            this.Folder.Click += new System.EventHandler(this.Folder_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(10, 295);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(88, 16);
            this.label5.TabIndex = 28;
            this.label5.Text = "Select Folder";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(910, 3);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(124, 16);
            this.label6.TabIndex = 29;
            this.label6.Text = "Check For Updates";
            // 
            // CheckUpdate
            // 
            this.CheckUpdate.BackColor = System.Drawing.Color.Transparent;
            this.CheckUpdate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CheckUpdate.BackgroundImage")));
            this.CheckUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CheckUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CheckUpdate.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.CheckUpdate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.CheckUpdate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CheckUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckUpdate.Location = new System.Drawing.Point(921, 22);
            this.CheckUpdate.Name = "CheckUpdate";
            this.CheckUpdate.Size = new System.Drawing.Size(99, 87);
            this.CheckUpdate.TabIndex = 30;
            this.CheckUpdate.UseVisualStyleBackColor = false;
            this.CheckUpdate.Click += new System.EventHandler(this.CheckUpdate_Click);
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(1032, 577);
            this.Controls.Add(this.CheckUpdate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Folder);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MultiPlayer);
            this.Controls.Add(this.Zombies);
            this.Controls.Add(this.Reset);
            this.Controls.Add(this.HelpBut);
            this.Controls.Add(this.FileOpen);
            this.Controls.Add(this.Compile);
            this.Controls.Add(this.CurrentMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "UI";
            this.Text = "UI";
            this.Load += new System.EventHandler(this.UI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ZIPSelector;
        private System.Windows.Forms.Label CurrentMenu;
        private System.Windows.Forms.HelpProvider Help;
        private System.Windows.Forms.Button Compile;
        private System.Windows.Forms.Button FileOpen;
        private System.Windows.Forms.Button HelpBut;
        private System.Windows.Forms.Button Reset;
        private System.Windows.Forms.Button Zombies;
        private System.Windows.Forms.Button MultiPlayer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button Folder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button CheckUpdate;
    }
}