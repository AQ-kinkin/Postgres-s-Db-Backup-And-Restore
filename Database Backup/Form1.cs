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

namespace Database_Backup
{
    public partial class Backup : Form
    {
        BackUp_SelectServer backUp_select_server;

        public Backup()
        {
            InitializeComponent();

            backUp_select_server = new BackUp_SelectServer(Configuration.typeConf.Servers);
            backUp_select_server.Dock = DockStyle.Fill;
            tabPage1.Controls.Add(backUp_select_server);

            tabPage3.Controls["groupBox3"].Controls["textBox1"].Text = Program.TheConfiguration.PathBinsPg;
        }

        private void Backup_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
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

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            switch(e.TabPageIndex)
            {
                case 0:
                    {
                        backUp_select_server.Mode = Configuration.typeConf.Servers;
                        tabPage1.Controls.Add(backUp_select_server);
                        break;
                    }
                case 3:
                    {
                        backUp_select_server.Mode = Configuration.typeConf.Tables;
                        tabPage4.Controls.Add(backUp_select_server);
                        break;
                    }
            }
        }
    }
}

