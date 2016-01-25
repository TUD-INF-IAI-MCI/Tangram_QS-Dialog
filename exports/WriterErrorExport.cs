using System;
using System.Collections.Generic;
using unoidl.com.sun.star.uno;
using tud.mci.tangram.models.documents;
using tud.mci.tangram.qsdialog.models;

using unoidl.com.sun.star.text;
using tud.mci.tangram.util;
using unoidl.com.sun.star.style;
using unoidl.com.sun.star.container;
using unoidl.com.sun.star.awt;

namespace tud.mci.tangram.exports
{
    class WriterErrorExport : AbstractWriterExportBase
    {

        #region CTOR

        public WriterErrorExport(List<Category> results, Evaluation eval, XComponentContext xContext = null)
            : base(results, eval, xContext)
        {
            this.eval = eval;
            this.results = results;

        }




        public void WriteDocument(String title, String path)
        {

            if (results != null && results.Count > 0)
            {

                // create Doc
                if (WriterDoc != null)
                {
                    PrepareDoc("Fehlerbericht");
                    // TODO: set Title
                    //TABLE


                    // fill with content

                    //Auflistung Fehler
                    int caNumber = 1;
                    //WriterDoc.NewParagraph();
                    if (eval.errorCount > 0)
                    {
                        WriterDoc.NewParagraph();
                        // create introduction
                        WriterDoc.AppendNewStyledTextParagraphAndGetTextRange("Nicht erfüllte Kriterien:", WriterDocument.ParaStyleName.HEADING_2);
                        WriterDoc.AppendNewStyledTextParagraph(" ");
                        
                        // start table
                        XTextTable table = WriterDoc.AddTable(4, 1);
                        OoUtils.SetStringProperty(table, "TableName", "Nicht bestandene Kriterien");
                        //OoUtils.SetIntProperty(table, "HeaderRowCount", 1);

                        //style of Table
                        setStyleOfTable(ref table);

                        //add headers                    
                        CreateTableHeaders(ref table);

                        //combine them to one cell via colspan
                        //last row added were done a colSpan
                        bool lastRowColSpan = false;
                        int colcount = 1;
                        foreach (Category ca in results)
                        {
                            int crNumber = 1;
                            if (ca.resultType == ResultType.fail)
                            {
                                WriterDocument.AddRowToTable(ref table);
                                colcount++;
                                WriterDocument.DoColSpan(ref table, "A" + colcount, 3);
                                lastRowColSpan = true;
                                SetCellCriterionTextValue(ref table, "A" + colcount, caNumber + "." + ca.Name);
                                //WriterDoc.AppendNewStyledTextParagraphAndGetTextRange(caNumber +". "+ ca.Name, WriterDocument.ParaStyleName.HEADING);
                                foreach (Criterion cr in ca.Criteria)
                                {


                                    if (cr.Res.resultType == ResultType.fail)
                                    {
                                        if (lastRowColSpan == true)
                                        {
                                            WriterDocument.AddRowToTable(ref table, 1, -1, 4);
                                            FillColWithText(ref table, ++colcount, cr, crNumber, caNumber);
                                            lastRowColSpan = false;
                                        }
                                        else
                                        {
                                            WriterDocument.AddRowToTable(ref table);
                                            FillColWithText(ref table, ++colcount, cr, crNumber, caNumber);
                                        }


                                    }

                                    crNumber++;
                                }
                            }

                            caNumber++;
                        }
                        WriterDoc.SetColSize(ref table, 6310, 1);
                        WriterDoc.SetColSize(ref table, 9400, 2);
                    }


                    if (eval.warningCount > 0)
                    {
                        WriterDoc.NewParagraph();
                        WriterDoc.AppendNewStyledTextParagraphAndGetTextRange("Verbesserungswürdige Kriterien: ", WriterDocument.ParaStyleName.HEADING_2);
                        WriterDoc.AppendNewStyledTextParagraphAndGetTextRange("„Verbesserungswürdige Kriterien“ gelten zwar als bestanden, es existieren aber Verbesserungsmöglichkeiten.");
                        WriterDoc.AppendNewStyledTextParagraph(" ");
                        XTextTable table2 = WriterDoc.AddTable(4, 1);
                        CreateTableHeaders(ref table2);
                        bool lastRowColSpan2 = false;
                        caNumber = 1;
                        int colcount2 = 1;
                        foreach (Category ca in results)
                        {
                            int crNumber = 1;
                            bool CategoryHeaderWasMade = false;

                            //WriterDoc.AppendNewStyledTextParagraphAndGetTextRange(caNumber +". "+ ca.Name, WriterDocument.ParaStyleName.HEADING);
                            foreach (Criterion cr in ca.Criteria)
                            {


                                if (cr.Res.resultType == ResultType.passwithwarning)
                                {
                                    if (CategoryHeaderWasMade == false)
                                    {
                                        WriterDocument.AddRowToTable(ref table2);
                                        colcount2++;
                                        WriterDocument.DoColSpan(ref table2, "A" + colcount2, 3);
                                        lastRowColSpan2 = true;
                                        SetCellCriterionTextValue(ref table2, "A" + colcount2, caNumber + "." + ca.Name);
                                        CategoryHeaderWasMade = true;
                                    }

                                    if (lastRowColSpan2 == true)
                                    {
                                        WriterDocument.AddRowToTable(ref table2, 1, -1, 4);
                                        FillColWithText(ref table2, ++colcount2, cr, crNumber, caNumber);
                                        lastRowColSpan2 = false;
                                    }
                                    else
                                    {
                                        WriterDocument.AddRowToTable(ref table2);
                                        FillColWithText(ref table2, ++colcount2, cr, crNumber, caNumber);
                                    }



                                }
                                crNumber++;
                            }






                            caNumber++;
                        }
                        WriterDoc.SetColSize(ref table2, 6310, 1);
                        WriterDoc.SetColSize(ref table2, 9400, 2);
                    }
                }
            }



            WriterDoc.TextViewCursor.gotoStart(false);
        }




        private void SetCellCriterionTextValue(ref XTextTable table, string cellName, string name)
        {
            var a1 = table.getCellByName(cellName);
            if (a1 != null && a1 is XText)
            {
                // set the style of the cell
                // set style for the TextRange before inserting text content!
                var start = ((XTextRange)a1).getStart();
                if (start != null && start is XTextRange)
                {

                    OoUtils.SetStringProperty(start, "CharFontName", "Verdana");
                    OoUtils.SetProperty(start, "CharWeight", (float)150.000000);
                    // OoUtils.SetIntProperty(start,"",3 );
                }

                //OoUtils.SetIntProperty(WriterDoc.TextViewCursor, "CharWeight", 2000);
                ((XText)a1).setString(name);

                //util.Debug.GetAllInterfacesOfObject(a1);

                //util.Debug.GetAllProperties(a1 as XText);

            }
        }

        protected override void SetCellTextList(ref XTextTable table, string cellName, Criterion cr)
        {
            String items;
            var a1 = table.getCellByName(cellName);
            if (a1 != null && a1 is XText)
            {
                // set the style of the cell
                // set style for the TextRange before inserting text content!
                var start = ((XTextRange)a1).getStart();
                if (start != null && start is XTextRange)
                {
                    OoUtils.SetStringProperty(start, "NumberingStyleName", WriterDocument.ParaStyleName.LIST_1);
                    OoUtils.SetStringProperty(start, "CharFontName", "Verdana");
                    OoUtils.SetProperty(start, "CharWeight", (float)100.000000);

                    items = CreateStringFailedItems(cr);
                    ((XText)a1).setString(items);
                }
            }
        }

        private String CreateStringFailedItems(Criterion cr)
        {
            String items = "";
            if (cr.Res.Type == CriterionType.rating)
            {
                items += cr.Rec.Items[0].Desc + "(Ratingergebnis " + cr.Res.ratingerg * 100 + "%)";
            }
            else
            {
                for (int i = 0; i < cr.Rec.Items.Count; i++)
                {
                    if (cr.Res.Rating[i] == 0)
                    {
                        items += cr.Rec.Items[i].Desc + cr.Rec.Items[i].VarText + "\r";
                        //if (i == cr.Rec.Items.Count - 1)
                        //{
                        //   items += cr.Rec.Items[i].Desc + cr.Rec.Items[i].VarText;
                        //}
                        //else
                        //{
                        //    items += cr.Rec.Items[i].Desc + cr.Rec.Items[i].VarText+"\r";
                        //}
                    }
                }
                if (items.EndsWith("\r"))
                {
                    items = items.Remove(items.Length - 1);
                }
            }
            return items;
        }

        private void setStyleOfTable(ref XTextTable table)
        {
            if (table != null)
            {
                // util.Debug.GetAllProperties(table);
            }
        }

        private void CreateTableHeaders(ref XTextTable table)
        {
            setHeaderCellTextValue(ref table, "A1", "Kategorie");
            setHeaderCellTextValue(ref table, "B1", "Nicht erfüllte Prüfpunkte");
            //var cell = table.getCellByName("A1");

            setHeaderCellTextValue(ref table, "C1", "Bemerkung des Prüfers");
            setHeaderCellTextValue(ref table, "D1", "Priorität");
        }

        #endregion

    }
}
