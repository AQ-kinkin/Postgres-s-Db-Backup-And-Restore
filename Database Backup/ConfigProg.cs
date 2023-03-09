using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Database_Backup
{ 
    static class ConfigProgXML
    {
        static public string GetRepConfProg()
        {
            return Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
        }
        static public string GetPathConfProg()
        {
            return GetRepConfProg() + @"\Conf_" + string.Join("", Path.GetFileNameWithoutExtension(System.Windows.Forms.Application.ExecutablePath).Split(' ')) + ".xml";
        }
        static public bool IsFileConfigPresent()
        {
            return File.Exists(GetPathConfProg());
        }
        static public string GetStringParam(string section, string NomParam)
        {
            string Reponse = string.Empty;

            if (IsFileConfigPresent())
            {
                //Ouvrir le fichier
                XmlDocument docxml = new XmlDocument();
                docxml.Load(GetPathConfProg());

                //On recupere le noeud racine dans la variable root
                XmlElement racine = docxml.DocumentElement;

                // Test si l'Ei est déja enregistrer
                XmlNode Node = docxml.SelectSingleNode("Programe/" + section + "/" + NomParam);

                if (Node != null) Reponse = Node.InnerText;
            }

            return Reponse;
        }
        static public bool SetStringParam(string section, string NomParam, string ValParam)
        {
            bool Reponse = false;
            string PathFileXmlConfigProg = GetPathConfProg();

            if (File.Exists(PathFileXmlConfigProg))
            {
                //Ouvrir le fichier
                XmlDocument docxml = new XmlDocument();
                docxml.Load(PathFileXmlConfigProg);

                //On recupere le noeud racine dans la variable root
                XmlElement racine = docxml.DocumentElement;

                // Test si l'Ei est déja enregistrer
                XmlNode Node = docxml.SelectSingleNode("Programe/" + section + "/" + NomParam);

                if (Node == null)
                {
                    //On crée un nouveau noeud
                    Node = docxml.CreateElement(section);

                    // On ajoute les noeuds
                    (Node.AppendChild(docxml.CreateElement(NomParam) as XmlNode)).InnerText = ValParam;

                    racine.AppendChild(Node);

                    Reponse = true;
                }
                else
                {
                    Node.InnerText = ValParam;
                    Reponse = true;
                }

                //On sauvegarde le fichier
                if (Reponse) docxml.Save(PathFileXmlConfigProg);
            }
            else
            {
                //ouverture du fichier
                XmlTextWriter docxml = null;
                try
                {
                    docxml = new XmlTextWriter(PathFileXmlConfigProg, Encoding.UTF8);
                    docxml.Formatting = Formatting.Indented;

                    //On écrit la première ligne avec les spécifications du document XML
                    docxml.WriteStartDocument();

                    //on écrit le noeud racine du document
                    docxml.WriteStartElement("Programe");

                    //On écrit la première section
                    docxml.WriteStartElement(section);

                    //On écrit le param
                    docxml.WriteStartElement(NomParam);
                    docxml.WriteString(ValParam);
                    docxml.WriteEndElement();

                    //On ferme la balise section
                    docxml.WriteEndElement();

                    //On ferme la balise Programe
                    docxml.WriteEndElement();
                }
                finally
                {
                    // Fermeture Fichier
                    if (docxml != null) docxml.Close();
                }
                Reponse = true;
            }

            return Reponse;
        }
    }
}
