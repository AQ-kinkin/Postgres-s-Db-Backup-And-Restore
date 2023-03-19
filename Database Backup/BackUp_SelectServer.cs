using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using postgres_backups_solutions;
using static System.Windows.Forms.Design.AxImporter;

namespace Database_Backup
{
    public partial class BackUp_SelectServer : UserControl
    {
        Imput_Params grpBox_saisie;
        Configuration.typeConf _mode;
        Boolean ConfSelected { get { return grpBox_saisie.ConfSelected; } }
        public Configuration.typeConf Mode
        {
            get { return _mode; }
            set { changeMode(value); }
        }

        public BackUp_SelectServer(Configuration.typeConf mode)
        {
            InitializeComponent();

            _mode = mode;
            grpBox_saisie = new Imput_Params(mode);
            grpBox_saisie.Location = new Point(16, 45);
            grpBox_saisie.Init_Champs(Program.TheConfiguration.load_lastsaisie(mode), false);
            this.Controls.Add(grpBox_saisie);

            (this.Controls["comboBox1"] as ComboBox).Items.AddRange(Program.TheConfiguration.ListServer[mode]);
        }

        private void changeMode(Configuration.typeConf value)
        {
            grpBox_saisie.changeMode(value);
            grpBox_saisie.Init_Champs(Program.TheConfiguration.load_lastsaisie(value), false);
            (this.Controls["comboBox1"] as ComboBox).Items.Clear();
            (this.Controls["comboBox1"] as ComboBox).Items.AddRange(Program.TheConfiguration.ListServer[value]);
/*            switch (value)
            {
                case Configuration.typeConf.Servers:
                {
                    break;
                }
                case Configuration.typeConf.Tables:
                {
                    break;
                }
            }*/
            _mode = value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                call_postgre_backup dllClass = new call_postgre_backup();
                var save = new SaveFileDialog();
                save.Filter = "Fichier dump | *.dump";
                save.Title = "Enregistrer Sous";
                Dictionary<String, String> data = Configuration.Create_Dic_params(_mode, grpBox_saisie.Host, grpBox_saisie.Port, grpBox_saisie.Database, grpBox_saisie.Username, grpBox_saisie.Password, grpBox_saisie.Table);
                
                if (save.ShowDialog() == DialogResult.OK)
                {
                    if (!ConfSelected)
                    {
                        if (MessageBox.Show("Voulez-vous sauvegarder la configuration ?", "Gagnez du temps", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Enregistre_Configuration EnregistreConfiguration = new Enregistre_Configuration(_mode, data);
                            if (EnregistreConfiguration.ShowDialog() == DialogResult.OK)
                            { Program.TheConfiguration.save_conf(_mode, EnregistreConfiguration.NomConf, Configuration.Create_Dic_params(_mode, grpBox_saisie.Host, grpBox_saisie.Port, grpBox_saisie.Database, grpBox_saisie.Username, grpBox_saisie.Password, EnregistreConfiguration.Table)); }
                            EnregistreConfiguration.Close();
                        }
                    }
                    switch (_mode)
                    {
                        case Configuration.typeConf.Servers:
                            {
                                string[] info_connexion = new string[] { grpBox_saisie.Host, grpBox_saisie.Port, grpBox_saisie.Username, grpBox_saisie.Password };
                                string[] Options = new string[] { "-Fc" };
                                dllClass.dump_base(Program.TheConfiguration.PathBinsPg + @"\pg_dump.exe", info_connexion, save.FileName, grpBox_saisie.Database, Options);
                                break;
                            }
                        case Configuration.typeConf.Tables:
                            {
                                /*if (!ConfSelected)
                                {
                                    if (MessageBox.Show("Voulez-vous sauvegarder la configuration ?", "Gagnez du temps", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        Enregistre_Configuration EnregistreConfiguration = new Enregistre_Configuration(_mode, data);
                                        if (EnregistreConfiguration.ShowDialog() == DialogResult.OK)
                                        { Program.TheConfiguration.save_conf(_mode, EnregistreConfiguration.NomConf, Configuration.Create_Dic_params(_mode, grpBox_saisie.Host, grpBox_saisie.Port, grpBox_saisie.Database, grpBox_saisie.Username, grpBox_saisie.Password, null)); }
                                        EnregistreConfiguration.Close();
                                    }
                                }*/
                                string[] info_connexion = new string[] { grpBox_saisie.Host, grpBox_saisie.Port, grpBox_saisie.Username, grpBox_saisie.Password };
                                string[] Options = new string[] { "-Fc" };
                                dllClass.dump_table(Program.TheConfiguration.PathBinsPg + @"\pg_dump.exe", info_connexion, save.FileName, grpBox_saisie.Database, grpBox_saisie.Table, Options);
                                break;
                            }
                    }
                    Program.TheConfiguration.save_lastsaisie(_mode.ToString(), data);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Selection du fichier de sauvegarde", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            grpBox_saisie.Init_Champs(Program.TheConfiguration.load_conf((sender as ComboBox).Text, _mode), true);
        }
    }
}
