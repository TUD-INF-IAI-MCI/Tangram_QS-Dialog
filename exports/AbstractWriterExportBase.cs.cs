using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using unoidl.com.sun.star.uno;
using tud.mci.tangram.models.documents;
using tud.mci.tangram.util;
using System.Collections.Specialized;
using tud.mci.tangram.qsdialog.models;
using unoidl.com.sun.star.style;
using unoidl.com.sun.star.container;
using unoidl.com.sun.star.awt;
using unoidl.com.sun.star.text;

namespace tud.mci.tangram.exports
{
    abstract class AbstractWriterExportBase
    {
        #region Member

        private XComponentContext _xContext;
        protected XComponentContext xContext
        {
            get
            {
                if (_xContext == null)
                    xContext = OO.GetContext();
                return _xContext;
            }
            set { _xContext = value; }
        }


        private WriterDocument _writerDoc;
        protected WriterDocument WriterDoc
        {
            get
            {
                if (_writerDoc == null)
                    WriterDoc = new WriterDocument(xContext);
                return _writerDoc;
            }
            set { _writerDoc = value; }
        }

        public List<Category> results;
        public Evaluation eval;

        #endregion

        #region CTOR

        public AbstractWriterExportBase(List<Category> results, Evaluation eval,  XComponentContext xContext = null)
        {
            if (xContext != null)
            
                this.xContext = xContext;
                this.results = results;
                this.eval = eval;
                
           
        }

        #endregion



        /// <summary>
        /// Prepares the doc.
        /// </summary>
        protected void PrepareDoc(String title)
        {
            // Heading
            if (WriterDoc != null)
            {

                //TODO: Formatvorlagen anpassen
                XStyleFamiliesSupplier styleSuppl = WriterDoc.Document as XStyleFamiliesSupplier;
                if (styleSuppl != null)
                {
                    PrepareFontStyleFamilies(styleSuppl);
                }
                    //addTextToTextFrame(ref textFrame);

                CreateReportHeader(title);



                
            }
        }

        protected virtual void CreateReportHeader(String title)
        {
            WriterDoc.AppendNewStyledTextParagraphAndGetTextRange(title, WriterDocument.ParaStyleName.HEADING_1);
            //XTextFrame textFrame = WriterDoc.AddTextFrame(1000000, 16500);
            //WriterDoc.AppendNewStyledTextParagraphAndGetTextRange("Untersuchte Grafik: ", WriterDocument.ParaStyleName.NONE);
            WriterDoc.NewParagraph();
            WriterDoc.NewParagraph();
            WriterDoc.AddText("Untersuchte Grafik: ", 150);
            WriterDoc.AddTextAndNewParagraph("UKNOWN");
            WriterDoc.AddText("Typ des Mediums: ", 150);
            WriterDoc.AddTextAndNewParagraph(eval.earl.mediaType.ToString());
            WriterDoc.AddText("Prüfer: ", 150);
            WriterDoc.AddTextAndNewParagraph("UKNOWN");
            WriterDoc.AddText("Prüfdatum: ", 150);
            WriterDoc.AddTextAndNewParagraph(System.DateTime.Today.ToShortDateString());
            WriterDoc.AddTextAndNewParagraph("Ergebnis: ", 150);
            XTextTable table = WriterDoc.AddTable(4, 4);
            FillPriorityTable(ref table);
            WriterDoc.AddText("Gesamtergebnis: ", 150);
            WriterDoc.AddTextAndNewParagraph(eval.criteriaCount - eval.errorCount + "/" + eval.criteriaCount + " Kriterien bestanden");
            WriterDoc.AddTextAndNewParagraph("Gesamtbewertung: ", 150, 14);
            WriterDoc.AddText(eval.finalerg, 150, 14);
            WriterDoc.NewParagraph();
        }



        protected virtual void FillPriorityTable(ref XTextTable table)
        {
            createPriorityTableHeader(ref table);
            createPriorityTableContent(ref table);
        }


        protected virtual void createPriorityTableContent(ref XTextTable table)
        {

            WriterDocument.SetCellTextValue(ref table, "A2", "Priorität 1 (unbedingt erforderlich)");
            WriterDocument.SetCellTextValue(ref table, "A3", "Priorität 2 (wünschenswert)");
            WriterDocument.SetCellTextValue(ref table, "A4", "Priorität 3 (hilfreich, aber nicht erforderlich)");

            WriterDocument.SetCellTextValue(ref table, "B2", eval.prio1Pass.ToString(), "Verdana", 100, 1, 12, 3);
            WriterDocument.SetCellTextValue(ref table, "B3", eval.prio2Pass.ToString(), "Verdana", 100, 1, 12, 3);
            WriterDocument.SetCellTextValue(ref table, "B4", eval.prio3Pass.ToString(), "Verdana", 100, 1, 12, 3);

            WriterDocument.SetCellTextValue(ref table, "C2", eval.failprio1Count.ToString(), "Verdana", 100, 1, 12, 3);
            WriterDocument.SetCellTextValue(ref table, "C3", eval.failprio2Count.ToString(), "Verdana", 100, 1, 12, 3);
            WriterDocument.SetCellTextValue(ref table, "C4", eval.failprio3Count.ToString(), "Verdana", 100, 1, 12, 3);

            WriterDocument.SetCellTextValue(ref table, "D2", eval.prio1Count.ToString(), "Verdana", 100, 1, 12, 3);
            WriterDocument.SetCellTextValue(ref table, "D3", eval.prio2Count.ToString(), "Verdana", 100, 1, 12, 3);
            WriterDocument.SetCellTextValue(ref table, "D4", eval.prio3Count.ToString(), "Verdana", 100, 1, 12, 3);
        }

        protected virtual void createPriorityTableHeader(ref XTextTable table)
        {
            WriterDocument.SetCellTextValue(ref table, "A1", "Kriterien","Verdana", 150);
            WriterDocument.SetCellTextValue(ref table, "B1", "Bestanden", "Verdana", 150);
            //var cell = table.getCellByName("A1");

            WriterDocument.SetCellTextValue(ref table, "C1", "Nicht bestanden", "Verdana", 150);
            WriterDocument.SetCellTextValue(ref table, "D1", "Gesamtanzahl", "Verdana", 150);
            //WriterDocument.SetCellTextValue(ref table, "E1", "verbesserungswürdig");

        }

        protected virtual void PrepareFontStyleFamilies(XStyleFamiliesSupplier styleSuppl)
        {
            var families = styleSuppl.getStyleFamilies();
            if (families != null && families.hasByName("ParagraphStyles"))
            {
                XNameAccess parStyles = families.getByName("ParagraphStyles").Value as XNameAccess;
                if (parStyles != null)
                {

                    var titleStyle = parStyles.getByName(WriterDocument.ParaStyleName.TITLE).Value;
                    if (titleStyle != null)
                    {

                        //util.Debug.GetAllProperties(titleStyle);
                        //OoUtils.SetNamedProperty(titleStyle, "ParaVertAlignment", TabAlign.LEFT);
                        OoUtils.SetIntProperty(titleStyle, "CharHeight", 24);
                        OoUtils.SetIntProperty(titleStyle, "ParaAdjust", 0);
                        OoUtils.SetStringProperty(titleStyle, "CharFontName", "Verdana");

                    }
                    var frameStyle = parStyles.getByName(WriterDocument.ParaStyleName.FRAME_CONTENT).Value;
                    if (frameStyle != null)
                    {
                        OoUtils.SetStringProperty(frameStyle, "CharFontName", "Verdana");
                    }
                    var headerStyle = parStyles.getByName(WriterDocument.ParaStyleName.HEADING_1).Value;
                    if (headerStyle != null)
                    {
                        OoUtils.SetStringProperty(headerStyle, "CharFontName", "Verdana");
                        OoUtils.SetIntProperty(headerStyle, "CharHeight", 24);
                        OoUtils.SetIntProperty(headerStyle, "CharUnderline", 0);
                        util.Debug.GetAllProperties(headerStyle);
                    }
                    var headerStyle2 = parStyles.getByName(WriterDocument.ParaStyleName.HEADING_2).Value;
                    if (headerStyle2 != null)
                    {
                        OoUtils.SetStringProperty(headerStyle2, "CharFontName", "Verdana");
                        OoUtils.SetIntProperty(headerStyle2, "CharHeight", 16);
                        OoUtils.SetProperty(headerStyle2, "CharPosture", FontSlant.NONE);
                        OoUtils.SetIntProperty(headerStyle2, "CharUnderline", 1);
                    }
                    var noneStyle = parStyles.getByName(WriterDocument.ParaStyleName.NONE).Value;
                    if (headerStyle != null)
                    {
                        OoUtils.SetStringProperty(noneStyle, "CharFontName", "Verdana");
                        OoUtils.SetIntProperty(noneStyle, "CharHeight", 12);
                    }


                }


            }
        }


        protected virtual void FillColWithText(ref XTextTable table, int colCount, Criterion cr, int crNumber, int caNumber)
        {
            WriterDocument.SetCellTextValue(ref table, "A" + colCount, caNumber + "." + crNumber + " " + cr.Name);
            SetCellTextList(ref table, "B" + colCount, cr);
            //SetCellTextValue(ref table, "B" + colCount, caNumber + "." + crNumber + " " + cr.Name);
            WriterDocument.SetCellTextValue(ref table, "C" + colCount, cr.Comment);
            WriterDocument.SetCellTextValue(ref table, "D" + colCount, cr.Priority.ToString(), "Verdana", 150, 1, 13);
        }


        protected virtual void SetCellTextList(ref XTextTable table, string cellName, Criterion cr)
        {
            throw new NotImplementedException();
        }

        protected void setHeaderCellTextValue(ref XTextTable table, string cellName, string value)
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
                }
                ((XText)a1).setString(value);
            }
        }


    }
}
