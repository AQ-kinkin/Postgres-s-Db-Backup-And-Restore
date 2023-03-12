using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Database_Backup
{
    public partial class Imput_Params : UserControl
    {
        public String Host
        {
            get { return txtHost.Text; }
        }
        public String Port
        {
            get { return txtPort.Text; }
        }
        public String Database
        {
            get { return txtDatabase.Text; }
        }
        public String Username
        {
            get { return txtUsername.Text; }
        }
        public String Password
        {
            get { return txtPassword.Text; }
        }
        public String Table
        {
            get { return txtTable.Text; }
        }
        public Boolean ConfSelected { get; set; }
        private Configuration.typeConf _mode { get; set; }

        public Imput_Params(Configuration.typeConf modeName)
        {
            InitializeComponent();

            _mode = modeName;
            changeMode(modeName);
        }
        public void Init_Champs(Dictionary<string, string> Param, Boolean Selected)
        {
            if (Param == null) throw new ArgumentNullException();

            if (Param.ContainsKey("Host")) txtHost.Text = Param["Host"];
            if (Param.ContainsKey("Port")) txtPort.Text = Param["Port"];
            if (Param.ContainsKey("NameBase")) txtDatabase.Text = Param["NameBase"];
            if (Param.ContainsKey("User")) txtUsername.Text = Param["User"];
            if (Param.ContainsKey("PassWord")) txtPassword.Text = Param["PassWord"];
            if (_mode == Configuration.typeConf.Tables)
            {
                if (Param.ContainsKey("Table")) txtTable.Text = Param["Table"];
            }

            ConfSelected = Selected;
        }

        public void changeMode(Configuration.typeConf modeName)
        {
            switch (modeName)
            {
                case Configuration.typeConf.Servers:
                    {
                        this.Size = new Size(312, 218);
                        break;
                    }
                case Configuration.typeConf.Tables:
                    {
                        this.Size = new Size(312, 256);
                        break;
                    }
            }
            _mode = modeName;
        }

        public void changeMode(Configuration.typeConf modeName, Dictionary<string, string> Param)
        {
            changeMode(modeName);
            if (Param == null) return;
            Init_Champs(Param, false);
            /*switch (modeName)
            {
                case mode.Serveur:
                    {
                        if (Param.Count != 5) throw new ArgumentException("Erreur dans le nombre d'argument. 5 sont attendu ( Host, Port, NameBase, User, PassWord ). ils peuvent être vide");
                        break;
                    }
                case mode.Table:
                    {
                        if (Param.Count != 6) throw new ArgumentException("Erreur dans le nombre d'argument. 6 sont attendu ( Host, Port, NameBase, User, PassWord, Table ). ils peuvent être vide");
                        break;
                    }
            }*/

        }

        private void champstxt_TextChanged(object sender, EventArgs e)
        {
            ConfSelected = false;
        }
    }

}
