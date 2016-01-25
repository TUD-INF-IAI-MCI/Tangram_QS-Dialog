using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tud.mci.tangram.qsdialog.models;
using System.Xml;
using System.Xml.Schema;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using tud.mci.tangram.qsdialog.Dialogs;
using unoidl.com.sun.star.awt;
namespace tud.mci.tangram.EARL
{
    class EarlReader
    {

        public EarlReader() { }
        XmlDocument earl;
        XmlNamespaceManager nsmgr;
        public void updateDialog(OrderedDictionary od, XmlDocument earl, UnoDialogSample dialog)
        {
            

            List<Category> li = new List<Category>();
            foreach (Object c in od.Values)
            {
                //if (c is Category) System.Console.WriteLine(((Category)c).Criteria[0].Res.Rating[0] +"test");
                if (c is Category) li.Add(c as Category);
            }
            nsmgr = new XmlNamespaceManager(earl.NameTable);
            nsmgr.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            nsmgr.AddNamespace("rdfs", "http://www.w3.org/2000/01/rdf-schema#");
            nsmgr.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema#");
            nsmgr.AddNamespace("xml", "http://www.w3.org/XML/1998/namespace");
            nsmgr.AddNamespace("dc", "http://purl.org/dc/elements/1.1/");
            nsmgr.AddNamespace("earl", "http://www.w3.org/ns/earl#");
            nsmgr.AddNamespace("foaf", "http://xmlns.com/foaf/spec/");
            nsmgr.AddNamespace("dct", "http://purl.org/dc/terms/");
            nsmgr.AddNamespace("doap", "http://usefulinc.com/ns/doap#");
            this.earl = earl;


            #region set MediumTyp
            string text = earl.SelectSingleNode("//earl:TestSubject", nsmgr).InnerText;
            string medium = Regex.Match(text, "(Medium_)[a-zA-Z]*").ToString();
            medium = medium.Substring(7);

            if (medium.Equals("All") || medium.Equals("Schwellpapier") || medium.Equals("Tiger") || medium.Equals("Stiftplatte"))
            {
                ItemEvent ie = new ItemEvent();
                ie.Source = dialog.GetControlByName(medium) as XRadioButton;
                dialog.itemStateChanged(ie);
            }
            
            #endregion




            foreach (Category ca in li)
            {
                foreach (Criterion cr in ca.Criteria)
                {
                    XmlNode node = earl.SelectSingleNode("//earl:TestResult[@rdf:ID='" + "result_" + cr.Id + "']", nsmgr).FirstChild;
                    if (node.Name == "earl:info" )
                    {
                        cr.Comment = node.InnerText;
                    }
                    switch (cr.Res.Type)
                    {
                        case CriterionType.all:
                            for (int i = 0; i < cr.Res.Rating.Count; i++)
                            {
                                cr.Res.Rating[i] = getItemResult(cr, i);
                            }
                            break;
                        case CriterionType.one:
                            for (int i = 0; i < cr.Res.Rating.Count; i++)
                            {
                                cr.Res.Rating[i] = getItemResult(cr, i);
                            }
                            break;
                        case CriterionType.count:
                            for (int i = 0; i < cr.Res.Rating.Count; i++)
                            {
                                cr.Res.Rating[i] = getItemResult(cr, i);
                            }
                            break;
                        case CriterionType.rating:
                            this.setRating(cr, cr.Res.Rating);
                            break;
                    }
                }
            }

        }
        public int getItemResult(Criterion cr, int index)
        {
            String value ="";
            XmlNode node = earl.SelectSingleNode("//earl:TestResult[@rdf:ID='" + "result_" + cr.Id +"_item"+index+ "']", nsmgr);
            if (node != null)
            {
                value = node.LastChild.Attributes.GetNamedItem("rdf:resource").Value;
            }
            
            switch (value)
            {
                case "http://www.w3.org/ns/earl#failed":
                    return 0;
                case "http://www.w3.org/ns/earl#passed":
                    return 1;

            }
            //System.Console.WriteLine(value);
            return 0;
        }
        public void setRating(Criterion cr, List<float> resultList)
        {
            XmlNode node = earl.SelectSingleNode("//earl:TestResult[@rdf:ID='" + "result_" + cr.Id + "']", nsmgr);

            if (node != null) 
            {
                String rating = node.InnerText;
                Regex regex = new Regex("\\d+([,]{1}\\d+)?");
                Match ma = regex.Match(rating);
                float erg = float.Parse(ma.ToString());
                int index = Convert.ToInt16((erg * (cr.Res.Rating.Count - 1)));
                //noch nicht die perfekte Lösung aber zur Sicherheit erstmal alle Werte in der RatingListe auf 0 setzen
                for (int i = 0; i < cr.Res.Rating.Count; i++)
                {
                    cr.Res.Rating[i] = 0;

                }
                cr.Res.Rating[index] = 1;
            }
            }
            
             
            
        
    }


    //    Graph earlGraph;
    //    //List<String> NodeIdTestCases = new List<string>();
    //    //List<String> NodeIdFinalTest = new List<string>();
    //    List<String> categories = new List<String>();
    //    RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
    //    List<Uri> uriList = new List<Uri>();
    //    public EarlReader()
    //    {
    //        earlGraph= new Graph();
    //    }

    //    public void readingEARL(String file){
    //        earlGraph = new Graph();
    //        FileLoader.Load(earlGraph, file);
    //    }
    //    public void saveEarl()
    //    {
    //        rdfxmlwriter.Save(earlGraph, "earl.rdf");
    //    }
    //    public void createStringLists()
    //    {
    //        //IUriNode titel = earlGraph.CreateUriNode("dct:title");
    //        //IUriNode subject = earlGraph.GetUriNode(":BA");

    //        foreach (UriNode un in earlGraph.Nodes.UriNodes())
    //        {
    //            uriList.Add(un.Uri);


    //        }
    //        //foreach (Uri u in uriList)
    //        //{
    //        //   if(u.)
    //        //}
    //        //Regex nodeIdTestCaseReg = new Regex("[_][:][A-Z][A-Z][_][A-Z][A-Z]?");
    //        //Regex nodeIdFinalTestReg = new Regex("[_][:]finaltest");
    //        //foreach (INode node in earlGraph.Nodes.BlankNodes())
    //        //{

    //        //    if(nodeIdFinalTestReg.IsMatch(node.ToString()))NodeIdFinalTest.Add(node.ToString());
    //        //    if (nodeIdTestCaseReg.IsMatch(node.ToString())) NodeIdTestCases.Add(node.ToString());

    //            //foreach (Triple tri in earlGraph.GetTriplesWithSubject(node))
    //            //{
    //            //    System.Console.WriteLine(tri.ToString());
    //            //}
    //           //_:BA_

    //        }
    //        //foreach (String i in NodeIds)
    //        //{
    //        //    System.Console.WriteLine(i);
    //        //}
    //        //foreach(INode iN in earlGraph.Nodes.BlankNodes()){
    //        //    System.Console.WriteLine(iN);
    //        //    foreach(Triple tri in earlGraph.ge)
    //        //}
    //    }
    ////    public void showFinalTest()
    ////    {
    ////        //foreach(Triple tri in earlGraph.GetTriplesWithSubjectPredicate(earlGraph.GetUriNode()))
    ////        //createStringLists();
    ////        //foreach (String str in NodeIdFinalTest)
    ////        //{
    ////        //    foreach (Triple tri in earlGraph.GetTriplesWithPredicate(Uri."http://www.w3.org/ns/earl#test"))
    ////        //    {
    ////        //        System.Console.WriteLine(tri);
    ////        //    }
    ////        //}


    ////}
}
