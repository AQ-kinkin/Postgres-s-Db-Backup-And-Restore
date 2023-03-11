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
            Servers
        };

        private String _PathBinsPg = string.Empty;
        public String[] ListServer { get; set; }


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
            ListServer = ConfigProgXML.GetListOfSection(Sections.Servers.ToString());
        }

        public bool save_lastsaisie(String NamePanel, String Host, String Port, String User, String PassWord, String NameBase)
        {
            return ConfigProgXML.SetSectionParam(Sections.LastUse.ToString(), NamePanel, new Dictionary<string, string>(){ { "Host", Host},  { "Port", Port}, { "NameBase", NameBase}, { "User", User}, { "PassWord", Encrypt(PassWord) } } );
        }

        public Dictionary<string, string> load_lastsaisie(string NamePanel)
        {
            Dictionary<string, string> Answer = new Dictionary<string, string>()
            {
                { "Host", string.Empty },
                { "Port", string.Empty},
                { "NameBase", string.Empty},
                { "User", string.Empty},
                { "PassWord", string.Empty}
            };

            ConfigProgXML.ReadSectionParam(Sections.LastUse.ToString(), NamePanel, Answer);

            Answer["PassWord"] = Decrypt(Answer["PassWord"]);

            return Answer;
        }

        public bool save_conf_server(string confname, string host, string port, string username, string password, string database)
        {
            return ConfigProgXML.SetSectionParam(Sections.Servers.ToString(), confname, new Dictionary<string, string>() {
                { "Host", host }, { "Port", port }, { "NameBase", database }, { "User", username }, { "PassWord", Encrypt(password) }
            });
        }

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
    }
}
