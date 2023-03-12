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
                XmlNode Node = docxml.SelectSingleNode("Program/" + section + "/" + NomParam);

                if (Node != null) Reponse = Node.InnerText;
            }

            return Reponse;
        }
        static public bool SetStringParam(string section, string ParamName, string ValParam)
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
                XmlNode Node = docxml.SelectSingleNode("Program/" + section + "/" + ParamName);

                if (Node == null)
                {
                    //On crée un nouveau noeud
                    Node = docxml.CreateElement(section);

                    // On ajoute les noeuds
                    (Node.AppendChild(docxml.CreateElement(ParamName) as XmlNode)).InnerText = ValParam;

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
                    docxml.WriteStartElement("Program");

                    //On écrit la première section
                    docxml.WriteStartElement(section);

                    //On écrit le param
                    docxml.WriteStartElement(ParamName);
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

        static public string[] GetListOfSection(string SectionName)
        {
            string[] Reponse = null;

            if (IsFileConfigPresent())
            {
                //Ouvrir le fichier
                XmlDocument docxml = new XmlDocument();
                docxml.Load(GetPathConfProg());

                //On recupere le noeud racine dans la variable root
                XmlElement racine = docxml.DocumentElement;

                // Test si l'Ei est déja enregistrer
                XmlNodeList Nodes = docxml.SelectNodes("Program/" + SectionName + "/*");
                if( (Nodes != null) && (Nodes.Count > 0) )
                {
                    Reponse = new string[Nodes.Count];
                    for (int i = 0; i < Nodes.Count; i++)
                    {
                        Reponse[i] = Nodes[i].Name;
                    }
                }
            }

            return (Reponse == null)?new string[0]: Reponse;
        }

        static public void ReadSectionParam(string SectionName, string namePanel, Dictionary<string, string> Data)
        {
            string Reponse = string.Empty;
            XmlNode Node, ParentNode = null;

            if (IsFileConfigPresent())
            {
                //Ouvrir le fichier
                XmlDocument docxml = new XmlDocument();
                docxml.Load(GetPathConfProg());

                //On recupere le noeud racine dans la variable root
                XmlElement racine = docxml.DocumentElement;

                // Test si l'Ei est déja enregistrer
                Node = docxml.SelectSingleNode("Program/" + SectionName + "/" + namePanel);
                if (Node != null)
                {
                    ParentNode = Node;

                    for (int i = 0; i < Data.Count; i++)
                    {
                        KeyValuePair<string, string> keysloop = Data.ElementAt(i);
                        Node = ParentNode.SelectSingleNode(keysloop.Key);
                        if (Node != null) { Data[keysloop.Key] = Node.InnerText; }
                    }
                }
            }
        }

        static public bool SetSectionParam(string SectionName, string namePanel, Dictionary<string, string> Data)
        {
            bool Reponse = false;
            string PathFileXmlConfigProg = GetPathConfProg();
            XmlNode Node, ParentNode = null;
            if (Data == null) return false;

            if (File.Exists(PathFileXmlConfigProg))
            {
                //Ouvrir le fichier
                XmlDocument docxml = new XmlDocument();
                docxml.Load(PathFileXmlConfigProg);

                //On recupere le noeud racine dans la variable root
                try
                {
                    XmlElement racine = docxml.DocumentElement;

                    // Test si l'Ei est déja enregistrer
                    Node = docxml.SelectSingleNode("Program/" + SectionName);
                    if (Node == null)
                    {
                        //On crée un nouveau noeud
                        Node = docxml.CreateElement(SectionName);
                        
                        // On ajoute le noeud
                        racine.AppendChild(Node);
                    }
                    ParentNode = Node;

                    // Test si l'Ei est déja enregistrer
                    Node = docxml.SelectSingleNode("Program/" + SectionName  + "/" + namePanel);
                    if (Node == null)
                    {
                        //On crée un nouveau noeud
                        Node = docxml.CreateElement(namePanel);

                        ParentNode.AppendChild(Node);
                    }
                    ParentNode = Node;

                    for(int i = 0; i < Data.Count; i++)
                    {
                        KeyValuePair<string, string> keysloop = Data.ElementAt(i);
                        Node = ParentNode.SelectSingleNode(keysloop.Key);
                        if (Node == null)
                        {
                            // On ajoute les noeuds
                            ParentNode.AppendChild(docxml.CreateElement(keysloop.Key)).InnerText = keysloop.Value;
                        }
                        else { Node.InnerText = keysloop.Value; }
                    }

                    Reponse = true;
                }
                catch { throw; }

                //On sauvegarde le fichier
                if (Reponse) docxml.Save(PathFileXmlConfigProg);
            }

            return Reponse;
        }
    }
}
