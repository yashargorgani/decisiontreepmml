using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PMML;

namespace DecisionTree
{
    class Fields
    {
        public Dictionary<string, string> type = new Dictionary<string,string>();   //this dictionary contains the type name of each field

        NodeList DataField = null;

        //with constructing new object based on and XML get element DataField(GetElementsByTagName("DataField")) list, the type dictionary will initialize
        public Fields(NodeList dataDic)
        {
            DataField = dataDic;
            for (int i = 0; i < DataField.Size(); i++)
            {
                type.Add(DataField.getNode(i).Attribute("name"), DataField.getNode(i).Attribute("dataType"));
            }
        }
    }
}
