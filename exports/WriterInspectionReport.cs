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
    class WriterInspectionReport : AbstractWriterExportBase
    {

        #region CTOR

        public WriterInspectionReport(List<Category> results, Evaluation eval, XComponentContext xContext = null)
            : base(results, eval, xContext)
        {
            this.eval = eval;
            this.results = results;

        }


        /// <summary>
        /// build the basic structure of the writerinspectionreport. 
        /// </summary>
        /// <param name="title">Title of the document</param>
        /// <param name="path">storeing path</param>
        public void WriteDocument(String title, String path)
        {

            if (results != null && results.Count > 0)
            {

                // create Doc
                if (WriterDoc != null)
                {
                    PrepareDoc("Prüfbericht");
                    // TODO: set Title
                    //TABLE


                    // fill with content

                    //Auflistung Fehler
                    int caNumber = 1;
                    //WriterDoc.NewParagraph();
                    if (eval.errorCount > 0)
                    {
                        WriterDoc.NewParagraph();
                        WriterDoc.AppendNewStyledTextParagraphAndGetTextRange("Nicht erfüllte Kriterien:", WriterDocument.ParaStyleName.HEADING_2);
                        WriterDoc.AppendNewStyledTextParagraph(" ");
                        XTextTable table = WriterDoc.AddTable(4, 1);
                        OoUtils.SetStringProperty(table, "TableName", "Nicht bestandene Kriterien");
                        //OoUtils.SetIntProperty(table, "HeaderRowCount", 1);
                        //style of Table
                        setStyleOfTable(ref table);
                        //add headers

                        createTableHeaders(ref table);





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

                        if (eval.warningCount > 0)
                        {
                            WriterDoc.NewParagraph();
                            WriterDoc.AppendNewStyledTextParagraphAndGetTextRange("Verbesserungswürdige Kriterien: ", WriterDocument.ParaStyleName.HEADING_2);
                            WriterDoc.AppendNewStyledTextParagraphAndGetTextRange("„Verbesserungswürdige Kriterien“ gelten zwar als bestanden, es existieren aber Verbesserungsmöglichkeiten.");
                            WriterDoc.AppendNewStyledTextParagraph(" ");
                            XTextTable table2 = WriterDoc.AddTable(4, 1);
                            createTableHeaders(ref table2);


                            caNumber = 1;
                            int rowCount = 1;
                            // do improvable categories
                            foreach (Category ca in results)
                            {                                
                                addImprovableCategoriesToTable(caNumber, ref rowCount, ref table2, ca);                                
                                
                                caNumber++;
                            }

                            WriterDoc.SetColSize(ref table2, 6310, 1);
                            WriterDoc.SetColSize(ref table2, 9400, 2);
                        }
                        if (eval.passCount > 0 && eval.passCount != eval.warningCount)
                        {
                            WriterDoc.NewParagraph();
                            WriterDoc.AppendNewStyledTextParagraphAndGetTextRange("Erfüllte Kriterien: ", WriterDocument.ParaStyleName.HEADING_2);
                            WriterDoc.AppendNewStyledTextParagraph(" ");
                            XTextTable table3 = WriterDoc.AddTable(4, 1);
                            createTableHeaders(ref table3);
                            bool lastRowColSpan3 = false;
                            caNumber = 1;
                            int colcount3 = 1;
                            foreach (Category ca in results)
                            {
                                int crNumber = 1;
                                bool CategoryHeaderWasMade = false;

                                //WriterDoc.AppendNewStyledTextParagraphAndGetTextRange(caNumber +". "+ ca.Name, WriterDocument.ParaStyleName.HEADING);
                                foreach (Criterion cr in ca.Criteria)
                                {


                                    if (cr.Res.resultType == ResultType.pass)
                                    {
                                        if (CategoryHeaderWasMade == false)
                                        {
                                            WriterDocument.AddRowToTable(ref table3);
                                            colcount3++;
                                            WriterDocument.DoColSpan(ref table3, "A" + colcount3, 3);
                                            lastRowColSpan3 = true;
                                            SetCellCriterionTextValue(ref table3, "A" + colcount3, caNumber + "." + ca.Name);
                                            CategoryHeaderWasMade = true;
                                        }

                                        if (lastRowColSpan3 == true)
                                        {
                                            WriterDocument.AddRowToTable(ref table3, 1, -1, 4);
                                            FillColWithText(ref table3, ++colcount3, cr, crNumber, caNumber);
                                            lastRowColSpan3 = false;
                                        }
                                        else
                                        {
                                            WriterDocument.AddRowToTable(ref table3);
                                            FillColWithText(ref table3, ++colcount3, cr, crNumber, caNumber);
                                        }



                                    }
                                    crNumber++;
                                }






                                caNumber++;
                            }
                            WriterDoc.SetColSize(ref table3, 6310, 1);
                            WriterDoc.SetColSize(ref table3, 9400, 2);
                        }
                    }
                }



            }
            WriterDoc.TextViewCursor.gotoStart(false);
        }

        /// <summary>
        /// fill the table for the improvable categories 
        /// </summary>
        /// <param name="caNumber">index number of the categorie eg. 1. Bildaufbau</param>
        /// <param name="rowCount">Number of the row which has to fill</param>
        /// <param name="table2">table to fill</param>
        /// <param name="ca">category to check</param>
        private void addImprovableCategoriesToTable(int caNumber, ref  int rowCount, ref XTextTable table2, Category ca)
        {            
            bool lastRowColSpan2 = false;
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
                        rowCount++;
                        WriterDocument.DoColSpan(ref table2, "A" + rowCount, 3);
                        lastRowColSpan2 = true;
                        SetCellCriterionTextValue(ref table2, "A" + rowCount, caNumber + "." + ca.Name);
                        CategoryHeaderWasMade = true;
                    }

                    if (lastRowColSpan2 == true)
                    {
                        WriterDocument.AddRowToTable(ref table2, 1, -1, 4);
                        FillColWithText(ref table2, ++rowCount, cr, crNumber, caNumber);
                        lastRowColSpan2 = false;
                    }
                    else
                    {
                        WriterDocument.AddRowToTable(ref table2);
                        FillColWithText(ref table2, ++rowCount, cr, crNumber, caNumber);
                    }


                }
                crNumber++;
            }
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
            //base.SetCellTextList(ref table, cellName, cr);

            String itemsFail;
            String itemsPass;
            var a1 = table.getCellByName(cellName);
            if (a1 != null && a1 is XText)
            {
                // set the style of the cell
                // set style for the TextRange before inserting text content!
                var start = ((XTextRange)a1).getStart();
                if (start != null && start is XTextRange)
                {
                    itemsFail = createStringFailedItems(cr);
                    itemsPass = CreateStringPassedItems(cr);
                    XTextCursor xTextCurser = ((XText)a1).createTextCursor();
                    xTextCurser.gotoEnd(false);
                    if (itemsFail != String.Empty)
                    {
                        OoUtils.SetStringProperty(xTextCurser, "NumberingStyleName", WriterDocument.ParaStyleName.NONE);
                        OoUtils.SetProperty(xTextCurser, "CharWeight", 150);
                        OoUtils.SetProperty(xTextCurser, "CharHeight", 12);
                        ((XText)a1).insertString(xTextCurser, "Nicht erfüllt: \r", false);

                        xTextCurser.gotoEnd(false);
                        OoUtils.SetStringProperty(xTextCurser, "NumberingStyleName", WriterDocument.ParaStyleName.LIST_1);
                        OoUtils.SetStringProperty(xTextCurser, "CharFontName", "Verdana");
                        OoUtils.SetProperty(xTextCurser, "CharWeight", (float)100.000000);
                        OoUtils.SetProperty(xTextCurser, "CharHeight", 12);
                        ((XText)a1).insertString(xTextCurser, itemsFail, false);
                        xTextCurser.gotoEnd(false);
                        //OoUtils.SetStringProperty(xTextCurser, "NumberingStyleName", WriterDocument.ParaStyleName.NONE);


                    }
                    if (itemsPass != String.Empty && itemsFail != String.Empty)
                    {
                        ((XText)a1).insertString(xTextCurser, "\r", false);
                        //XTextCursor xTextCurser2 = ((XText)a1).createTextCursor();
                        xTextCurser.gotoEnd(false);

                        OoUtils.SetStringProperty(xTextCurser, "NumberingStyleName", WriterDocument.ParaStyleName.NONE);
                        OoUtils.SetProperty(xTextCurser, "CharWeight", 150);
                        OoUtils.SetProperty(xTextCurser, "CharHeight", 12);
                        ((XText)a1).insertString(xTextCurser, "\rErfüllt: \r", false);
                        xTextCurser.gotoEnd(false);
                        OoUtils.SetStringProperty(xTextCurser, "NumberingStyleName", WriterDocument.ParaStyleName.LIST_1);
                        OoUtils.SetStringProperty(xTextCurser, "CharFontName", "Verdana");
                        OoUtils.SetProperty(xTextCurser, "CharWeight", (float)100.000000);
                        OoUtils.SetProperty(xTextCurser, "CharHeight", 12);
                        ((XText)a1).insertString(xTextCurser, itemsPass, false);
                        xTextCurser.gotoEnd(false);
                        return;

                    }
                    if (itemsPass != String.Empty)
                    {
                        OoUtils.SetStringProperty(xTextCurser, "NumberingStyleName", WriterDocument.ParaStyleName.NONE);
                        OoUtils.SetProperty(xTextCurser, "CharWeight", 150);
                        OoUtils.SetProperty(xTextCurser, "CharHeight", 12);
                        ((XText)a1).insertString(xTextCurser, "Erfüllt: \r", false);
                        xTextCurser.gotoEnd(false);
                        OoUtils.SetStringProperty(xTextCurser, "NumberingStyleName", WriterDocument.ParaStyleName.LIST_1);
                        OoUtils.SetStringProperty(xTextCurser, "CharFontName", "Verdana");
                        OoUtils.SetProperty(xTextCurser, "CharWeight", (float)100.000000);
                        OoUtils.SetProperty(xTextCurser, "CharHeight", 12);
                        ((XText)a1).insertString(xTextCurser, itemsPass, false);
                        xTextCurser.gotoEnd(false);
                    }

                }
            }
        }

        private string CreateStringPassedItems(Criterion cr)
        {
            String items = "";
            if (cr.Res.Type == CriterionType.rating && (cr.Res.resultType == ResultType.pass || cr.Res.resultType == ResultType.passwithwarning))
            {
                items += cr.Rec.Items[0].Desc + "(Ratingergebnis " + cr.Res.ratingerg * 100 + "%)";
            }
            else
            {
                for (int i = 0; i < cr.Rec.Items.Count; i++)
                {
                    if (cr.Res.Rating[i] == 1)
                    {
                        items += cr.Rec.Items[i].Desc + cr.Rec.Items[i].VarText + "\r";

                    }
                }
                if (items.EndsWith("\r"))
                {
                    items = items.Remove(items.Length - 1);
                }
            }
            return items;
        }

        private String createStringFailedItems(Criterion cr)
        {
            String items = "";
            if (cr.Rec.Type == CriterionType.one && cr.Res.resultType == ResultType.pass)
            {
                return "";
            }
            if (cr.Res.Type == CriterionType.rating && cr.Res.resultType == ResultType.fail)
            {
                items += cr.Rec.Items[0].Desc + "(Ratingergebnis " + cr.Res.ratingerg * 100 + "%)";
            }
            else if (cr.Res.Type == CriterionType.rating)
            {
                return "";
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

        private void createTableHeaders(ref XTextTable table)
        {
            setHeaderCellTextValue(ref table, "A1", "Kategorie");
            setHeaderCellTextValue(ref table, "B1", "Prüfpunkte");
            //var cell = table.getCellByName("A1");

            setHeaderCellTextValue(ref table, "C1", "Bemerkung des Prüfers");
            setHeaderCellTextValue(ref table, "D1", "Priorität");

        }


        #endregion

    }
}
