namespace DebugCompiler
{
    partial class ErrorForm
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
            this.AcceptButton = new System.Windows.Forms.Button();
            this.ReasonBox = new System.Windows.Forms.GroupBox();
            this.ReasonLine = new System.Windows.Forms.Label();
            this.LinkBox = new System.Windows.Forms.GroupBox();
            this.LinkLabel = new System.Windows.Forms.LinkLabel();
            this.ReasonBox.SuspendLayout();
            this.LinkBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // AcceptButton
            // 
            this.AcceptButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.AcceptButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AcceptButton.FlatAppearance.BorderColor = System.Drawing.Color.DodgerBlue;
            this.AcceptButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AcceptButton.ForeColor = System.Drawing.Color.White;
            this.AcceptButton.Location = new System.Drawing.Point(116, 259);
            this.AcceptButton.Name = "AcceptButton";
            this.AcceptButton.Size = new System.Drawing.Size(102, 43);
            this.AcceptButton.TabIndex = 0;
            this.AcceptButton.Text = "Accept";
            this.AcceptButton.UseVisualStyleBackColor = false;
            this.AcceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // ReasonBox
            // 
            this.ReasonBox.Controls.Add(this.ReasonLine);
            this.ReasonBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ReasonBox.ForeColor = System.Drawing.Color.White;
            this.ReasonBox.Location = new System.Drawing.Point(12, 12);
            this.ReasonBox.Name = "ReasonBox";
            this.ReasonBox.Size = new System.Drawing.Size(308, 189);
            this.ReasonBox.TabIndex = 1;
            this.ReasonBox.TabStop = false;
            this.ReasonBox.Text = "Info";
            // 
            // ReasonLine
            // 
            this.ReasonLine.AutoSize = true;
            this.ReasonLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReasonLine.Location = new System.Drawing.Point(6, 16);
            this.ReasonLine.Name = "ReasonLine";
            this.ReasonLine.Size = new System.Drawing.Size(65, 20);
            this.ReasonLine.TabIndex = 0;
            this.ReasonLine.Text = "Reason";
            // 
            // LinkBox
            // 
            this.LinkBox.Controls.Add(this.LinkLabel);
            this.LinkBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LinkBox.ForeColor = System.Drawing.Color.White;
            this.LinkBox.Location = new System.Drawing.Point(12, 207);
            this.LinkBox.Name = "LinkBox";
            this.LinkBox.Size = new System.Drawing.Size(308, 46);
            this.LinkBox.TabIndex = 2;
            this.LinkBox.TabStop = false;
            this.LinkBox.Text = "Links";
            // 
            // LinkLabel
            // 
            this.LinkLabel.ActiveLinkColor = System.Drawing.Color.Navy;
            this.LinkLabel.AutoSize = true;
            this.LinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLabel.Location = new System.Drawing.Point(6, 16);
            this.LinkLabel.Name = "LinkLabel";
            this.LinkLabel.Size = new System.Drawing.Size(32, 16);
            this.LinkLabel.TabIndex = 0;
            this.LinkLabel.TabStop = true;
            this.LinkLabel.Text = "Link";
            this.LinkLabel.VisitedLinkColor = System.Drawing.Color.Blue;
            this.LinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
            // 
            // ErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(332, 314);
            this.ControlBox = false;
            this.Controls.Add(this.LinkBox);
            this.Controls.Add(this.ReasonBox);
            this.Controls.Add(this.AcceptButton);
            this.Name = "ErrorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error";
            this.ReasonBox.ResumeLayout(false);
            this.ReasonBox.PerformLayout();
            this.LinkBox.ResumeLayout(false);
            this.LinkBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AcceptButton;
        private System.Windows.Forms.GroupBox ReasonBox;
        private System.Windows.Forms.Label ReasonLine;
        private System.Windows.Forms.GroupBox LinkBox;
        private System.Windows.Forms.LinkLabel LinkLabel;
    }
}