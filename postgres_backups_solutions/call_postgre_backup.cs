using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace postgres_backups_solutions
{
    public class call_postgre_backup
    {
        public void dump_base(string binpath, string host, string port, string user, string pwd, string fullpathfile, string dbname)
        {
            Process process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = binpath;
            startInfo.Arguments = "-h " + host + " -f \"" + fullpathfile + "\" -p " + port + " -U " + user + " -Fc " + dbname;
            startInfo.EnvironmentVariables["PGPASSWORD"] = pwd;
            process.StartInfo = startInfo;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            process.Close();
        }

        public void dump_table(string binpath, string host, string port, string user, string pwd, string fullpathfile, string dbname, string tabname)
        {
            Process process = new Process();
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = binpath;
            startInfo.Arguments = "-h " + host + " -f \"" + fullpathfile + "\" -p " + port + " -U " + user + " -Fc -d" + dbname + " -t" + tabname;
            startInfo.EnvironmentVariables["PGPASSWORD"] = pwd;
            process.StartInfo = startInfo;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            process.Close();
        }
    }
}
