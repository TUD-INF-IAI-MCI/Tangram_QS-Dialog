using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tud.mci.LanguageLocalization;
using tud.mci.tangram.EARL;
using tud.mci.tangram.qsdialog.models;

namespace tud.mci.tangram.qsdialog
{
    public partial class QsDialog : Form
    {
        #region Member

        LL ll = new LL(Properties.Resources.Language);

        List<Category> categories;
        Dictionary<String, Category> categoriesDict;
        Dictionary<String, Criterion> allcriteria;
        Earl earl;
        Evaluation eval;
        EarlReader earlReader;

        int maxCountOfItemsTypeOne;
        int maxCountOfItemsTypeAll;
        int maxCountOfRatingItems;

        #endregion


        public QsDialog(String catalogPath)
        {
            InitializeComponent();

            initializeCriteria(catalogPath);

            fillTreeView(categories);

        }


        private void initializeCriteria(string p)
        {
            categories = Parser.Parse(p);
            categoriesDict = Parser.AllCategoriesDict;
            allcriteria = Parser.GetAllCriteria();
            earl = new Earl();
            eval = new Evaluation();
            earlReader = new EarlReader();

            maxCountOfItemsTypeOne = Parser.GetMaxCountOfItemsTypOne();
            maxCountOfItemsTypeAll = Parser.GetMaxCountOfItems();
            maxCountOfRatingItems = Parser.GetMaxCountOfRatingItems();
        }

        #region initialization

        private void QsDialog_Load(object sender, EventArgs e)
        {
            #region load language texts

            #endregion


            #region event registering

            if (this.treeView_Steps != null)
            {
                this.treeView_Steps.BeforeSelect += treeView_Steps_BeforeSelect;
                this.treeView_Steps.AfterSelect += treeView_Steps_AfterSelect;
            }


            #endregion


        }

        #region Steps TreeView

        private void fillTreeView(List<Category> categories)
        {
            if (categories != null && categories.Count > 0 && this.treeView_Steps != null)
            {
                this.treeView_Steps.Visible = true;
                int width = this.treeView_Steps.Width;

                // Introduction
                TreeNode introductionNode = new TreeNode();
                introductionNode.NodeFont = topNodeFont;
                //ATTENTION: you have to set the text after setting the font - so the text is readable at all
                introductionNode.Text = ll.GetTrans("tangram.qsdialog.introduction");
                this.treeView_Steps.Nodes.Add(introductionNode);

                #region criteria

                List<TreeNode> catNodes = new List<TreeNode>();

                int c = 0;
                foreach (tud.mci.tangram.qsdialog.models.Category ca in categories)
                {


                    //var catLable = CreateFixedLabel(++c + ". " + ca.Name, 0, 0, 0, 0, 0, this, ca.Id);
                    ////SetProperty(introduction, "Tag", s.ToString()); //TODO: add the tag for the introduction
                    //SetProperty(catLable, "FontHeight", h2_fontHieght);
                    //SetProperty(catLable, "FontWeight", h2_fontWeight);
                    //SetProperty(catLable, "HelpText", ca.Id);
                    //SetProperty(catLable, "MultiLine", true);

                    ////SetProperty(catLable, "HelpURL", 6);
                    //Object t = null;
                    //if (catLable is XControl)
                    //{
                    //    t = sc.AddElementToTheEndAndAdoptTheSize(catLable as XControl, ca.Id, h2_leftMargin, 7);
                    //}
                    //criteriaLabelMap.Add(ca.Id, t);
                    ////Debug.GetAllProperties(catLable);

                    ////insertRoadmapItem(1, false, ca.Name, 1);
                    ////List<Criterion> criteria = ca.Criteria;
                    ////categorie adden
                    //criteriaMap.Add(ca.Id, ca);


                    List<TreeNode> criterionNodes = new List<TreeNode>();
                    int i = 0;
                    foreach (Criterion cr in ca.Criteria)
                    {

                        // criteriaMap.Add(cr.Id, cr);

                        int s = 3;
                        switch (cr.Rec.Type)
                        {

                            case tud.mci.tangram.qsdialog.models.CriterionType.all:
                                s = 1;
                                break;
                            case tud.mci.tangram.qsdialog.models.CriterionType.one:
                                s = 2;
                                break;
                            case tud.mci.tangram.qsdialog.models.CriterionType.count:
                                s = 3;
                                break;
                            case tud.mci.tangram.qsdialog.models.CriterionType.rating:
                                s = 3;
                                break;
                            default:
                                s = 0;
                                break;
                        }


                        TreeNode criterionNode = new TreeNode(ll.GetTrans("tangram.qsdialog.list.entry", cr.Name, cr.Id));
                        criterionNode.NodeFont = crNodeFont;
                        criterionNode.Name = cr.Id;
                        criterionNodes.Add(criterionNode);


                        //var tLabel = CreateFixedLabel(c + "." + ++i + ". " + cr.Name, 0, 0, 0, 0, 0, this, cr.Id);
                        //SetProperty(tLabel, "HelpURL", s.ToString());
                        //SetProperty(tLabel, "HelpText", cr.Id);
                        //SetProperty(tLabel, "MultiLine", true);
                        //Object l = null;

                        //if (tLabel is XControl)
                        //{
                        //    //sc.addElementAndAdoptTheSize(tLabel as XControl, cr.Id, 5, 20 + (i++ * 20));
                        //    l = sc.AddElementToTheEndAndAdoptTheSize(tLabel as XControl, cr.Id, h3_leftMargin, 5);

                        //}

                        //criteriaLabelMap.Add(cr.Id, l);

                        //var t = InsertFixedLabel(cr.Name, 10, 20 + (i++ * 10), 100, 10, 0, this, cr.Id);
                        //SetProperty(t, "HelpURL", s.ToString());

                        //SetProperty(t, "HelpURL", Convert.ToString(index++));
                        //util.Debug.GetAllProperties(t);


                    }


                    TreeNode catNode = new TreeNode(ca.Name, criterionNodes.ToArray());
                    catNode.NodeFont = catNodeFont;
                    catNode.Text = ll.GetTrans("tangram.qsdialog.list.entry", ca.Name, ca.Id);
                    catNode.Name = ca.Id;
                    catNodes.Add(catNode);

                }

                #endregion

                // Criteria
                TreeNode criteriaNode = new TreeNode(String.Empty, catNodes.ToArray());
                criteriaNode.NodeFont = topNodeFont;
                criteriaNode.Text = ll.GetTrans("tangram.qsdialog.criteria");
                this.treeView_Steps.Nodes.Add(criteriaNode);


                // Evaluation
                TreeNode evaluationNode = new TreeNode();
                evaluationNode.NodeFont = topNodeFont;
                evaluationNode.Text = ll.GetTrans("tangram.qsdialog.evaluation");
                
                this.treeView_Steps.Nodes.Add(evaluationNode);

                //adopt the elements line heights and widths
                adoptTreeNodeSizes(this.treeView_Steps);

            }
        }

        private static void adoptTreeNodeSizes(TreeView treeView)
        {
            if (treeView != null && treeView.Nodes != null)
            {

               // treeView.ItemHeight = topNodeElementHeight;
                

            }
        }




        // Microsoft Sans Serif; 8,25pt
        static Font topNodeFont = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
        static Font catNodeFont = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
        static Font crNodeFont = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);

        const int topNodeElementHeight = 12;
        const int catNodeElementHeight = 10;
        const int crNodeElementHeight = 10;

        #region TreeView events

        void treeView_Steps_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e != null && e.Node != null && sender != null && sender is TreeView && ((TreeView)sender).SelectedNode != null)
            {
                if (((TreeView)sender).SelectedNode.NodeFont == null) return;

                ((TreeView)sender).SelectedNode.NodeFont = new Font(((TreeView)sender).SelectedNode.NodeFont,
                     ((TreeView)sender).SelectedNode.NodeFont.Style & ~FontStyle.Underline
                    );
            }
        }

        private void treeView_Steps_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e != null && e.Node != null)
            {
                if (e.Node.NodeFont == null) e.Node.NodeFont = crNodeFont;
                e.Node.NodeFont = new Font(e.Node.NodeFont, e.Node.NodeFont.Style
                    | FontStyle.Underline
                    );
                showCategories(e.Node.Text, e.Node.Name);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Fill dialog elements

        public void showCategories(String name, string UID)
        {
            if (!string.IsNullOrEmpty(UID))
            {
                string catTitle = String.Empty;
                IQsEntry entry = null;
                Criterion cr = null;


                // find category or criteria in map
                if (this.categoriesDict.ContainsKey(UID))
                {
                    Category cat = this.categoriesDict[UID];
                    entry = cat;
                    catTitle = ll.GetTrans("tangram.qsdialog.list.entry", cat.Name, cat.Id);

                }
                else if (allcriteria.ContainsKey(UID))
                {
                    cr = allcriteria[UID];
                    entry = cr;
                    catTitle = (cr != null && cr.CA != null) ? ll.GetTrans("tangram.qsdialog.list.entry", cr.CA.Name, cr.CA.Id) : String.Empty;
                }

                // fill in the general elements
                if (entry != null)
                {
                    setCategoryTitle(catTitle);
                    setCriterionDescription(entry.Desc);
                    setCriterionHelp(entry.Help);
                }

                if (cr != null)
                {
                    setCriterionTitle(ll.GetTrans("tangram.qsdialog.list.entry", cr.Name, cr.Id));
                    setCriterionPriority(ll.GetTrans("tangram.qsdialog.priority", cr.Priority.ToString()));
                }
                else // if this is an category node and not a detailed criterion
                {
                    setCriterionPriorityVisibility(false);
                    setCriterionTitleVisibility(false);
                    setCriterionRatingVisibility(false);
                }


            }
        }

        //TODO: check for invocation

        #region control element value setter

        void setCategoryTitle(String title)
        {

            if (this.label_catTitle != null)
            {
                this.label_catTitle.Text = title;
            }

        }

        void setCriterionTitle(String title)
        {
            if (this.label_critTitle != null)
            {
                setCriterionTitleVisibility(true);
                this.label_critTitle.Text = title;
            }
        }
        void setCriterionTitleVisibility(bool visibility)
        {
            if (this.label_critTitle != null)
            {
                this.label_critTitle.Visible = visibility;
            }
        }

        void setCriterionPriority(String title)
        {
            if (this.label_priority != null)
            {
                setCriterionPriorityVisibility(true);
                this.label_priority.Text = title;
            }
        }
        void setCriterionPriorityVisibility(bool visibility)
        {
            if (this.label_priority != null)
            {
                this.label_priority.Visible = visibility;
            }
        }

        void setCriterionDescription(String desc)
        {
            if (this.growLabel_description != null)
            {
                this.growLabel_description.Text = desc;
            }
        }

        void setCriterionHelp(String helpText)
        {
            if (this.webBrowser_Help != null)
            {

                this.webBrowser_Help.Navigate("about:blank");
                this.webBrowser_Help.Refresh();
                this.webBrowser_Help.Document.Write(string.Empty);

                this.webBrowser_Help.Document.Write(@"<html>
<head>
    <title>Help text for criteria</title>
    <style>
      body { font-family: Helvetica, Arial, Sans-Serif; font-size: 8.5pt;}  
    </style>
  </head>
<body>" + helpText + "</body></html>");
                this.webBrowser_Help.Refresh();

            }
        }

        void setCriterionRating(Criterion cr)
        {
            //TODO

        }

        void setCriterionRatingVisibility(bool visibility)
        {
            if (this.groupBox_Rating != null)
            {
                this.groupBox_Rating.Visible = visibility;
            }
        }

        #endregion

        #endregion

        #region button events

        private void button_Next_Click(object sender, EventArgs e)
        {
            if (this.treeView_Steps != null && treeView_Steps.Visible && this.treeView_Steps.Nodes != null && this.treeView_Steps.Nodes.Count > 0)
            {
                TreeNode nextNode = null;

                if (this.treeView_Steps.SelectedNode != null)
                {
                    // If currently selected node has a child
                    if (this.treeView_Steps.SelectedNode.FirstNode != null)
                    {
                        nextNode = this.treeView_Steps.SelectedNode.FirstNode;
                        this.treeView_Steps.SelectedNode.Expand();
                    }
                    else
                    {
                        //if currently selected node has a sibling
                        if (this.treeView_Steps.SelectedNode.NextNode != null)
                        {
                            nextNode = this.treeView_Steps.SelectedNode.NextNode;
                        }
                        else
                        {
                            // check if the parent node has a sibling
                            TreeNode parent = this.treeView_Steps.SelectedNode.Parent;
                            while (nextNode == null)
                            {
                                if (parent != null)
                                {
                                    if (parent.NextNode != null)
                                    {
                                        nextNode = parent.NextNode;
                                        break;
                                    }
                                    else
                                    {
                                        parent = parent.Parent;
                                    }
                                }
                                else // take the first node again ??? Handle evaluation?
                                {
                                    nextNode = this.treeView_Steps.Nodes[0];
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    //select first node
                    nextNode = this.treeView_Steps.Nodes[0];
                }

                this.treeView_Steps_BeforeSelect(this.treeView_Steps, new TreeViewCancelEventArgs(nextNode, false, TreeViewAction.Unknown));
                this.treeView_Steps.SelectedNode = nextNode;
                this.treeView_Steps_AfterSelect(this.treeView_Steps, new TreeViewEventArgs(this.treeView_Steps.SelectedNode));

            }
        }

        private void button_Prev_Click(object sender, EventArgs e)
        {
            if (this.treeView_Steps != null && treeView_Steps.Visible && this.treeView_Steps.Nodes != null && this.treeView_Steps.Nodes.Count > 0)
            {
                TreeNode prevNode = null;

                if (this.treeView_Steps.SelectedNode != null)
                {
                    //check if node has previous node
                    if (this.treeView_Steps.SelectedNode.PrevNode != null)
                    {
                        prevNode = this.treeView_Steps.SelectedNode.PrevNode;

                        //get last child of the currently selected element
                        while (prevNode != null && prevNode.LastNode != null)
                        {
                            prevNode = prevNode.LastNode;
                        }


                    }
                    else
                    {
                        if (this.treeView_Steps.SelectedNode.Parent != null)
                        {
                            prevNode = this.treeView_Steps.SelectedNode.Parent;
                        }
                        else
                        {
                            //select last node
                            prevNode = this.treeView_Steps.Nodes[this.treeView_Steps.Nodes.Count - 1];
                        }
                    }
                }
                else
                {
                    //select last node
                    prevNode = this.treeView_Steps.Nodes[this.treeView_Steps.Nodes.Count - 1];
                }

                this.treeView_Steps_BeforeSelect(this.treeView_Steps, new TreeViewCancelEventArgs(prevNode, false, TreeViewAction.Unknown));
                this.treeView_Steps.SelectedNode = prevNode;
                this.treeView_Steps_AfterSelect(this.treeView_Steps, new TreeViewEventArgs(this.treeView_Steps.SelectedNode));

            }
        }


        #endregion

    }
}
