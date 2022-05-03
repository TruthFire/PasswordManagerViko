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
using CsvHelper;
using System.Globalization;

namespace PasswordManagerViko
{
    public partial class Form1 : Form
    {
        public List<PassInfo> passList = new List<PassInfo>();
        public Form1()
        {
            CheckCSVFile();
            InitializeComponent();
            LoadInfo();
        }


        void LoadInfo()
        {
            foreach (PassInfo pass in passList)
            {
                listBox1.Items.Add(pass);
            }
        }

        void CheckCSVFile()
        {
            if (!File.Exists(@"passwords.enc"))
            {
                if(!File.Exists(@"passwords.csv"))
                {
                    var myFile = File.Create(@"passwords.csv");
                    myFile.Close();

                }
                AESHelper.AES_Encrypt(@"passwords.csv", @"passwords.enc");
                File.Delete(@"passwords.aes");

            }
            else
            {
                
                AESHelper.AES_Decrypt(@"passwords.enc", @"passwords.csv");
                File.Delete(@"passwords.enc");
                if (new FileInfo(@"passwords.csv").Length != 0)
                {
                    using(var reader = new StreamReader(@"passwords.csv"))
                    using(var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        while(csv.Read())
                         passList.Add(csv.GetRecord<PassInfo>());                        
                    }
                }
            }
        }
        public void onAppExit()
        {
            using (var writer = new StreamWriter("passwords.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(passList);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            onAppExit();
            AESHelper.AES_Encrypt(@"passwords.csv", @"passwords.enc");
            File.Delete(@"passwords.csv");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddAccount ac = new(this);
            ac.Show();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox1.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
               // MessageBox.Show(index.ToString());

                Clipboard.SetText(passList[Convert.ToInt32(index)].password);
                MessageBox.Show(passList[Convert.ToInt32(index)].password);
            }
        }
    }
}
