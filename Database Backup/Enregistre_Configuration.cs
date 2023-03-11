using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Database_Backup
{
    public partial class Enregistre_Configuration : Form
    {
        public enum mode
        {
            Serveur,
            Table
        }

        public String Server
        {
            get { return textBox_ServerName.Text; }
        }
        public String Host
        {
            get { return grpBox_saisie.Host; }
        }
        public String Port
        {
            get { return grpBox_saisie.Port; }
        }
        public String Database
        {
            get { return grpBox_saisie.Database; }
        }
        public String Username
        {
            get { return grpBox_saisie.Username; }
        }
        public String Password
        {
            get { return grpBox_saisie.Password; }
        }

        private Imput_Params grpBox_saisie;

        public Enregistre_Configuration(mode Mode, string host, string port, string database, string user, string password)
        {
            InitializeComponent();

            grpBox_saisie = new Imput_Params(Imput_Params.mode.Serveur);
            grpBox_saisie.load_backup_tab(new Dictionary<string, string>() {
                { "Host", host },
                { "Port", port },
                { "NameBase", database },
                { "User", user },
                { "PassWord", password }
            });
            switch (Mode)
            {
                case mode.Serveur:
                    {
                        this.Text = "Enregistrement d'un serveur";
                        grpBox_saisie.Location = new Point(3, 55);
                        this.Controls.Add(grpBox_saisie);
                        //this.Size = new Size(312, 218);
                        break;
                    }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
