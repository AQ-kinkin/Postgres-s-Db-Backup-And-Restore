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
        public Backup()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

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

/*                  Process process = new Process();
                    var startInfo = new ProcessStartInfo();
                    startInfo.FileName = @"C:\Program Files\PostgreSQL\14\bin\pg_dump.exe";
                    startInfo.Arguments = "-h " + Host.Text + " -f \"" + save.FileName + "\" -p " + Port.Text + " -U " + Username.Text + " -Fc " + Database.Text;
                    startInfo.EnvironmentVariables["PGPASSWORD"] = Password.Text;
                    process.StartInfo = startInfo;
                    startInfo.CreateNoWindow = true;
                    startInfo.UseShellExecute = false;
                    process.Start();
                    process.WaitForExit();
                    process.Close();*/
                    dllClass.Simple_Dump(textBox1.Text + @"\pg_dump.exe", Host.Text, Port.Text, Username.Text, Password.Text, save.FileName, Database.Text);
                    MessageBox.Show("Done !");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

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

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            textBox1.Text = Program.Configuration.PathBinsPg;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Program.Configuration.PathBinsPg = textBox1.Text;
        }
    }
}

