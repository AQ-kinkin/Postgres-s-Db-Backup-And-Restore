using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Database_Backup
{
    internal static class Program
    {
        static public Configuration? TheConfiguration;

        static public void InitVariables()
        {
            if (Configuration.IsFileConfigPresent())
            {
                try
                {
                    TheConfiguration = new Configuration(false);
                }
                catch { throw; }
            }
            else
            {
                TheConfiguration = new Configuration(true);
                // throw new Exception("Votre fichier de configuration est vide.");
            }
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            String[] Args = Environment.GetCommandLineArgs();

            InitVariables();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Backup());
        }
    }
}
