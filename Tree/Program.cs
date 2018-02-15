using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DecisionTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("K", 0.056);
            input.Add("Na", 0.739);
            input.Add("BP", "LOW");
            input.Add("Sex", "M");
            input.Add("Age", 47);
            input.Add("Cholesterol", "HIGH");

            TreeModel t1 = new TreeModel("E:/IT/PMML/Decision Trees/Models Files/Drug.xml");
            Console.WriteLine(t1.Predict(input));
            Console.ReadKey();
        }
    }


}
