using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;

namespace APIAddIn
{
    public class DiagramManager
    {
        static Logger logger = new Logger();

        static List<int> diagramLinks = null;

        static public void setLogger(Logger l)
        {
            logger = l;
        }


        static public bool isVisible(EA.Connector con)
        {
            if (diagramLinks != null)
                return diagramLinks.Contains(con.ConnectorID);
            return true;
        }

        static public void captureDiagramLinks(EA.Diagram diagram)
        {
            diagramLinks = new List<int>();
            foreach (object link in diagram.DiagramLinks)
            {
                EA.DiagramLink l = (EA.DiagramLink)link;
                if (!l.IsHidden)
                    diagramLinks.Add(l.ConnectorID);
            }
            logger.log("Number of diagram links:" + diagramLinks.Count);
        }

        static public List<string> queryAPIDiagrams(EA.Repository Repository)
        {
            string resultDoc = Repository.SQLQuery("SELECT ea_guid FROM t_diagram where stereotype='APIDiagram';");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(resultDoc);
            List<string> result = new List<string>();
            foreach (XmlNode node in doc.GetElementsByTagName("ea_guid"))
            {
                result.Add(node.InnerText);
            }
            return result;
        }
        static public List<string> querySchemaDiagrams(EA.Repository Repository)
        {

            string resultDoc = Repository.SQLQuery("SELECT ea_guid FROM t_diagram where Stereotype='SchemaDiagram'");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(resultDoc);
            List<string> result = new List<string>();
            foreach (XmlNode node in doc.GetElementsByTagName("ea_guid"))
            {
                result.Add(node.InnerText);
            }
            return result;
        }
        static public List<string> querySampleDiagrams(EA.Repository Repository)
        {

            string resultDoc = Repository.SQLQuery("SELECT ea_guid FROM t_diagram where Stereotype='SampleDiagram'");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(resultDoc);
            List<string> result = new List<string>();
            foreach (XmlNode node in doc.GetElementsByTagName("ea_guid"))
            {
                result.Add(node.InnerText);
            }
            return result;
        }

        static public void exportDiagram(EA.Repository Repository,EA.Diagram diagram)
        {            
            string confluencedata = null;

            if (diagram.Version != "1.0")
            {
                confluencedata = diagram.Version;
            }
            else
            {
                confluencedata = diagram.Notes;
            }
            if (confluencedata == null || confluencedata.Length == 0)
            {                
                logger.log("Diagram ["+diagram.Name+"] notes field does not specify a <confluence page name>. Default to CanonicalDataModel");
                confluencedata = "CanonicalDataModel";
            }
            
            {
                char[] delimiter = { ',' };
                string[] pages = confluencedata.Split(delimiter);
                foreach (string page in pages)
                {
                    string file = @"d:\tmp\content\" + page + "---" + diagram.Name + ".svg";
                    logger.log(file);
                    SVGExport.EAPlugin.SaveDiagramAsSvg(Repository, diagram, file);
                }
            }

        }

    }
}
