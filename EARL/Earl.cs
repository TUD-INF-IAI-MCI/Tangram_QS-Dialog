using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tud.mci.tangram.qsdialog.models;
using System.Xml;
using System.Xml.Schema;


namespace tud.mci.tangram.EARL
{
    //Klasse zum erstellen der EARL RDF
    public class Earl
    {

        //2. Versuch
        String baseUri;
        XmlDocument earl;
        XmlElement testPerson;
        XmlElement software2;
        XmlElement group;
        XmlElement testsubject;
        XmlNamespaceManager nsmgr;
        XmlElement root;
        public MediaType mediaType;


        public Earl()
        {
        }


        //Creates a XML-Document with namespaces, testsubject, testergroup  
        public void createEmptyEarlGraph(MediaType chosenMediaType)
        {
            this.mediaType = chosenMediaType;
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            baseUri = "XMLFile1.xml";
            earl = new XmlDocument();
            XmlDeclaration xdeclaration = earl.CreateXmlDeclaration("1.0", "utf-8", null);
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
            XmlDocumentType doctype;
            doctype = earl.CreateDocumentType("rdf:RDF", null, null, @"
    <!ENTITY rdf 'http://www.w3.org/1999/02/22-rdf-syntax-ns#'>
    <!ENTITY rdfs 'http://www.w3.org/2000/01/rdf-schema#'>
    <!ENTITY xsd 'http://www.w3.org/2001/XMLSchema#'>
    <!ENTITY xml 'http://www.w3.org/XML/1998/namespace'>
    <!ENTITY dc 'http://purl.org/dc/elements/1.1/'>
    <!ENTITY earl 'http://www.w3.org/ns/earl#'>
    <!ENTITY foaf 'http://xmlns.com/foaf/spec/'>
    <!ENTITY dct 'http://purl.org/dc/terms/'>
    <!ENTITY doap 'http://usefulinc.com/ns/doap#'>");
            earl.AppendChild(doctype);


            root = earl.CreateElement("rdf", "RDF", nsmgr.LookupNamespace("rdf"));

            foreach (String prefix in nsmgr)
            {
                if (!String.IsNullOrWhiteSpace(prefix)
                    && !prefix.Equals("xmlns")
                    )
                {
                    var uri = nsmgr.LookupNamespace(prefix);
                    if (!String.IsNullOrWhiteSpace(uri))
                        root.SetAttribute("xmlns:" + prefix, uri);
                }
            }
            earl.AppendChild(root);

            software2 = earl.CreateElement("earl", "Software", nsmgr.LookupNamespace("earl"));
            software2.SetAttributeNode("ID", nsmgr.LookupNamespace("rdf"));
            software2.SetAttribute("ID", "pruefdialog");
            root.AppendChild(software2);

            XmlElement desc = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
            desc.AppendChild(earl.CreateTextNode("Ein Dialog zur Bewertung taktiler Grafiken"));
            desc.SetAttributeNode("lang", nsmgr.LookupNamespace("xml"));
            desc.SetAttribute("lang", "de");
            software2.AppendChild(desc);

            XmlElement title = earl.CreateElement("dct", "title", nsmgr.LookupNamespace("dct"));
            title.AppendChild(earl.CreateTextNode("Pruefdialog taktile Grafiken"));
            title.SetAttributeNode("lang", nsmgr.LookupNamespace("xml"));
            title.SetAttribute("lang", "de");
            software2.AppendChild(title);

            testPerson = earl.CreateElement("foaf", "Person", nsmgr.LookupNamespace("foaf"));
            testPerson.SetAttributeNode("ID", nsmgr.LookupNamespace("rdf"));
            testPerson.SetAttribute("ID", "testperson");

            XmlElement name = earl.CreateElement("foaf", "name", nsmgr.LookupNamespace("foaf"));
            name.AppendChild(earl.CreateTextNode("Max Mustermann"));
            testPerson.AppendChild(name);

            group = earl.CreateElement("foaf", "Group", nsmgr.LookupNamespace("foaf"));
            group.SetAttributeNode("ID", nsmgr.LookupNamespace("rdf"));
            group.SetAttribute("ID", "assertorgroup");
            root.AppendChild(group);

            //TODOOOOOOOO!!!!
            XmlElement descgroup = earl.CreateElement("dct", "title", nsmgr.LookupNamespace("dct"));
            descgroup.AppendChild(earl.CreateTextNode("Max Mustermann und Pruefdialog"));
            group.AppendChild(descgroup);

            XmlElement descAssertorGroup = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
            descAssertorGroup.AppendChild(earl.CreateTextNode("Max Mustermann überprüfte die Grafik manuell mit dem Pruefdialog"));
            group.AppendChild(descAssertorGroup);

            XmlElement mainAssertor = earl.CreateElement("earl", "mainAssertor", nsmgr.LookupNamespace("earl"));
            mainAssertor.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
            mainAssertor.SetAttribute("resource", "#"+software2.GetAttribute("ID"));
            group.AppendChild(mainAssertor);

            XmlElement foafMember = earl.CreateElement("foaf", "member", nsmgr.LookupNamespace("foaf"));
            group.AppendChild(foafMember);

            foafMember.AppendChild(testPerson);

            testsubject = earl.CreateElement("earl", "TestSubject", nsmgr.LookupNamespace("earl"));
            testsubject.SetAttributeNode("ID", nsmgr.LookupNamespace("rdf"));
            testsubject.SetAttribute("ID", "testsubject");
            root.AppendChild(testsubject);

            XmlElement bildTitle = earl.CreateElement("dct", "title", nsmgr.LookupNamespace("dct"));
            bildTitle.AppendChild(earl.CreateTextNode("Titel der Grafik"));
            bildTitle.SetAttributeNode("lang", nsmgr.LookupNamespace("xml"));
            bildTitle.SetAttribute("lang", "de");
            testsubject.AppendChild(bildTitle);

            XmlElement mediaType = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
            mediaType.AppendChild(earl.CreateTextNode("Die geprüfte Grafik"+"(Medium_"+ chosenMediaType.ToString().Normalize() +")" ));
            testsubject.AppendChild(mediaType);




        }

        //public void createTestRequirement(Category ca)
        //{
        //    XmlElement category1 = earl.CreateElement("earl","TestRequirement", nsmgr.LookupNamespace("earl"));
        //    category1.SetAttributeNode("about", nsmgr.LookupNamespace("rdf"));
        //    category1.SetAttribute("about", baseUri + "#" + ca.Id);
        //    root.AppendChild(category1);

        //    XmlElement desc = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
        //    desc.AppendChild(earl.CreateTextNode(ca.Desc.Normalize()));
        //    category1.AppendChild(desc);

        //    XmlElement title = earl.CreateElement("dct", "title", nsmgr.LookupNamespace("dct"));
        //    title.AppendChild(earl.CreateTextNode(ca.Name.Normalize()));
        //    category1.AppendChild(title);
            
            

        //}


        //public void createTestCase(Criterion cr)
        //{
        //    XmlElement criterion1 = earl.CreateElement("earl", "TestCase", nsmgr.LookupNamespace("earl"));
        //    criterion1.SetAttributeNode("about", nsmgr.LookupNamespace("rdf"));
        //    criterion1.SetAttribute("about", baseUri + "#" + cr.Id);
        //    root.AppendChild(criterion1);

        //    XmlElement desc = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
        //    desc.AppendChild(earl.CreateTextNode(cr.Desc.Normalize()));
        //    criterion1.AppendChild(desc);

        //    XmlElement title = earl.CreateElement("dct", "title", nsmgr.LookupNamespace("dct"));
        //    title.AppendChild(earl.CreateTextNode(cr.Name.Normalize()));
        //    criterion1.AppendChild(title);

        //    XmlElement isPart = earl.CreateElement("dct", "isPartOf", nsmgr.LookupNamespace("dct"));
        //    isPart.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
        //    isPart.SetAttribute("resource", baseUri + "#" + cr.CA.Id);
        //    criterion1.AppendChild(isPart);

        //    XmlElement hasPart = earl.CreateElement("dct", "hasPart", nsmgr.LookupNamespace("dct"));
        //    hasPart.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
        //    hasPart.SetAttribute("resource", baseUri + "#" + cr.Id);
        //    //System.Console.WriteLine("@about = \"" + baseUri + "#" + cr.CA.Id + "\"");
        //    earl.SelectSingleNode("//earl:TestRequirement[@rdf:about='"+baseUri +"#" + cr.CA.Id + "']", nsmgr).AppendChild(hasPart);



        //    createItemTestCases(cr, cr.Rec.Items);
            

            
        //}
        
        private void createItemTestCases(Criterion cr, List<Item> list)
        {
            //int z = 0;
            //foreach (Item i in list) { 

            //XmlElement item = earl.CreateElement("earl", "TestCase", nsmgr.LookupNamespace("earl"));
            //item.SetAttributeNode("about", nsmgr.LookupNamespace("rdf"));
            //item.SetAttribute("about", baseUri + "#" + cr.Id +"_item"+z);
            //root.AppendChild(item);

            //XmlElement desc = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
            //desc.AppendChild(earl.CreateTextNode(i.Desc.Normalize()));
            //item.AppendChild(desc);

            //XmlElement title = earl.CreateElement("dct", "title", nsmgr.LookupNamespace("dct"));
            //title.AppendChild(earl.CreateTextNode(z.ToString().Normalize()));
            //item.AppendChild(title);

            //XmlElement isPart = earl.CreateElement("dct", "isPartOf", nsmgr.LookupNamespace("dct"));
            //isPart.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
            //isPart.SetAttribute("resource", baseUri + "#" + cr.Id);
            //item.AppendChild(isPart);

            //XmlElement hasPart = earl.CreateElement("dct", "hasPart", nsmgr.LookupNamespace("dct"));
            //hasPart.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
            //hasPart.SetAttribute("resource", baseUri + "#" + cr.Id +"_item" +z);
            ////System.Console.WriteLine("@about = \"" + baseUri + "#" + cr.CA.Id + "\"");
            //earl.SelectSingleNode("//earl:TestCase[@rdf:about='" + baseUri + "#" + cr.Id + "']", nsmgr).AppendChild(hasPart);
            
                
            //z++;
            //}
        }
        //creates RESULT classes for criterions type "all"
        public void createAllItemResult(Criterion cr, int index, int erg)
        {
            XmlElement result = earl.CreateElement("earl", "TestResult", nsmgr.LookupNamespace("earl"));
            result.SetAttributeNode("ID", nsmgr.LookupNamespace("rdf"));
            result.SetAttribute("ID", "result_" + cr.Id + "_item" + index);
            root.AppendChild(result);


            XmlElement title = earl.CreateElement("dct", "title", nsmgr.LookupNamespace("dct"));
            title.AppendChild(earl.CreateTextNode(cr.Rec.Items[index].Desc.Normalize()));
            result.AppendChild(title);

            String varText = createVarText(cr.Rec.Items[index]).Normalize();
            //XmlElement desc = earl.CreateElement("dct","description", nsmgr.LookupNamespace("dct"));
            //desc.AppendChild(earl.CreateTextNode(cr.Rec.Items[index].Desc.Normalize()));
            //result.AppendChild(desc);

            switch (erg)
            {
                case 1:
                    XmlElement pass = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
                    pass.AppendChild(earl.CreateTextNode("Der Prüfpunkt : " + cr.Rec.Items[index].Desc.Normalize() + varText+ " wurde erfüllt"));
                    result.AppendChild(pass);

                    XmlElement earlOutcomePass = earl.CreateElement("earl", "outcome", nsmgr.LookupNamespace("earl"));
                    earlOutcomePass.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    earlOutcomePass.SetAttribute("resource", "http://www.w3.org/ns/earl#passed");
                    result.AppendChild(earlOutcomePass);
                    break;
                case 0:
                    XmlElement fail = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
                    fail.AppendChild(earl.CreateTextNode("Prüfpunkt: " + cr.Rec.Items[index].Desc.Normalize() + varText + " wurde nicht erfüllt"));
                    result.AppendChild(fail);

                    XmlElement earlOutcomeFail = earl.CreateElement("earl", "outcome", nsmgr.LookupNamespace("earl"));
                    earlOutcomeFail.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    earlOutcomeFail.SetAttribute("resource", "http://www.w3.org/ns/earl#failed");
                    result.AppendChild(earlOutcomeFail);
                    break;


            }
            this.createSubAssertion(cr, result);
            //this.createSubAssertion(cr, result);
            




        }
        //creates RESULT classes for criterions type "one"
        public void createOneItemResult(Criterion cr, int index, int erg)
        {
            XmlElement result = earl.CreateElement("earl", "TestResult", nsmgr.LookupNamespace("earl"));
            result.SetAttributeNode("ID", nsmgr.LookupNamespace("rdf"));
            result.SetAttribute("ID", "result_" + cr.Id + "_item" + index);
            root.AppendChild(result);


            XmlElement title = earl.CreateElement("dct", "title", nsmgr.LookupNamespace("dct"));
            title.AppendChild(earl.CreateTextNode(cr.Rec.Items[index].Desc.Normalize()));
            result.AppendChild(title);

            //XmlElement desc = earl.CreateElement("dct","description", nsmgr.LookupNamespace("dct"));
            //desc.AppendChild(earl.CreateTextNode(cr.Rec.Items[index].Desc.Normalize()));
            //result.AppendChild(desc);
            String varText = createVarText(cr.Rec.Items[index]).Normalize();

            switch (erg)
            {
                case 1:
                    XmlElement pass = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
                    pass.AppendChild(earl.CreateTextNode("Der Prüfpunkt : " + cr.Rec.Items[index].Desc.Normalize() + varText +  " wurde erfüllt"));
                    result.AppendChild(pass);

                    XmlElement earlOutcomePass = earl.CreateElement("earl", "outcome", nsmgr.LookupNamespace("earl"));
                    earlOutcomePass.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    earlOutcomePass.SetAttribute("resource", "http://www.w3.org/ns/earl#passed");
                    result.AppendChild(earlOutcomePass);
                    break;
                case 0:
                    XmlElement fail = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
                    fail.AppendChild(earl.CreateTextNode("Der Prüfpunkt: " + cr.Rec.Items[index].Desc.Normalize() + varText +" wurde nicht erfüllt"));
                    result.AppendChild(fail);

                    XmlElement earlOutcomeFail = earl.CreateElement("earl", "outcome", nsmgr.LookupNamespace("earl"));
                    earlOutcomeFail.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    earlOutcomeFail.SetAttribute("resource", "http://www.w3.org/ns/earl#failed");
                    result.AppendChild(earlOutcomeFail);
                    break;


            }
            this.createSubAssertion(cr, result);

            //INode result = earlGraph.CreateUriNode(":" + cr.CA.Id + "/" + cr.Id + "/result/item" + index);


            //earlGraph.Assert(result, rdfType, earlGraph.CreateUriNode("earl:TestResult"));
            //earlGraph.Assert(result, earlGraph.CreateUriNode("dct:title"), earlGraph.CreateLiteralNode(cr.Rec.Items[index].Desc.Normalize()));
            //switch (erg)
            //{

            //    case 1:
            //        earlGraph.Assert(result, earlGraph.CreateUriNode("earl:info"), earlGraph.CreateLiteralNode("Die Empfehlung : " + cr.Rec.Items[index].Desc.Normalize() + " wurde erfüllt"));
            //        earlGraph.Assert(result, earlGraph.CreateUriNode("earl:outcome"), earlGraph.CreateUriNode("earl:pass"));
            //        break;
            //    case 0:
            //        earlGraph.Assert(result, earlGraph.CreateUriNode("earl:info"), earlGraph.CreateLiteralNode("Die Empfehlung : " + cr.Rec.Items[index].Desc.Normalize() + " wurde nicht erfüllt"));
            //        earlGraph.Assert(result, earlGraph.CreateUriNode("earl:outcome"), earlGraph.CreateUriNode("earl:fail"));
            //        break;
            //}
            //this.createSubAssertion(cr, result);


        }
        //creates RESULT classes for criterions type "Count"
        public void createCountItemResult(Criterion cr, int index, int erg)
        {

            XmlElement result = earl.CreateElement("earl", "TestResult", nsmgr.LookupNamespace("earl"));
            result.SetAttributeNode("ID", nsmgr.LookupNamespace("rdf"));
            result.SetAttribute("ID", "result_" + cr.Id + "_item" + index);
            root.AppendChild(result);


            XmlElement title = earl.CreateElement("dct", "title", nsmgr.LookupNamespace("dct"));
            title.AppendChild(earl.CreateTextNode(cr.Rec.Items[index].Desc.Normalize()));
            result.AppendChild(title);

            String varText = createVarText(cr.Rec.Items[index]).Normalize();
            //XmlElement desc = earl.CreateElement("dct","description", nsmgr.LookupNamespace("dct"));
            //desc.AppendChild(earl.CreateTextNode(cr.Rec.Items[index].Desc.Normalize()));
            //result.AppendChild(desc);

            switch (erg)
            {
                case 1:
                    XmlElement pass = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
                    pass.AppendChild(earl.CreateTextNode("Der Prüfpunkt : " + cr.Rec.Items[index].Desc.Normalize() + varText+ " wurde erfüllt"));
                    result.AppendChild(pass);

                    XmlElement earlOutcomePass = earl.CreateElement("earl", "outcome", nsmgr.LookupNamespace("earl"));
                    earlOutcomePass.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    earlOutcomePass.SetAttribute("resource", "http://www.w3.org/ns/earl#passed");
                    result.AppendChild(earlOutcomePass);
                    break;
                case 0:
                    XmlElement fail = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
                    fail.AppendChild(earl.CreateTextNode("Der Prüfpunkt : " + cr.Rec.Items[index].Desc.Normalize() + varText + " wurde nicht erfüllt"));
                    result.AppendChild(fail);

                     XmlElement earlOutcomeFail = earl.CreateElement("earl", "outcome", nsmgr.LookupNamespace("earl"));
                    earlOutcomeFail.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    earlOutcomeFail.SetAttribute("resource", "http://www.w3.org/ns/earl#failed");
                    result.AppendChild(earlOutcomeFail);
                    break;


            }
            this.createSubAssertion(cr, result);

            //INode result = earlGraph.CreateUriNode(":" + cr.CA.Id + "/" + cr.Id + "/result/item" + index);


            //earlGraph.Assert(result, rdfType, earlGraph.CreateUriNode("earl:TestResult"));
            //earlGraph.Assert(result, earlGraph.CreateUriNode("dct:title"), earlGraph.CreateLiteralNode(cr.Rec.Items[index].Desc.Normalize()));
            //switch (erg)
            //{

            //    case 1:
            //        earlGraph.Assert(result, earlGraph.CreateUriNode("earl:info"), earlGraph.CreateLiteralNode("Die Empfehlung : " + cr.Rec.Items[index].Desc.Normalize() + " wurde erfüllt"));
            //        earlGraph.Assert(result, earlGraph.CreateUriNode("earl:outcome"), earlGraph.CreateUriNode("earl:pass"));
            //        break;
            //    case 0:
            //        earlGraph.Assert(result, earlGraph.CreateUriNode("earl:info"), earlGraph.CreateLiteralNode("Die Empfehlung : " + cr.Rec.Items[index].Desc.Normalize() + " wurde nicht erfüllt"));
            //        earlGraph.Assert(result, earlGraph.CreateUriNode("earl:outcome"), earlGraph.CreateUriNode("earl:fail"));
            //        break;
            //}
            //this.createSubAssertion(cr, result);
        }
        //creates RESULT classes for criterions type "Rating" which pass
        public void createRatingResultPass(Criterion cr, float erg, int index)
        {
            XmlElement result = earl.CreateElement("earl", "TestResult", nsmgr.LookupNamespace("earl"));
            result.SetAttributeNode("ID", nsmgr.LookupNamespace("rdf"));
            result.SetAttribute("ID", "result_" + cr.Id + "_item" + index);
            root.AppendChild(result);


            XmlElement title = earl.CreateElement("dct", "title", nsmgr.LookupNamespace("dct"));
            title.AppendChild(earl.CreateTextNode(cr.Rec.Items[index].Desc));
            result.AppendChild(title);

            //XmlElement desc = earl.CreateElement("dct","description", nsmgr.LookupNamespace("dct"));
            //desc.AppendChild(earl.CreateTextNode(cr.Rec.Items[index].Desc.Normalize()));
            //result.AppendChild(desc);

           
                    XmlElement pass = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
                    pass.AppendChild(earl.CreateTextNode("Das Rating wurde mit dem Wert " + erg + " bestanden"));
                    result.AppendChild(pass);
                    
                    

                    XmlElement earlOutcomePass = earl.CreateElement("earl", "outcome", nsmgr.LookupNamespace("earl"));
                    earlOutcomePass.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    earlOutcomePass.SetAttribute("resource", "http://www.w3.org/ns/earl#passed");
                    result.AppendChild(earlOutcomePass);
                  
            this.createSubAssertion(cr, result);

            //INode result = earlGraph.CreateUriNode(":" + cr.CA.Id + "/" + cr.Id + "/result/itemrating");

            //earlGraph.Assert(result, rdfType, earlGraph.CreateUriNode("earl:TestResult"));
            //earlGraph.Assert(result, earlGraph.CreateUriNode("earl:info"), earlGraph.CreateLiteralNode("Das Rating wurde mit dem Wert " + erg + " bestanden"));
            //earlGraph.Assert(result, earlGraph.CreateUriNode("earl:outcome"), earlGraph.CreateUriNode("earl:pass"));

            //createSubAssertion(cr, result);
        }
        //creates RESULT classes for criterions type "Rating" which fail
        public void createRatingResultFail(Criterion cr, float erg, int index)
        {
            XmlElement result = earl.CreateElement("earl", "TestResult", nsmgr.LookupNamespace("earl"));
            result.SetAttributeNode("ID", nsmgr.LookupNamespace("rdf"));
            result.SetAttribute("ID", "result_" + cr.Id + "_item" + index);
            root.AppendChild(result);


            XmlElement title = earl.CreateElement("dct", "title", nsmgr.LookupNamespace("dct"));
            title.AppendChild(earl.CreateTextNode(cr.Rec.Items[index].Desc));
            result.AppendChild(title);

            //XmlElement desc = earl.CreateElement("dct","description", nsmgr.LookupNamespace("dct"));
            //desc.AppendChild(earl.CreateTextNode(cr.Rec.Items[index].Desc.Normalize()));
            //result.AppendChild(desc);


            XmlElement fail = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
            fail.AppendChild(earl.CreateTextNode("Das Rating wurde mit dem Wert " + erg + " nicht bestanden"));
            result.AppendChild(fail);


            XmlElement earlOutcomeFail = earl.CreateElement("earl", "outcome", nsmgr.LookupNamespace("earl"));
            earlOutcomeFail.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
            earlOutcomeFail.SetAttribute("resource", "http://www.w3.org/ns/earl#failed");
            result.AppendChild(earlOutcomeFail);

            this.createSubAssertion(cr, result);

            //INode result = earlGraph.CreateUriNode(":" + cr.CA.Id + "/" + cr.Id + "/result/itemrating");

            //earlGraph.Assert(result, rdfType, earlGraph.CreateUriNode("earl:TestResult"));
            //earlGraph.Assert(result, earlGraph.CreateUriNode("earl:info"), earlGraph.CreateLiteralNode("Das Rating wurde mit dem Wert " + erg + " nicht bestanden"));
            //earlGraph.Assert(result, earlGraph.CreateUriNode("earl:outcome"), earlGraph.CreateUriNode("earl:fail"));

        }
        //final Result ob Criterion bestanden oder nicht 
        public void createCriterionResult(Criterion cr, bool pass , float evaluationValue)
        {
            
            XmlElement result = earl.CreateElement("earl", "TestResult", nsmgr.LookupNamespace("earl"));
            result.SetAttributeNode("ID", nsmgr.LookupNamespace("rdf"));
            result.SetAttribute("ID", "result_" + cr.Id);
            root.AppendChild(result);
            

            if (cr.Comment != String.Empty)
            {
                XmlElement title = earl.CreateElement("earl", "info", nsmgr.LookupNamespace("earl"));
                title.AppendChild(earl.CreateTextNode(cr.Comment));
                result.AppendChild(title);
            }
            
            
            switch (pass)
            {
                case true:
                    XmlElement passed = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
                    passed.AppendChild(earl.CreateTextNode(("Das Kriterium: " + cr.Name + " wurde erfüllt." + createErgPassString(cr, evaluationValue)).Normalize()));
                    result.AppendChild(passed);

                    XmlElement earlOutcomePass = earl.CreateElement("earl", "outcome", nsmgr.LookupNamespace("earl"));
                    earlOutcomePass.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    earlOutcomePass.SetAttribute("resource", "http://www.w3.org/ns/earl#passed");
                    result.AppendChild(earlOutcomePass);
                    break;
                case false:
                    XmlElement warning = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
                    warning.AppendChild(earl.CreateTextNode(("Das Kriterium: " + cr.Name.Normalize() + " wurde nicht erfüllt. "+ createErgFailString(cr, evaluationValue)).Normalize()));
                    result.AppendChild(warning);

                    XmlElement earlOutcomeWarning = earl.CreateElement("earl", "outcome", nsmgr.LookupNamespace("earl"));
                    earlOutcomeWarning.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    earlOutcomeWarning.SetAttribute("resource", "http://www.w3.org/ns/earl#failed");
                    result.AppendChild(earlOutcomeWarning);
                    break;
               
            }
            createAssertionCriterion(cr, result);
            
        }

        

        
        
        private void createAssertionCriterion(Criterion cr, XmlElement result)
        {
            XmlElement test = earl.CreateElement("earl", "Assertion", nsmgr.LookupNamespace("earl"));
            test.SetAttributeNode("ID", nsmgr.LookupNamespace("rdf"));
            test.SetAttribute("ID", "finalassertion_"+cr.Id);
            root.AppendChild(test);

            XmlElement tester = earl.CreateElement("earl", "assertedBy", nsmgr.LookupNamespace("earl"));
            tester.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
            //var a = testPerson.GetAttribute("about", nsmgr.LookupNamespace("rdf"));
            tester.SetAttribute("resource","#"+ group.GetAttribute("ID", nsmgr.LookupNamespace("rdf")));
            test.AppendChild(tester);

            XmlElement subject = earl.CreateElement("earl", "subject", nsmgr.LookupNamespace("earl"));
            subject.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
            //var a = testPerson.GetAttribute("about", nsmgr.LookupNamespace("rdf"));
            subject.SetAttribute("resource","#"+ testsubject.GetAttribute("ID", nsmgr.LookupNamespace("rdf")));
            test.AppendChild(subject);

            XmlElement test1 = earl.CreateElement("earl", "test", nsmgr.LookupNamespace("earl"));
            test1.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
            //var a = testPerson.GetAttribute("about", nsmgr.LookupNamespace("rdf"));
            test1.SetAttribute("resource", baseUri + "#" + cr.Id);
            test.AppendChild(test1);

            XmlElement res = earl.CreateElement("earl", "result", nsmgr.LookupNamespace("earl"));
            res.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
            //var a = testPerson.GetAttribute("about", nsmgr.LookupNamespace("rdf"));
            res.SetAttribute("resource","#"+ result.GetAttribute("ID", nsmgr.LookupNamespace("rdf")));
            test.AppendChild(res);
            
        }




        private void createSubAssertion(Criterion cr, XmlElement result)
        {
            
            int i = 0;
            //sucht nach freiem platz für tesKnoten
            while (i < 20)
            {
                //var a = earl.SelectSingleNode("//earl:Assertion[@rdf:about='" + baseUri + "#" + cr.Id + "_test_item" + i + "']", nsmgr);
                if (earl.SelectSingleNode("//earl:Assertion[@rdf:ID='"+ "assertion" + i+ "_"  + cr.Id + "']", nsmgr) == null)
                {
                    XmlElement test = earl.CreateElement("earl", "Assertion", nsmgr.LookupNamespace("earl"));
                    test.SetAttributeNode("ID", nsmgr.LookupNamespace("rdf"));
                    test.SetAttribute("ID", "assertion" + i+ "_"  + cr.Id);
                    root.AppendChild(test);

                    XmlElement tester = earl.CreateElement("earl", "assertedBy", nsmgr.LookupNamespace("earl"));
                    tester.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    //var a = testPerson.GetAttribute("about", nsmgr.LookupNamespace("rdf"));
                    tester.SetAttribute("resource", "#" + group.GetAttribute("ID",nsmgr.LookupNamespace("rdf")));
                    test.AppendChild(tester);

                    XmlElement subject = earl.CreateElement("earl", "subject", nsmgr.LookupNamespace("earl"));
                    subject.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    //var a = testPerson.GetAttribute("about", nsmgr.LookupNamespace("rdf"));
                    subject.SetAttribute("resource","#"+ testsubject.GetAttribute("ID", nsmgr.LookupNamespace("rdf")));
                    test.AppendChild(subject);

                    XmlElement test1 = earl.CreateElement("earl", "test", nsmgr.LookupNamespace("earl"));
                    test1.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    //var a = testPerson.GetAttribute("about", nsmgr.LookupNamespace("rdf"));
                    test1.SetAttribute("resource", baseUri + "#" + cr.Id);
                    test.AppendChild(test1);

                    XmlElement res  = earl.CreateElement("earl", "result", nsmgr.LookupNamespace("earl"));
                    res.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    //var a = testPerson.GetAttribute("about", nsmgr.LookupNamespace("rdf"));
                    res.SetAttribute("resource","#"+ result.GetAttribute("ID", nsmgr.LookupNamespace("rdf")));
                    test.AppendChild(res);

                    break;
                }
                i++;
            }

      
        }
        public void testFunction()
        {
     

        }

        public void writeEarlRdf()
        {



            string fullPath = typeof(tud.mci.tangram.qsdialog.Dialogs.UnoDialogSample).Assembly.Location;
            string baseDir = System.IO.Path.GetDirectoryName(fullPath);
            earl.Save(baseDir+"\\document.rdf");

        }





        internal void createRequirementResult(Category ca, bool pass)
        {

            XmlElement result = earl.CreateElement("earl", "TestResult", nsmgr.LookupNamespace("earl"));
            result.SetAttributeNode("ID", nsmgr.LookupNamespace("rdf"));
            result.SetAttribute("ID", "result_" + ca.Id);
            root.AppendChild(result);

           

            switch (pass)
            {
                case true:
                    XmlElement passed = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
                    passed.AppendChild(earl.CreateTextNode("Die Kategorie: " + ca.Name.Normalize() + " wurde erfüllt"));
                    result.AppendChild(passed);

                    XmlElement earlOutcomePass = earl.CreateElement("earl", "outcome", nsmgr.LookupNamespace("earl"));
                    earlOutcomePass.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    earlOutcomePass.SetAttribute("resource", "http://www.w3.org/ns/earl#passed");
                    result.AppendChild(earlOutcomePass);
                    break;

                case false:
                    XmlElement fail = earl.CreateElement("dct", "description", nsmgr.LookupNamespace("dct"));
                    fail.AppendChild(earl.CreateTextNode("Die Kategorie: " + ca.Name.Normalize() + " wurde nicht erfüllt"));
                    result.AppendChild(fail);

                    XmlElement earlOutcomeFail = earl.CreateElement("earl", "outcome", nsmgr.LookupNamespace("earl"));
                    earlOutcomeFail.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
                    earlOutcomeFail.SetAttribute("resource", "http://www.w3.org/ns/earl#failed");
                    result.AppendChild(earlOutcomeFail);
                    break;
            }

            createAssertionCategorie(ca, result);

        }

        private void createAssertionCategorie(Category ca, XmlElement result)
        {

            XmlElement test = earl.CreateElement("earl", "Assertion", nsmgr.LookupNamespace("earl"));
            test.SetAttributeNode("about", nsmgr.LookupNamespace("rdf"));
            test.SetAttribute("about", "finalassertion_" + ca.Id);
            root.AppendChild(test);

            XmlElement tester = earl.CreateElement("earl", "assertedBy", nsmgr.LookupNamespace("earl"));
            tester.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
            //var a = testPerson.GetAttribute("about", nsmgr.LookupNamespace("rdf"));
            tester.SetAttribute("resource","#"+ group.GetAttribute("ID", nsmgr.LookupNamespace("rdf")));
            test.AppendChild(tester);

            XmlElement subject = earl.CreateElement("earl", "subject", nsmgr.LookupNamespace("earl"));
            subject.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
            //var a = testPerson.GetAttribute("about", nsmgr.LookupNamespace("rdf"));
            subject.SetAttribute("resource","#"+ testsubject.GetAttribute("ID", nsmgr.LookupNamespace("rdf")));
            test.AppendChild(subject);

            XmlElement test1 = earl.CreateElement("earl", "test", nsmgr.LookupNamespace("earl"));
            test1.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
            //var a = testPerson.GetAttribute("about", nsmgr.LookupNamespace("rdf"));
            test1.SetAttribute("resource", baseUri + "#" + ca.Id);
            test.AppendChild(test1);

            XmlElement res = earl.CreateElement("earl", "result", nsmgr.LookupNamespace("earl"));
            res.SetAttributeNode("resource", nsmgr.LookupNamespace("rdf"));
            //var a = testPerson.GetAttribute("about", nsmgr.LookupNamespace("rdf"));
            res.SetAttribute("resource","#"+ result.GetAttribute("ID", nsmgr.LookupNamespace("rdf")));
            test.AppendChild(res);
           
        }





        //Zusatzfunktionen
        private String createErgPassString(Criterion cr, float evaluationValue )
        {
            String erg = "";
            switch(cr.Rec.Type){
                case CriterionType.all:
                    if (cr.Res.Rating.Count == 1)
                    {
                        erg = " Es wurde " + evaluationValue + " von " + cr.Rec.Items.Count + "Prüfpunkt erfüllt.";
                    }
                    else{
                        erg = " Es wurden " + evaluationValue*cr.Rec.Items.Count + " von " + cr.Rec.Items.Count + " Prüfpunkte erfüllt."; 
                        }
                    break;
                case CriterionType.count:
                    String pruef = " Prüfpunkte";
                    if (new Parameters().PassCount * cr.Rec.Items.Count == 1) pruef = " Prüfpunkt";
                    erg = " Es wurden " + evaluationValue * cr.Rec.Items.Count + " von " + cr.Rec.Items.Count + " Prüfpunkte erfüllt. Zum bestehen dieses Kriteriums benötigte es " + new Parameters().PassCount * cr.Rec.Items.Count + pruef +".";
                    break;
                case CriterionType.rating:
                    erg = ("(Bewertung: " + evaluationValue + "%)");
                    break;
            }
            return erg;
        }


        private string createErgFailString(Criterion cr, float evaluationValue)
        {
            String erg = "";
            switch(cr.Rec.Type){
                case CriterionType.all:
                    if (cr.Res.Rating.Count == 1)
                    {
                        erg = " Es wurde " + evaluationValue + " von " + cr.Rec.Items.Count + " Prüfpunkt erfüllt.";
                    }
                    else{
                        erg = " Es wurden " + evaluationValue*cr.Rec.Items.Count + " von " + cr.Rec.Items.Count + " Prüfpunkte erfüllt. Es hätten alle bestanden werden müssen."; 
                        }
                    break;
                case CriterionType.count:
                    String pruef = " Prüfpunkte";
                    if (new Parameters().PassCount * cr.Rec.Items.Count == 1) pruef = " Prüfpunkt";
                    erg = " Es wurden " + evaluationValue * cr.Rec.Items.Count + " von " + cr.Rec.Items.Count + " Prüfpunkte erfüllt. Zum bestehen dieses Kriteriums benötigte es " + new Parameters().PassCount * cr.Rec.Items.Count + pruef +".";
                    break;
                case CriterionType.rating:
                    erg = ("(Bewertung: " + evaluationValue +"%)");
                    break;
            }
            return erg;
        }
        //muss ausgelagert werden
        private String createVarText(Item i)
        {
            if (i.Var.Count == 0)
            {
                i.VarText = "";
                return "";
            }
            else
            {
                String varText = "";
                foreach (Variable va in i.Var)
                {
                    if (va.MediaType.Equals(mediaType))
                    {

                        if (va.Min != null)
                        {
                            varText += " minimal: " + va.Min;
                        }
                        if (va.Max != null)
                        {
                            varText += " maximal: " + va.Max;
                        }
                        if (va.Value != null)
                        {
                            varText += " Value : " + va.Value;
                        }


                        i.VarText = varText;
                        return varText;
                    }

                }
                if (varText.Equals(String.Empty))
                {

                    foreach (Variable va in i.Var)
                    {
                        if (va.MediaType.Equals(MediaType.All))
                        {

                            if (va.Min != null)
                            {
                                varText += " minimal: " + va.Min;
                            }
                            if (va.Max != null)
                            {
                                varText += " maximal: " + va.Max;
                            }
                            if (va.Value != null)
                            {
                                varText += " Value : " + va.Value;
                            }


                            i.VarText = varText;
                            return varText;
                        }
                    }
                }
            }
            return null;
        }
    }
}
