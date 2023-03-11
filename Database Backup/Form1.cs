using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using postgres_backups_solutions;

namespace Database_Backup
{
    public partial class Backup : Form
    {
        Imput_Params grpBox_saisie;

        public Backup()
        {
            InitializeComponent();

            grpBox_saisie = new Imput_Params(Imput_Params.mode.Serveur);
            grpBox_saisie.Location = new Point(16, 45);
            grpBox_saisie.load_backup_tab(Program.TheConfiguration.load_lastsaisie("Backup"));
            this.tabPage1.Controls.Add(grpBox_saisie);
            (this.tabPage1.Controls["comboBox1"] as ComboBox).Items.AddRange(Program.TheConfiguration.ListServer);

            tabPage3.Controls["groupBox3"].Controls["textBox1"].Text = Program.TheConfiguration.PathBinsPg;
        }

        private void Backup_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                call_postgre_backup dllClass = new call_postgre_backup();
                var save = new SaveFileDialog();
                save.Filter = "Fichier dump | *.dump";
                save.Title = "Enregistrer Sous";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    dllClass.Simple_Dump(Program.TheConfiguration.PathBinsPg + @"\pg_dump.exe", grpBox_saisie.Host, grpBox_saisie.Port, grpBox_saisie.Username, grpBox_saisie.Password, save.FileName, grpBox_saisie.Database);
                    if (MessageBox.Show("Voulez-vous sauvegarder la configuration ?", "Gagnez du temps", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Enregistre_Configuration EnregistreConfiguration = new Enregistre_Configuration(Enregistre_Configuration.mode.Serveur, grpBox_saisie.Host, grpBox_saisie.Port, grpBox_saisie.Database, grpBox_saisie.Username, grpBox_saisie.Password);
                        if (EnregistreConfiguration.ShowDialog() == DialogResult.OK)
                        { Program.TheConfiguration.save_conf_server(EnregistreConfiguration.Server, EnregistreConfiguration.Host, EnregistreConfiguration.Port, EnregistreConfiguration.Username, EnregistreConfiguration.Password, EnregistreConfiguration.Database); }
                        EnregistreConfiguration.Close();
                    }
                    Program.TheConfiguration.save_lastsaisie("Backup", grpBox_saisie.Host, grpBox_saisie.Port, grpBox_saisie.Username, grpBox_saisie.Password, grpBox_saisie.Database);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Selection du fichier de sauvegarde", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            var OpenFile = new OpenFileDialog();
            OpenFile.InitialDirectory = @"C:\";
            OpenFile.Filter = "Fichier Dump | *.dump";
            OpenFile.Title = "Select File";
            OpenFile.ShowDialog();
            dbrestore.Text = OpenFile.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
                Process process = new Process();
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = @"C:\Program Files\PostgreSQL\14\bin\pg_restore.exe";
                startInfo.Arguments = "-h " + Host2.Text + " -p " + Port2.Text + " -U " + Username2.Text + " -d " + Database2.Text + " -c " + dbrestore.Text;
                startInfo.EnvironmentVariables["PGPASSWORD"] = Password2.Text;
                process.StartInfo = startInfo;
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
                process.Close();
                MessageBox.Show("Done !");
        
        }

        private void select_dir(object sender, EventArgs e)
        {
            if (sender == null) return;
            switch((sender as Button).Name)
            {
                case "button2":
                {
                        var OpenDir = new FolderBrowserDialog();
                        OpenDir.RootFolder = Environment.SpecialFolder.MyComputer;
                        OpenDir.ShowDialog();
                        textBox1.Text = OpenDir.SelectedPath;
                        break;
                }
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Program.TheConfiguration.PathBinsPg = textBox1.Text;
        }
    }
}

