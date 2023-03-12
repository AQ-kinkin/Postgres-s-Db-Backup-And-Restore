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
        public String NomConf
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
        public String Table
        {
            get { return grpBox_saisie.Table; }
        }

        private Imput_Params grpBox_saisie;

        public Enregistre_Configuration(Configuration.typeConf Mode, Dictionary<string, string> datas)
        {
            InitializeComponent();

            grpBox_saisie = new Imput_Params(Mode);

            grpBox_saisie.Init_Champs(datas, false);
            switch (Mode)
            {
                case Configuration.typeConf.Servers:
                    {
                        this.Text = "Enregistrement d'un serveur";
                        grpBox_saisie.Location = new Point(3, 55);
                        this.Controls.Add(grpBox_saisie);
                        this.Size = new Size(342, 350);
                        break;
                    }
                case Configuration.typeConf.Tables:
                    {
                        this.Text = "Enregistrement d'une table d'une BDD";
                        grpBox_saisie.Location = new Point(3, 55);
                        this.Controls.Add(grpBox_saisie);
                        this.Size = new Size(342, 380);
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
