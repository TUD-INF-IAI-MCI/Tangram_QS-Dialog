using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using tud.mci.tangram.qsdialog.models;


namespace tud.mci.tangram.qsdialog.models
{
    static class Parser
    {

        public static List<Category> AllCategories = new List<Category>();
        public static Dictionary<String,Category> AllCategoriesDict = new Dictionary<String,Category>();

        /// <summary>
        /// Gets the base directory of the current running application.
        /// </summary>
        /// <returns></returns>
        public static string GetBaseDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static List<Category> Parse()
        {
            return Parse(GetBaseDir() + @"\Test\XMLFile1.xml");
        }

        /// <summary>
        /// Parses the specified criteria catalog at the given path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static List<Category> Parse(string path)
        {

            List<Category> categories = new List<Category>();
            Dictionary<String, Category> catDict = new Dictionary<string, Category>();

            if (!System.IO.File.Exists(path)) return categories;

            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(path);

                XmlNodeList nodes = xDoc.DocumentElement.SelectNodes("category");


                foreach (XmlNode node in nodes)
                {
                    if (node == null) continue;
                    Category ca = new Category();
                    List<Criterion> crlist = new List<Criterion>();
                    XmlNodeList criterions = node.SelectNodes("criterion");

                    foreach (XmlNode node2 in criterions)
                    {
                        if (node2 == null) continue;

                        Recommendation rec = new Recommendation();

                        var helpNode7 = node2.SelectSingleNode("recommendation");
                        if (helpNode7 != null)
                        {
                            var helpNode8 = helpNode7.Attributes["type"];
                            CriterionType type = CriterionType.unknown;
                            if (helpNode8 != null)
                            {
                                try
                                {
                                    if (Enum.IsDefined(typeof(CriterionType), helpNode8.Value))
                                    {
                                        type = (CriterionType)Enum.Parse(typeof(CriterionType), helpNode8.Value);

                                    }
                                }
                                catch { }
                                rec.Type = type;
                            }
                        }
                        Criterion cr = new Criterion(rec);

                        var helpNode = node2.SelectSingleNode("help");
                        if (helpNode != null)
                        {
                            cr.Help = helpNode.InnerText;
                        }

                        var helpNode2 = node2.Attributes["name"];
                        if (helpNode2 != null)
                        {
                            cr.Name = helpNode2.Value;
                        }

                        var helpNode3 = node2.Attributes["id"];
                        if (helpNode3 != null)
                        {
                            cr.Id = helpNode3.Value;
                        }

                        var helpNode4 = node2.Attributes["mode"];
                        if (helpNode4 != null)
                        {
                            cr.Mode = helpNode4.Value;
                        }

                        var helpNode5 = node2.Attributes["priority"];
                        if (helpNode5 != null)
                        {
                            cr.Priority = Convert.ToInt32(helpNode5.Value);
                        }
                        var helpNode6 = node2.Attributes["relation"];
                        if (helpNode6 != null)
                        {
                            cr.Relation = helpNode6.Value;
                        }


                        var helpNode0 = node2.SelectSingleNode("desc");
                        if (helpNode0 != null)
                        {
                            cr.Desc = helpNode0.InnerText;

                        }

                        XmlNodeList items = node2.SelectSingleNode("recommendation").SelectNodes("item");
                        foreach (XmlNode node3 in items)
                        {

                            if (node3 == null) continue;

                            Item i = new Item();
                            var helpNode30 = node3.Attributes["role"];
                            if (helpNode30 != null)
                            {
                                i.Role = helpNode30.Value;
                                if (helpNode30.Value == "rating")
                                {
                                    cr.Rec.Type = CriterionType.rating;

                                }

                            }

                            var helpNode9 = node3.SelectSingleNode("desc");
                            if (helpNode9 != null)
                            {
                                i.Desc = helpNode9.InnerText;
                            }

                            XmlNodeList vars = node3.SelectNodes("var");
                            if (vars.Count == 0)
                            {
                                cr.Rec.Items.Add(i);

                            }
                            else
                            {
                                foreach (XmlNode var in vars)
                                {
                                    Variable v = new Variable();


                                    var helpNode10 = var.Attributes["name"];
                                    if (helpNode10 != null)
                                    {
                                        v.Name = helpNode10.Value;
                                    }

                                    var helpNode11 = var.Attributes["max"];
                                    if (helpNode11 != null)
                                    {
                                        v.Max = helpNode11.Value;
                                    }

                                    var helpNode12 = var.Attributes["min"];
                                    if (helpNode12 != null)
                                    {
                                        v.Min = helpNode12.Value;
                                    }

                                    var helpNode13 = var.Attributes["value"];
                                    if (helpNode13 != null)
                                    {
                                        v.Value = helpNode13.Value;
                                    }

                                    var helpNode14 = var.Attributes["media"];
                                    if (helpNode14 != null)
                                    {
                                        if (helpNode14.Value.Equals("all")) v.MediaType = MediaType.All;
                                        if (helpNode14.Value.Equals("Tiger")) v.MediaType = MediaType.Tiger;
                                        if (helpNode14.Value.Equals("Schwellpapier")) v.MediaType = MediaType.Schwellpapier;
                                    }
                                    //System.Console.WriteLine(v.Value + v.Max + v.Min);
                                    i.Var.Add(v);
                                    //TODO: IS not the best solution because when a item has more then one variable, it will repeatedly added
                                    

                                }
                                cr.Rec.Items.Add(i);
                            }
                        }
                        
                        //System.Console.WriteLine(cr.Id);
                        //System.Console.WriteLine(cr.Priority);
                        cr.CA = ca;
                        cr.setResult();
                        crlist.Add(cr);
                    }

                    var helpNode33 = node.Attributes["name"];
                    if (helpNode33 != null)
                    {
                        ca.Name = helpNode33.Value;
                    }

                    var helpNode15 = node.Attributes["id"];
                    if (helpNode15 != null)
                    {
                        ca.Id = helpNode15.Value;
                    }

                    var helpNode16 = node.SelectSingleNode("desc");
                    if (helpNode16 != null)
                    {
                        ca.Desc = helpNode16.InnerText;
                    }

                    var helpNode17 = node.SelectSingleNode("help");
                    if (helpNode17 != null)
                    {
                        ca.Help = helpNode17.InnerText;
                    }

                    ca.Criteria = crlist;
                    categories.Add(ca);
                    catDict.Add(ca.Id, ca);

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception in criteria parsing:\r\n" + ex);
            }

            AllCategories = categories;
            AllCategoriesDict = catDict;
            return categories;
        }

        public static Dictionary<String, Criterion> GetAllCriteria()
        {
            List<Category> caList = AllCategories;
            Dictionary<String,Criterion> crList = new Dictionary<String, Criterion>();

            foreach (Category ca in caList)
            {
                List<Criterion> crHelpList = ca.Criteria;
                foreach (Criterion cr in crHelpList)
                {
                    crList.Add(cr.Id, cr);
                }
            }

            return crList;

        }
        //give you the maximum count of Items a criterion have
        public static int GetMaxCountOfItems()
        {
            int maxCount = 0;
            Dictionary<String,Criterion> crList = GetAllCriteria();
            foreach (Criterion cr in crList.Values)
            {
                if (cr.getCountOfItems() > maxCount)
                {
                    maxCount = cr.getCountOfItems();
                }
            }
            return maxCount;
        }

        public static int GetMaxCountOfItemsTypOne()
        {
            int maxCount = 0;
            //List<Criterion>crListTypeOne = new List<Criterion>();
            Dictionary<String, Criterion> crList = GetAllCriteria();
            foreach (Criterion cr in crList.Values)
            {
                if (cr.Rec.Type == CriterionType.one)
                {
                    if (cr.Rec.Items.Count > maxCount) { maxCount = cr.Rec.Items.Count; }
                }
            }
            return maxCount;
        }
        public static int GetMaxCountOfRatingItems()
        {
            int maxCount = 0;
            Dictionary<String, Criterion> crList = GetAllCriteria();
            foreach (Criterion cr in crList.Values)
            {
                int countForCriterium = 0;
                foreach (Item i in cr.Rec.Items)
                {
                    if (i.Role == "rating") { countForCriterium++; }
                }
                if (countForCriterium > maxCount) { maxCount = countForCriterium; }
            }
            return maxCount;
        }


    }
}
