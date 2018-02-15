using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PMML
{
    class Pmml
    {
        XmlDocument doc = new XmlDocument();
        string version;
        string appName;

        public Pmml(string dir)
        {
            doc.Load(dir);
            XmlNodeList tmp = doc.GetElementsByTagName("PMML");
            version = tmp[0].Attributes["version"].Value;
            appName = tmp[0].FirstChild.FirstChild.Attributes["name"].Value;
        }

        public NodeList getList(string name)
        {
            NodeList nList = new NodeList();
            XmlNodeList xNList = doc.GetElementsByTagName(name);

            nList.xmlNodeList = xNList;
            
            return nList;
        }
    }
}
