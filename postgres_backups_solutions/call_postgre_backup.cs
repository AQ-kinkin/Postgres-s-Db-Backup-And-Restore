using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace postgres_backups_solutions
{
    public class call_postgre_backup
    {
        /// <summary>
        /// Dump d'une BDD de type postgreSQL
        /// </summary>
        /// <param name="binpath">Chemin local des binaires postgreSQL</param>
        /// <param name="Connexion_Info">Liste des information de connexion [0] = host , [1] = port, [2] = user, [3] = pwd. peux être null si non nécéssaire. </param>
        /// <param name="fullpathfile">Chemin local du fichier resulta du dump<</param>
        /// <param name="dbname">name of database</param>
        /// <param name="options">Liste des obtions à trnamettre à dump. </param>
        public void dump_base(string binpath, string[] Connexion_Info, string fullpathfile, string dbname, string[] options)
        {
            string Arguments = get_Arguments_connecxion(Connexion_Info) + "-f \"" + fullpathfile + "\" "  + get_Arguments_options(options) + dbname;
            string pwd = string.Empty;
            if (Connexion_Info != null)
            {
                if (!string.IsNullOrEmpty(Connexion_Info[3]))
                {
                    pwd = Connexion_Info[3];
                }
            }
            exec(binpath, Arguments, pwd);
        }

        private string get_Arguments_options(string[] options)
        {
            string answer = string.Empty;

            if (options == null) return answer;

            for(int pos=0; pos < options.Length; pos++)
            {
                if (!string.IsNullOrEmpty(options[pos]))
                {
                    answer = options[pos] + " ";
                }
            }

            return answer;
        }

        private string get_Arguments_connecxion(string[] Connexion_Info)
        //private string get_Arguments_connecxion(string[] Connexion_Info, int ctrlSize)
        {
            const int ctrlSize = 4;
            string answer = string.Empty;

            if (Connexion_Info == null) return answer;

            if (Connexion_Info.Length != ctrlSize) throw new ArgumentException("seul null or string[4] est autorisé.");

            if (!string.IsNullOrEmpty(Connexion_Info[0]))
            {
                answer = "-h " + Connexion_Info[0] + " ";
            }

            if (!string.IsNullOrEmpty(Connexion_Info[1]))
            {
                answer = "-p " + Connexion_Info[1] + " ";
            }

            if (!string.IsNullOrEmpty(Connexion_Info[2]))
            {
                answer = "-U " + Connexion_Info[2] + " ";
            }

            //if(ctrlSize > 4)
            //{
            //    answer = "-d " + Connexion_Info[4] + " ";
            //}

            return answer;
        }

        /// <summary>
        /// Dump d'une table dans une BDD de type postgreSQL
        /// </summary>
        /// <param name="binpath">Chemin local des binaires postgreSQL</param>
        /// <param name="Connexion_Info">Liste des information de connexion [0] = host , [1] = port, [2] = user, [3] = pwd. peux être null si non nécéssaire. </param>
        /// <param name="fullpathfile">Chemin local du fichier resulta du dump<</param>
        /// <param name="dbname">name of database</param>
        /// <param name="tabname">name of table in the database </param>
        /// <param name="options">Liste des obtions à trnamettre à dump. </param>
        public void dump_table(string binpath, string[] Connexion_Info, string fullpathfile, string dbname, string tabname, string[] options)
        {
            string Arguments = get_Arguments_connecxion(Connexion_Info) + "-f \"" + fullpathfile + "\" " + get_Arguments_options(options) + dbname + " -t" + tabname;
            string pwd = string.Empty;
            if (Connexion_Info != null)
            {
                if (!string.IsNullOrEmpty(Connexion_Info[3]))
                {
                    pwd = Connexion_Info[3];
                }
            }
            exec(binpath, Arguments, pwd); ;
        }

        ///// <summary>
        ///// Sauvegarde glogal de type physique de type postgreSQL
        ///// </summary>
        ///// <param name="binpath">Chemin local des binaires postgreSQL</param>
        ///// <param name="Connexion_Info">Liste des information de connexion [0] = host , [1] = port, [2] = user, [3] = pwd. peux être null si non nécéssaire. </param>
        ///// <param name="fullpathfile">Chemin local du fichier resulta du dump<</param>
        ///// <param name="options">Liste des obtions à trnamettre à dump. </param>
        //public void basebackup(string binpath, string[] Connexion_Info, string fullpathfile, string[] options)
        //{
        //    string Arguments = get_Arguments_connecxion(Connexion_Info)

        //    string pwd = string.Empty;
        //    if (Connexion_Info != null)
        //    {
        //        if (!string.IsNullOrEmpty(Connexion_Info[3]))
        //        {
        //            pwd = Connexion_Info[3];
        //        }
        //    }
        //    exec(binpath, Arguments, pwd); ;
        //}

        public void dumpall(string binpath, string[] Connexion_Info, string fullpathfile, string[] options)
        {
            string Arguments = get_Arguments_connecxion(Connexion_Info) + "-f \"" + fullpathfile + "\" " + get_Arguments_options(options); 

            string pwd = string.Empty;
            if (Connexion_Info != null)
            {
                if (!string.IsNullOrEmpty(Connexion_Info[3]))
                {
                    pwd = Connexion_Info[3];
                }
            }
            exec(binpath, Arguments, pwd); ;
        }
        

        private void exec(string binpath, string arguments, string pwd)
        {
            Process process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = binpath;
            startInfo.Arguments = arguments;
            if (!string.IsNullOrEmpty(pwd)) startInfo.EnvironmentVariables["PGPASSWORD"] = pwd;
            process.StartInfo = startInfo;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            process.Close();
        }
    }
}
