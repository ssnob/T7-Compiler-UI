using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DebugCompiler
{
    public partial class ErrorForm : Form
    {
        public string linktoopen = String.Empty;
        public ErrorForm()
        {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            clearReason();
            this.Close();
        }

        public void openme(string context)
        {
            ReasonLine.Text = context;
            this.Show();
        }

        public void setlinks(string links)
        {
            LinkLabel.Text = links;
            linktoopen = links;
            LinkBox.Show();
        }

        public void clearReason()
        {
            ReasonLine.Text = String.Empty;
            LinkLabel.Text = String.Empty;
        }

        public void setTitle(string context)
        {
            this.Text = context;
        }

        public void hidelink()
        {
            LinkBox.Hide();
        }

        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linktoopen);
        }
    }
}
