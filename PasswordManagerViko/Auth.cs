using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PasswordManagerViko
{
    public partial class Auth : Form
    {
        public Auth()
        {
            InitializeComponent();
        }

        void CheckFile()
        {
            if (!File.Exists(@"users.json"))
            {
               
                Register rf = new();
                rf.Show();
                this.Hide();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register rForm = new();
            rForm.Show();
            

        }
    }
}
