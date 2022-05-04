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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        void checkFile()
        {
            var newFile = File.Create(@"users.json");
            newFile.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(@"users.json"))
            {
                Users.Root root = new();
                Users.UserList ul = new();
                Users.UserInfo ui = new();
                ui.totalUsers = 1;
                ul.id = 1;
                ul.name = textBox1.Text;
                ul.password = textBox2.Text;

                ui.userList.Add(ui);
            }
        }
    }
}
