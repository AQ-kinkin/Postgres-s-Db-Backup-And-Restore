using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Database_Backup
{
    /// <summary>
    /// Gestion de la Configuration
    /// </summary>
    public class Configuration
    {
        private enum Sections
        {
            LastUse,
            Servers,
            Tables
        };

        public enum typeConf
        {
            Servers,
            Tables
        }

        private String _PathBinsPg = string.Empty;
        public Dictionary<typeConf,string[]> ListServer { get; set; }

        /// <summary>
        /// Conscruteur
        /// </summary>
        /// <param name="Create">si true création d'un fichier vide. Si false, chargement des variables</param>
        public Configuration(bool Create)
        {
            // byte[] textAsByte = Encoding.Default.GetBytes("Cruelle, pas le moindre mélange de sang et qu'il le reconnaissait pour avoir aidé la nuit précédente, uniquement parce qu'il vous plaira... Revenus devant le feu, et ne laisse que la paille de votre oeil avant d'avoir attrapé le criminel ? Éventons, pour la persistance de ce souvenir de femme. Prévoyant les événements, à étudier les étoiles. Tue le prince, avant-hier, ce matin ! Exactement ce dont nous étions entourés. Être sortie plusieurs fois avec une furie si grande et si noble étaient bouleversés par ces horribles symptômes qu'il avait versé.Plans politiques de l'économie de la nature renversé, quand j'eus satisfait sa curiosité : eh bien, dit-il.");
            // String testyyy = System.Convert.ToBase64String(textAsByte);

            if (Create) ConfigProgXML.SetStringParam("Paths", "PathBinsPg", @"C:\Program Files\PostgreSQL\14\bin");
            else
            {
                _PathBinsPg = ConfigProgXML.GetStringParam("Paths", "PathBinsPg");
                load_ListServer();
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

        private void load_ListServer()
        {
            ListServer = new Dictionary<typeConf,string[]>();

            foreach (typeConf mode in Enum.GetValues(typeof(typeConf)))
            {
                ListServer.Add(mode, ConfigProgXML.GetListOfSection(mode.ToString()));
            }
        }

        public static Dictionary<string, string> Create_Dic_params(Configuration.typeConf mode, string host, string port, string database, string user, string password, string tablename)
        {
            Dictionary<string, string> Answer = new Dictionary<string, string>() {
                { "Host", host },
                { "Port", port },
                { "NameBase", database },
                { "User", user },
                { "PassWord", password }
             };
            if (mode == Configuration.typeConf.Tables) Answer.Add("Table", tablename);

            return Answer;
        }

        private Dictionary<string, string> Create_list_param(typeConf mode)
        {
            Dictionary<string, string> Answer = new Dictionary<string, string>() {
                { "Host", string.Empty },
                { "Port", string.Empty},
                { "NameBase", string.Empty},
                { "User", string.Empty},
                { "PassWord", string.Empty}
             };
            if (mode == typeConf.Tables) Answer.Add("Table", string.Empty);
            return Answer;
        }

        public bool save_lastsaisie(String NamePanel, Dictionary<string, string> Params)
        {
            Params["PassWord"] = Encrypt(Params["PassWord"]);
            return ConfigProgXML.SetSectionParam(Sections.LastUse.ToString(), NamePanel, Params);
        }
        public bool save_conf(typeConf mode, string confname, Dictionary<string, string> Params)
        {
            Params["PassWord"] = Encrypt(Params["PassWord"]);
            return ConfigProgXML.SetSectionParam(mode.ToString(), confname, Params);
        }

        /*public bool save_conf_table(string confname, string host, string port, string username, string password, string database, string tablename)
        {
            return ConfigProgXML.SetSectionParam(Sections.Tables.ToString(), confname, new Dictionary<string, string>() {
                { "Host", host }, { "Port", port }, { "NameBase", database }, { "User", username }, { "PassWord", Encrypt(password) }, { "Table", tablename }
            });
        }*/

        public string Encrypt(string clearText)
        {
            string Answer = string.Empty;
            string EncryptionKey = "AQUI??P§NI#21#2";

            if (string.IsNullOrEmpty(clearText)) return string.Empty;

            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    Answer = Convert.ToBase64String(ms.ToArray());
                }
            }
            return Answer;
        }

        public string Decrypt(string cipherText)
        {
            string Answer = string.Empty;
            string EncryptionKey = "AQUI??P§NI#21#2";
            
            if (string.IsNullOrEmpty(cipherText)) return Answer;

            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        Answer = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch
            {
                return string.Empty;
            }

            return Answer;
        }
        public Dictionary<string, string> load_lastsaisie(typeConf mode)
        {
            Dictionary<string, string> Answer = Create_list_param(mode);

            return load_conf(Sections.LastUse, mode.ToString(), Answer);
        }
        public Dictionary<string, string> load_conf(string NamePanel, typeConf mode)
        {
            Dictionary<string, string> Answer = Create_list_param(mode);

            Sections _section = Sections.Servers;
            if (mode == typeConf.Tables) _section = Sections.Tables;

            return load_conf(_section, NamePanel, Answer);
        }
        private Dictionary<string, string> load_conf(Sections section, String nameConf, Dictionary<string, string> Answer)
        {
            ConfigProgXML.ReadSectionParam(section.ToString(), nameConf, Answer);

            Answer["PassWord"] = Decrypt(Answer["PassWord"]);

            return Answer;
        }
    }
}
