using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database_Backup
{
    /// <summary>
    /// Gestion de la Configuration
    /// </summary>
    public class Configuration
    {
        private String _PathBinsPg = string.Empty;

        /// <summary>
        /// Conscruteur
        /// </summary>
        /// <param name="Create">si true création d'un fichier vide. Si false, chargement des variables</param>
        public Configuration(bool Create)
        {
            if (Create) ConfigProgXML.SetStringParam("Paths", "PathBinsPg", @"C:\Program Files\PostgreSQL\14\bin");
            else
            {
                _PathBinsPg = ConfigProgXML.GetStringParam("Paths", "PathBinsPg");
            }
        }

        /// <summary>
        /// Determine si un fichier de configuration est présent
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsFileConfigPresent()
        {
            return ConfigProgXML.IsFileConfigPresent();
        }

        public String PathBinsPg
        {
            get
            {
                // if (_PathBinsPg.Equals(string.Empty)) throw new InvalidOperationException("La Variable est vide");
                return _PathBinsPg;
            }
            set
            {
                _PathBinsPg = value;
                ConfigProgXML.SetStringParam("Paths", "PathBinsPg", value);
            }
        }

    }
}
