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
    public partial class Imput_Params : UserControl
    {
        public enum mode
        {
            Serveur,
            Table
        }
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

        public Imput_Params(mode modeName)
        {
            InitializeComponent();
            switch(modeName)
            {
                case mode.Serveur:
                {
                    this.Size = new Size(312, 218);
                    break;
                }
                case mode.Table:
                {
                    this.Size = new Size(312, 261);
                    break;
                }
            }
                    
        }
        public void load_backup_tab(Dictionary<string, string> Param)
        {
            txtHost.Text = Param["Host"];
            txtPort.Text = Param["Port"];
            txtDatabase.Text = Param["NameBase"];
            txtUsername.Text = Param["User"];
            txtPassword.Text = Param["PassWord"];
        }
    }

}
