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
        public List<PassInfo> currList = new List<PassInfo>();
        bool passwordsShown = false;
        public Form1()
        {
            CheckCSVFile();
            InitializeComponent();
            LoadInfo(passList);
            currList = passList;
        }


        public void LoadInfo(List<PassInfo> plist)
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("name - login - password - link - comment");
            foreach (PassInfo pass in plist)
            {

                listBox1.Items.Add(pass);
            }
        }

        void CheckCSVFile()
        {
            if (!File.Exists(@"passwords.enc"))
            {
                if (!File.Exists(@"passwords.csv"))
                {
                    var myFile = File.Create(@"passwords.csv");
                    myFile.Close();

                }
                AESHelper.AES_EncryptFile(@"passwords.csv", @"passwords.enc");
                File.Delete(@"passwords.aes");

            }
            else
            {

                AESHelper.AES_DecryptFile(@"passwords.enc", @"passwords.csv");
                File.Delete(@"passwords.enc");
                if (new FileInfo(@"passwords.csv").Length != 0)
                {
                    using (var reader = new StreamReader(@"passwords.csv"))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        while (csv.Read())
                            passList.Add(csv.GetRecord<PassInfo>());
                    }
                }
            }
        }
        public void onAppExit()
        {
            if (passwordsShown)
            {
                foreach (PassInfo pass in currList)
                {
                    pass.password = AESHelper.AES_EncryptString(pass.password);
                }
            }
            using (var writer = new StreamWriter("passwords.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(passList);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*onAppExit();
            AESHelper.AES_EncryptFile(@"passwords.csv", @"passwords.enc");
            File.Delete(@"passwords.csv");*/
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
                if (Convert.ToInt32(index) != 0)
                {
                    string pwd;
                    //MessageBox.Show(index.ToString());
                    if (passwordsShown)
                    {
                        pwd = passList[Convert.ToInt32(index - 1)].password;
                    }
                    else
                    {
                        pwd = AESHelper.AES_DecryptString(passList[Convert.ToInt32(index - 1)].password);
                    }
                    Clipboard.SetText(pwd);
                    MessageBox.Show(pwd);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var a = passList.Where(x => x.name.Contains(textBox1.Text)).ToList();
            // MessageBox.Show(a.ToString());
            if (a.Count() != 0)
            {
                LoadInfo(a);
                currList = a;
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadInfo(passList);
            currList = passList;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdatePassword up = new UpdatePassword(passList, this);
            up.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!passwordsShown)
            {
                List<PassInfo> a = new(currList.ToList());
                foreach (var item in a)
                {
                    item.password = AESHelper.AES_DecryptString(item.password);
                }
                LoadInfo(a);
                passwordsShown = true;
            }

            else
            {
                List<PassInfo> a = new(currList.ToList());
                foreach (var item in a)
                {
                    item.password = AESHelper.AES_EncryptString(item.password);
                }
                LoadInfo(passList);
                passwordsShown = false;
                

            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            onAppExit();
            AESHelper.AES_EncryptFile(@"passwords.csv", @"passwords.enc");
            File.Delete(@"passwords.csv");
        }
    }
}



/* TODO
 Paleidus sistemą pirmą kartą sukuriamas .csv arba .txt failas. Išjungiant sistemą šis failas turi būti užšifruojamas AES algoritmu. Kitą kartą paleidus sistemą failas yra dešifruojamas. (4 taškai) done
Naujo slaptažodžio išsaugojimas: užpildžius formą (pavadinimas, slaptažodis, URL/aplikacija, komentaras), visa jos informacija saugojama .csv arba .txt faile. Slaptažodžiui pritaikomas šifravimo algoritmas (pvz.: AES, DES ar RSA. Renkatės savo nuožiūra). (3 taškai) DONE
Slaptažodžio paieška pagal pavadinimą. (2 taškai) DONE
Slaptažodžio atnaujinimas pagal pavadinimą: suradus tinkamą slaptažodį jis pakeičiamas naujai įvestu. Naujam slaptažodžiui taip pat turi būti pritaikytas šifravimo algoritmas. (2 taškai) DONE
Slaptažodžio ištrynimas pagal pavadinimą: suradus tinkamą slaptažodį visa informacija apie jį ištrinama iš .csv arba .txt failo. (2 taškai)
Paleidus sistemą pirmą kartą reikalinga vartotojo paskyros sukūrimo forma: slapyvardis, slaptažodis (šifruojamas PBKDF2, Bcrypt, Scrypt, Argon2 arba pasirenkant maišos funkciją). Kuriant vartotojo paskyrą yra sugeneruojamas ir vartotojui priskiriamas .csv arba .txt failas. Failas yra užšifruojamas AES algoritmu. (3 taškai) Partially done
Prisijungimas prie sistemos: vartotojui prijungus failas dešifruojamas. (3 taškai)
Atsitiktinio slaptažodžio generavimo funkcija (panaudojama kuriant naują slaptažodį). (2 taškai)
Papildoma funkcija slaptažodžio paieškai pagal pavadinimą: suradus tinkamą slaptažodį jis iškart nerodomas, pateikiamas tik jo užšifruotas rezultatas. Paspaudus mygtuką rodyti parodomas slaptažodis. (2 taškai) DONE
Mygtukas galintis nukopijuoti slaptažodį į iškarpinę. (2 taškai) DONE 
*/
