using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordManagerViko
{
    public partial class AddAccount : Form
    {
        Form1 cForm;
        public AddAccount(Form1 currForm)
        {
            cForm = currForm;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pwd = AESHelper.AES_EncryptString(textBox3.Text);
            cForm.passList.Add(new PassInfo(textBox1.Text, textBox2.Text, pwd, textBox4.Text, textBox5.Text));
            cForm.LoadInfo(cForm.passList);
            MessageBox.Show("OK");
        }
    }
}
