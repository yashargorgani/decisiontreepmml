using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using PMML;

namespace DecisionTree
{
    class TreeNode
    {
        public int id; //id of the node in XML file
        public int[] cID;   //array of its children ids
        public Dictionary<string, string> type = new Dictionary<string, string>();
        Node predicate;
        public string score;

        Node xnode;

        public TreeNode(Node xmlNd)
        {
            xnode = xmlNd;
            score = xnode.Attribute("score");
            for (int i = 0; i < xnode.ChildCount(); i++)
            {
                switch (xnode.Child(i).Name())
            {
                case "True": predicate = xnode.Child(i);
                    break;
                case "False": predicate = xnode.Child(i);
                    break;
                case "SimplePredicate": predicate = xnode.Child(i);
                    break;
                case "CompoundPredicate": predicate = xnode.Child(i);
                    break;
            }
            }
        }

        //Checks the Node Predicate
        public Boolean Predicate(Dictionary<string, object> inputDic)
        {
            Boolean result = false;
                        
            switch (predicate.Name())
            {
                case "True": result = true;
                    break;
                case "False": result = false;
                    break;
                case "SimplePredicate": result = Simple(inputDic);
                    break;
                case "CompoundPredicate": result = Compound(inputDic);
                    break;
            }
            return result;
        }


        //this function checks NOT Default Predicate
        Boolean Predicate(Dictionary<string, object> inputDic, Node fPred)
        {
            Boolean result = false;

            switch (fPred.Name())
            {
                case "True": result = true;
                    break;
                case "False": result = false;
                    break;
                case "SimplePredicate": result = Simple(inputDic, fPred);
                    break;
                case "CompoundPredicate": result = Compound(inputDic, fPred);
                    break;
            }
            return result;
        }

        //checks Default Predicate
        Boolean Compound(Dictionary<string, object> inputDic)
        {
            Boolean result = false;

            switch (predicate.Attribute("booleanOperator"))
            {
                case "or": for (int i = 0; i < predicate.ChildCount(); i++)
                    {
                        if (Predicate(inputDic, predicate.Child(i)))
                        {
                            result = true;
                            break;
                        }
                    }
                    break;
                case "and": for (int i = 0; i < predicate.ChildCount(); i++)
                    {
                        result = true;
                        if (!Predicate(inputDic, predicate.Child(i)))
                        {
                            result = false;
                            break;
                        }
                    }
                    break;
                case "surrogate": for (int i = 0; i < predicate.ChildCount(); i++)
                    {
                        if (Predicate(inputDic, predicate.Child(i)) != null)
                        {
                            result = Predicate(inputDic, predicate.Child(i));
                            break;
                        }
                    }
                    break;
            }
            
            return result;
        }

        //checks Not Default Predicate
        Boolean Compound(Dictionary<string, object> inputDic, Node fPred)
        {
            Boolean result = false;

            switch (fPred.Attribute("booleanOperator"))
            {
                case "or": for (int i = 0; i < fPred.ChildCount(); i++)
                    {
                        if (Predicate(inputDic, fPred.Child(i)))
                        {
                            result = true;
                            break;
                        }
                    }
                    break;
                case "and": for (int i = 0; i < fPred.ChildCount(); i++)
                    {
                        result = true;
                        if (!Predicate(inputDic, fPred.Child(i)))
                        {
                            result = false;
                            break;
                        }
                    }
                    break;
                case "surrogate": for (int i = 0; i < fPred.ChildCount(); i++)
                    {
                        if (Predicate(inputDic, fPred.Child(i)) != null)
                        {
                            result = Predicate(inputDic, fPred.Child(i)) != null;
                            break;
                        }
                    }
                    break;
            }

            return result;
        }

        //This function checks a Simple Predicate with Nodes Defaul Predicate
        Boolean Simple(Dictionary<string, object> inputDic)
        {
            Boolean result = false;
            switch (predicate.Attribute("operator"))
            {
                case "equal": if (type[predicate.Attribute("field")] == "double")
                    {
                        result = (Convert.ToDouble(inputDic[predicate.Attribute("field")]) == Convert.ToDouble(predicate.Attribute("value")));
                    }
                    else
                    {
                        result = (Convert.ToString(inputDic[predicate.Attribute("field")]) == predicate.Attribute("value"));
                    }
                    break;
                case "notEqual": if (type[predicate.Attribute("field")] == "double")
                    {
                        result = (Convert.ToDouble(inputDic[predicate.Attribute("field")]) != Convert.ToDouble(predicate.Attribute("value")));
                    }
                    else
                    {
                        result = (Convert.ToString(inputDic[predicate.Attribute("field")]) != predicate.Attribute("value"));
                    }
                    break;
                case "lessThan": result = (Convert.ToDouble(inputDic[predicate.Attribute("field")]) < Convert.ToDouble(predicate.Attribute("value")));
                    break;
                case "lessOrEqual": result = (Convert.ToDouble(inputDic[predicate.Attribute("field")]) <= Convert.ToDouble(predicate.Attribute("value")));
                    break;
                case "greaterThan": result = (Convert.ToDouble(inputDic[predicate.Attribute("field")]) > Convert.ToDouble(predicate.Attribute("value")));
                    break;
                case "greaterOrEqual": result = (Convert.ToDouble(inputDic[predicate.Attribute("field")]) >= Convert.ToDouble(predicate.Attribute("value")));
                    break;
            }


            return result;
        }

        // This function checks a Simple Predicate with NOT DEFAULT Predicate
        Boolean Simple(Dictionary<string, object> inputDic, Node fPred)
        {
            Boolean result = false;
            switch (fPred.Attribute("operator"))
            {
                case "equal": if (type[fPred.Attribute("field")] == "double")
                    {
                        result = (Convert.ToDouble(inputDic[fPred.Attribute("field")]) == Convert.ToDouble(fPred.Attribute("value")));
                    }
                    else
                    {
                        result = (Convert.ToString(inputDic[fPred.Attribute("field")]) == fPred.Attribute("value"));
                    }
                    break;
                case "notEqual": if (type[fPred.Attribute("field")] == "double")
                    {
                        result = (Convert.ToDouble(inputDic[fPred.Attribute("field")]) != Convert.ToDouble(fPred.Attribute("value")));
                    }
                    else
                    {
                        result = (Convert.ToString(inputDic[fPred.Attribute("field")]) != fPred.Attribute("value"));
                    }
                    break;
                case "lessThan": result = (Convert.ToDouble(inputDic[fPred.Attribute("field")]) < Convert.ToDouble(fPred.Attribute("value")));
                    break;
                case "lessOrEqual": result = (Convert.ToDouble(inputDic[fPred.Attribute("field")]) <= Convert.ToDouble(fPred.Attribute("value")));
                    break;
                case "greaterThan": result = (Convert.ToDouble(inputDic[fPred.Attribute("field")]) > Convert.ToDouble(fPred.Attribute("value")));
                    break;
                case "greaterOrEqual": result = (Convert.ToDouble(inputDic[fPred.Attribute("field")]) >= Convert.ToDouble(fPred.Attribute("value")));
                    break;
            }


            return result;
        }


   }
}
