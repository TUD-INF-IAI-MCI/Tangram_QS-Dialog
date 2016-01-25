// ***********************************************************************
// Assembly         : TANGRAM OOo Draw Extention
// Author           : Admin
// Created          : 09-17-2012
//
// Last Modified By : Admin
// Last Modified On : 09-17-2012
// ***********************************************************************
// <copyright file="UnoDialogSample.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using unoidl.com.sun.star.awt;
using unoidl.com.sun.star.uno;
using unoidl.com.sun.star.lang;
using unoidl.com.sun.star.view;
using tud.mci.tangram.EARL;
using tud.mci.tangram.qsdialog.models;
using tud.mci.tangram.models.dialogs;
using tud.mci.tangram.util;


namespace tud.mci.tangram.qsdialog.Dialogs
{

    public partial class UnoDialogSample : AbstactUnoDialogBase, XTextListener, XSpinListener, XActionListener, XFocusListener, XMouseListener, XItemListener, XAdjustmentListener, XKeyListener, XSelectionChangeListener
    {

        #region Members
        
        List<Category> categories = Parser.Parse();
        Dictionary<String,Criterion> allcriteria = Parser.GetAllCriteria();
        Earl earl = new Earl();
        Evaluation eval = new Evaluation();
        EarlReader earlReader = new EarlReader();
        
        int maxCountOfItemsTypeOne = Parser.GetMaxCountOfItemsTypOne();
        int maxCountOfItemsTypeAll = Parser.GetMaxCountOfItems();
        int maxCountOfRatingItems = Parser.GetMaxCountOfRatingItems();


        //UnoDialogSample oUnoDialogSample;
        int index;
        bool hide;
        
        MediaType chosenMediaType= MediaType.All;
        String[] mediaTypes = { "All", "Schwellpapier", "Tiger","Stiftplatte" };
        readonly OrderedDictionary criteriaMap = new OrderedDictionary();
        readonly OrderedDictionary criteriaLabelMap = new OrderedDictionary();
        private OrderedDictionary criteriaResultLabelMap = new OrderedDictionary();
        private OrderedDictionary testPointsMap = new OrderedDictionary();
        Object activeLabel;
        Object activeResultLabel;
        Object introduction;
        ScrollableContainer testPointsContainer;
        ScrollableContainer container;
        ScrollableContainer commentarResultContainer;
        //comment which shows in results
        XControl commentResult;
        #region control names
        const String evalHeaderName = "evalHeader";
        const String headerName = "catHeader";
        const String helpTextName = "helpTextLabel";
        const String descTextName = "descTextLabel";
        const String descBoxName = "Beschreibung";
        const String helpBoxName = "Hilfe";
        const String recomBoxName = "Prüfpunkte";
        const String prioritaetName = "Prioritaet";
        const String nextButtonName = "nextButtonLabel";
        const String prevButtonName = "prevButtonLabel";
        const String progressBarName = "progressBar";
        const String evalButtonName = "evaluationButtonLabel";
        const String earlLoadButtonName = "earlLoadButtonLabel";
        const String evalResultLabel = "finalergLabel";
        const String scrollContainerResultName = "scrollContainerResult";
        const String evalBoxName = "evalBox";
        const String inspectionReportButtonName = "inspectionReportButton";
        const String errorReportButtonName = "errorReportButton";
        const String toCriterionButtonName = "criterionButton";
        const String testPointsContainerName = "testPointsContainer";
        const String testPointsSkalaName = "testPointsSkala";
        const String criterionHeaderResultName = "criterionHeaderResult";
        const String commentOfTester = "commentOfTester";
        const String resultLabelName = "resultLabel";
        const String introductionLabelName = "introductionLabel";
        const String testPointLabelName = "testPointLabel";
        const String commentarResultContainerName = "commentarResultContainer";
        const String scrollContainerTestPointsName = "scrollContainerTestpoints";
        const String testPointHeaderName = "testPointHeader";
        const String commentarResultLabelName = "commentarResultLabel";
        const String prioExplanationLabelName = "prioExplanationLabel";
        
        #endregion

        #endregion


        /// <summary>
        /// Create a new instance of UnoDialogSample
        /// </summary>
        /// <param name="xContext"></param>
        /// <param name="xMcf"></param>
        public UnoDialogSample(XComponentContext xContext, XMultiComponentFactory xMcf)
            : base(xContext, xMcf) { }

        /// <summary>
        /// 
        /// </summary>
        public void ShowDialog()
        {
            //this = null;

            try
            {
                InitalizeDialog("TestDialog_name", "OpenOffice Test Dialog", 380, 380, 102, 41);
                createWindowPeer();

                #region Step 0 always visible

                createScrollableContainer(0, 0, 200, 3000);

                index = -1;
                activeLabel = null;
              

                //Creation of the Members
                var header1 = InsertFixedText("", 106, 6, 300, headerName);
                SetProperty(header1, "FontWeight", h1_fontWeight);
                var prioritaet1 = InsertFixedLabel("", 318, 6, 300, prioritaetName);
                var descBox1 = insertGroupBox(100, 20, 100, 250, descBoxName);
                var descText1 = InsertFixedLabel("", 105, 35, 200, 80, 0, null, descTextName);
                SetProperty(descText1, "MultiLine", true);
                var helpBox1 = insertGroupBox(100, 130, 100, 250, helpBoxName);
                var helpText1 = InsertFixedLabel("", 105, 145, 200, 80, 0, null, helpTextName);
                SetProperty(helpText1, "MultiLine", true);
                var recomBox1 = insertGroupBox(100, 240, 100, 250, recomBoxName);
                var commentLabel = InsertFixedText("Kommentar", 105, 290, 100, "commentLabel");
                var commentField = InsertEditField("", 105, 300, 200, this, this, this, "commentField");

                //experiment
                //var checkboxIsImportant  = insertCheckBox(this, 200, 200, 100, "isImportant");
                //SetProperty(checkboxIsImportant, "EnableVisible", false);
                //SetProperty(checkboxIsImportant,"TriState",)

                SetProperty(commentLabel, "EnableVisible", false);
                SetProperty(commentField, "EnableVisible", false);

                //util.Debug.GetAllProperties(commentField);


                SetProperty(descBox1, "TabIndex", ((short)(0)));
                SetProperty(helpBox1, "TabIndex", ((short)(0)));
                SetProperty(recomBox1, "TabIndex", ((short)(0)));

                #endregion

                #region Step 7 Introduction
                
                var header = GetControlByName(headerName);
                if (header != null)
                {
                    SetProperty(header, "Label", "Einführung");
                    SetProperty(header, "TabIndex ", 0);
                } 

                var mediumLabel = InsertFixedText("Medium:", 105, 145, 30);               
                var mediumRadio0 = insertRadioButton(this, 145, 145, 100,"All","All");
                mediumRadio0.setState(true);
               
                var mediumRadio1 = insertRadioButton(this, 145, 153, 100, "Schwellpapier", "Schwellpapier");
                var mediumRadio2 = insertRadioButton(this, 145, 161, 100, "Tiger", "Tiger");
                var mediumRadio3 = insertRadioButton(this, 145, 169, 100, "Stiftplatte", "Stiftplatte");
                
                SetStepProperty(mediumLabel, 7);
                SetStepProperty(mediumRadio0, 7);
                SetStepProperty(mediumRadio1, 7);
                SetStepProperty(mediumRadio2, 7);
                SetStepProperty(mediumRadio3, 7);
                

                if (mediumRadio0 is XWindow)
                {
                    (mediumRadio0 as XWindow).setFocus();
                }

                #endregion
             

                #region checkboxen + radiobuttons
                /* Position of the Items(RadioButtons, Checkboxen, ....) */
                int yPosCB = 247;
                int yPosRB = 247;
                int yPosRatingBox = 247;

                /* Creation of  checkboxen for type all or count */
                for (int s = 0; s <= maxCountOfItemsTypeAll; s++)
                {
                    yPosCB = yPosCB + 8;
                    var checkBox1 = insertCheckBox(this, 105, yPosCB, 300, "CheckBox" + s.ToString());
                    SetStepProperty(checkBox1, 1);
                    SetProperty(checkBox1, "TriState", false);
                    SetProperty(checkBox1, "EnableVisible", false);

                    
                    
                    

                    var checkBoxStep3 = insertCheckBox(this, 105, yPosCB, 300, "CheckBoxStep3" + s.ToString());
                    SetProperty(checkBoxStep3, "TriState", false);
                    SetProperty(checkBoxStep3,"EnableVisible", false);
                    SetStepProperty(checkBoxStep3, 3);
                    
                                                
                }


                /* Creation of RadioButtons */
                
                for (int s = 0; s <= maxCountOfItemsTypeOne; s++)
                {
                    yPosRB = yPosRB+8;
                    var radioButton1 = insertRadioButton(this, 105, yPosRB, 250, "label"+s, "RadioButton" + s.ToString());
                    //SetProperty(radioButton1, "Name", "RadioButton" + Convert.ToString(s));
                   // util.Debug.GetAllProperties(radioButton1);
                    SetStepProperty(radioButton1, 2);
                    SetProperty(radioButton1, "EnableVisible", false);
                    SetProperty(radioButton1, "Enabled", true);
                    SetProperty(radioButton1, "Tabstop", true);
                    SetProperty(radioButton1, "TabIndex", ((short)(s + 1)));

                    //util.Debug.GetAllProperties(radioButton1);
                    //util.Debug.GetAllInterfacesOfObject(radioButton1);
                    //util.Debug.GetAllServicesOfObject(radioButton1);
                }

                /* Creation of the RatingBoxes */
                for(int s = 0; s< maxCountOfRatingItems;s++){
                    yPosRatingBox = yPosRatingBox + 8;
                    insertRatingGroup(this, 105, yPosRatingBox, 100, 4, String.Empty, "RatingBox" + s.ToString());
                   // SetProperty(GetControlByName("RatingBox" + s.ToString()), "EnableVisible", false );
                }

                
               




                // hide the Rating items 
                hideRatingStep3();
                //insertRatingGroup(this, 10, 10, 100, 3, String.Empty, );
                //SetProperty(helpBox1, "EnableVisible", false);
                //SetProperty(descBox1, "EnableVisible", false);
                //SetProperty(recomBox1, "EnableVisible", false);

                #endregion

                #region buttons
                var buttonGoToCriterion = insertButton(this, 110, 343, 50,"Zum Kriterium",0, toCriterionButtonName);
                SetStepProperty(buttonGoToCriterion, 5);
                var buttonNext = insertButton(this, 280, 360, 70, "Weiter", 0, nextButtonName);
                SetProperty(buttonNext, "TabIndex ", ((short)(2)));
                var buttonPrev = insertButton(this, 100, 360, 70, "Zurück", 0, prevButtonName);
                SetProperty(buttonPrev, "EnableVisible", false);
                SetProperty(buttonPrev, "TabIndex ", ((short)(3)));
                var buttonSubmit = insertButton(this, 190, 360, 70, "Auswerten", 0, evalButtonName);
                SetProperty(buttonSubmit, "TabIndex ", ((short)(4)));
                var loadbutton = insertButton(this, 200, 200, 100, "Prüfbericht laden",7, earlLoadButtonName);
                SetStepProperty(loadbutton, 7);
                var inspectionReportButton = insertButton(this, 285, 363, 80, "Prüfbericht exportieren", 0, inspectionReportButtonName);
                SetStepProperty(inspectionReportButton, 5);
                var errorReportButton = insertButton(this, 200, 363, 80, "Fehlerbericht exportieren", 0, errorReportButtonName);
                SetStepProperty(errorReportButton, 5);
                #endregion
                #region evaluation
                //Step 5 for evalutaion Member
                var headerEvaluation = InsertFixedText("Ergebnis: ", 106, 6, 300, evalHeaderName);
                //var prioExplanationLabel = InsertFixedText("Prioritäten: 1-Hoch 2-Mittel 3-Niedrig",106,26,150,prioExplanationLabelName);
                var ergString = InsertFixedLabel("", 135, 6, 200, 80, 0, null, evalResultLabel);
                SetProperty(ergString, "MultiLine", true);
                SetProperty(ergString, "FontWeight", FontWeight.BOLD);
                var evalBox1 = insertGroupBox(100, 220, 140, 265, "Details", evalBoxName);
                
                var resultLabel = InsertFixedText("", 290, 30, 100, resultLabelName);
                var introductionLabel = InsertFixedText("Für Fehlerdetails, klicken Sie ein Kriterium in der oberen Box an.",150, 260,200,introductionLabelName);
                SetStepProperty(resultLabelName, 5);
                //var testPointSkala = InsertProgressBar(200, 260, 100, 10, testPointsSkalaName);
                var testPointHeader = InsertFixedText("Nicht erfüllte Prüfpunkte:", 110, 250, 200, testPointHeaderName);

                //var testPointLabel = InsertFixedText("", 280,250 , 50, testPointLabelName);
                var criterionHeaderResult = InsertFixedText("",110, 230, 250, 30, criterionHeaderResultName);
                var commentarResultLabel = InsertFixedText("Kommentar des Prüfers:", 110, 307,200, commentarResultLabelName);
                commentarResultContainer = createScrollableCommentarResultContainer(110,317, 220, 20, commentarResultContainerName);
                //SetProperty(testPointHeader, "FontWeight", FontWeight.BOLD);
                SetProperty(criterionHeaderResult, "FontWeight", FontWeight.BOLD);
                SetStepProperty(testPointHeader, 5);
                SetProperty(criterionHeaderResult, "MultiLine", true);

                //SetStepProperty(prioExplanationLabel, 5);
                SetStepProperty(commentarResultLabel, 5);
                SetStepProperty(headerEvaluation, 5);
                SetStepProperty(evalBox1, 5);
                //SetStepProperty(testPointLabel, 5);
                SetStepProperty(introductionLabel, 5);
                SetStepProperty(resultLabel, 5);
                SetStepProperty(criterionHeaderResult, 5);
                SetStepProperty(ergString, 5);
                SetStepProperty(commentarResultContainerName, 5);
                testPointsContainer = createScrollableContainerTestPoints(110,260 ,220 ,40, scrollContainerTestPointsName);
                

                #endregion
                showIntroduction();
                ExecuteDialogWithembeddedExampleSnippets();
            }
            catch (unoidl.com.sun.star.uno.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("uno.Exception:");
                System.Diagnostics.Debug.WriteLine(ex);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("System.Exception:");
                System.Diagnostics.Debug.WriteLine(ex);
            }

            finally
            {
                //make sure always to dispose the component and free the memory!
                if (this != null)
                {
                    if (MXComponent != null)
                    {
                        MXComponent.dispose();
                    }
                }
            }
        }

        private void initalize()
        {
            SetStepProperty(MXDialogControl, 0);
            testPointsContainer.SetVisible(false);
            commentarResultContainer.SetVisible(false);
            
        }

                      
        /// <summary>
        /// fill the items with the right content (TODO: split the functions)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="step1"></param>
        public void changeContent(Object id, int step1)
        {           
            handleStepChange(step1, null);

            

            int step = step1;
            Criterion crclick = (Criterion)this.criteriaMap[id];
            changeActiveLabel(id as String);


            var header = GetControlByName(headerName);
            if (header != null)
            {
                SetProperty(header, "Label", crclick.Name);
                SetProperty(header, "TabIndex ", 0);
            } 
            
            var prioritaet1 = GetControlByName(prioritaetName);
            if (prioritaet1 != null)
            {
                SetProperty(prioritaet1, "Label", "Priorität: " + crclick.Priority);
                SetProperty(prioritaet1, "EnableVisible", true);
                SetProperty(prioritaet1, "TabIndex ", 0);
            }

            var descLabel = GetControlByName(descTextName);
            if (descLabel != null)
            {
                SetProperty(descLabel, "Label", crclick.Desc);
                SetProperty(descLabel, "TabIndex ", 0);
            }

            var helpLabel = GetControlByName(helpTextName);
            if (helpLabel != null)
            {
                SetProperty(helpLabel, "Label", crclick.Help);
                SetProperty(helpLabel, "TabIndex ", 0);
            }


            


            int countCB = 0;
            int countRB = 0;
            int countRating = 0;
            int itemCount = crclick.Rec.Items.Count;

            switch (step1)
            {
                /* all */
                case 1:
                    foreach (Item i in crclick.Rec.Items)
                    {
                        // String vars="";
                        var a = GetControlByName("CheckBox" + Convert.ToString(countCB));
                        
                        //if (i.Var.Count > 0)
                        //{
                        //    foreach (Variable v in i.Var)
                        //    {
                        //        vars=vars +" "+ v.Max 
                        //    }
                        //}
                        String varText = createVarText(i);
                        if (createVarText(i) != null)
                        {
                            SetProperty(a, "Label", i.Desc + varText);
                        }
                        else
                        {
                            SetProperty(a, "Label", i.Desc);
                        }
                        (a as XCheckBox).setState(3);
                        SetProperty(a, "EnableVisible", true);

                       

                        SetProperty(a, "TabIndex ", 1);

                        float x = ((Criterion)criteriaMap[id]).Res.Rating[countCB];
                        if (x == 1)
                        {
                            (a as XCheckBox).setState(1);
                        }

                        if (countCB == 0 && a is XWindow)
                        {
                            (a as XWindow).setFocus();
                        }
                        countCB++;
                    }
                    //commentbox position
                    ShowCommentar(crclick, countCB);



                    for (int s = countCB; s < maxCountOfItemsTypeAll; s++)
                    {

                        var a = GetControlByName("CheckBox" + Convert.ToString(countCB));
                        
                        SetProperty(a, "EnableVisible", false);
                        SetProperty(a, "TabIndex ", 1);
                        countCB++;
                    }

                    break;
                
                /* one */
                case 2:
                    foreach (Item i in crclick.Rec.Items)
                    {
                        var a = GetControlByName("RadioButton" + Convert.ToString(countRB));
                        if (a is XRadioButton)
                        {
                            (a as XRadioButton).setState(false);
                            float x = crclick.Res.Rating[countRB];
                            if (x == 1)
                            {
                                (a as XRadioButton).setState(true);
                            }
                            else if (countRB == 0)
                            {
                                (a as XRadioButton).setState(true);
                            }
                            //Debug.GetAllProperties(a);
                            SetProperty(a, "Label", i.Desc);
                            SetProperty(a, "EnableVisible", true);
                            SetProperty(a, "TabIndex ", 1);

                            if (countRB == 0 && a is XWindow)
                            {
                                (a as XWindow).setFocus();
                            }

                            countRB++;
                        }
                    }

                    ShowCommentar(crclick, countRB);
                    for (int s = countRB; s < maxCountOfItemsTypeAll; s++)
                    {
                        var a = GetControlByName("CheckBox" + Convert.ToString(countRB));
                        SetProperty(a, "EnableVisible", false);
                        SetProperty(a, "TabIndex ", 1);
                        countRB++;
                    }
                    break;

                /* count & rating */
                case 3:
                    foreach (Item i in crclick.Rec.Items)
                    {
                        if (i.Role == "rating")
                        {
                            handleStepChange(4, null);
                            showRatingStep3();
                            var b = GetControlByName("RatingBox" + countRating.ToString());
                            SetProperty(b, "Label", i.Desc);
                            SetProperty(b, "EnableVisible", true);
                            SetProperty(b, "TabIndex ", 1);
                            //show only the result which was clicked
                            for (int z = 0; z < 5; z++)
                            {
                                var rating = GetControlByName("RatingBox" + countRating + "_RATING_" + z);
                                (rating as XRadioButton).setState(false);
                                if (crclick.Res.Rating[z] == 1)
                                {
                                    
                                    (rating as XRadioButton).setState(true);
                                }
                            }

                            if (countRating == 0 && b is XWindow)
                            {
                                (b as XWindow).setFocus();
                            }

                            countRating++;                           
                        }

                        else
                        {                            
                            var a = GetControlByName("CheckBoxStep3" + Convert.ToString(countCB));
                            String varText = createVarText(i);
                            if (varText != null){
                                SetProperty(a, "Label", i.Desc + varText);
                            }                       
                            else
                            {
                                SetProperty(a, "Label", i.Desc);
                            }
                            
                            (a as XCheckBox).setState(3);
                            float x = ((Criterion)criteriaMap[id]).Res.Rating[countCB];
                            if (x == 1)
                            {
                                (a as XCheckBox).setState(1);
                            }

                            if (countCB == 0 && a is XWindow)
                            {
                                (a as XWindow).setFocus();
                            }

                            countCB++;
                            SetProperty(a, "EnableVisible", true);
                            SetProperty(a, "TabIndex ", 1);
                        }
                    }
                    //TODOOOO : is not right beacause there can be more than one rating
                    if (countRating > 0) { 
                        ShowCommentarForRating(crclick, countRating);
                    }
                    else     ShowCommentar(crclick, countCB);
                    for (int s = countCB; s < maxCountOfItemsTypeAll; s++)
                    {

                        var c = GetControlByName("CheckBoxStep3" + Convert.ToString(countCB));
                        SetProperty(c, "EnableVisible", false);
                        SetProperty(c, "TabIndex ", 1);
                        countCB++;
                    }
                   
                    
                    for (int s = countRating; s < maxCountOfRatingItems; s++)
                    {
                        var b = GetControlByName("RatingBox" + countRating.ToString());
                        // SetProperty(b, "EnableVisible", false);
                        SetProperty(b, "TabIndex ", 1);
                        countRating++;
                    }

                    
                   
                                        
                    break;

                case 5:
                    hideAll();
                break;
            }

        }

        /// <summary>
        /// change the Content of Categories 
        /// </summary>        
        private void changeContentCategory(string id )
        {
            Category categorieClick = ((Category)this.criteriaMap[id]);
            
            handleStepChange(6, null);
            var prioritaet = GetControlByName(prioritaetName);
            SetProperty(prioritaet, "EnableVisible", false);

            var recomBox = GetControlByName("FrameControl3");
            var helpBox = GetControlByName("FrameControl2");
            SetProperty(recomBox, "EnableVisible", false);
            SetProperty(recomBox, "TabIndex ", 0);

            var header = GetControlByName(headerName);
            if (header != null)
            {
                SetProperty(header, "Label", categorieClick.Name);
                SetProperty(header, "TabIndex ", 0);
            }
            var descLabel = GetControlByName(descTextName);
            if (descLabel != null)
            {
                SetProperty(descLabel, "Label", categorieClick.Desc);
                SetProperty(descLabel, "TabIndex ", 0);
            }
            
            var helpLabel = GetControlByName(helpTextName);
            if (helpLabel != null)
            {               
                SetProperty(helpLabel, "Label", categorieClick.Help);
                if (GetProperty(helpLabel, "Label").Equals("")) SetProperty(helpBox,"EnableVisible", false);
                SetProperty(helpLabel, "TabIndex ", 0);
            }


            SetProperty(GetControlByName("commentLabel"), "EnableVisible", false);
            SetProperty(GetControlByName("commentField"), "EnableVisible", false);
            changeActiveLabel(id as String);            

        }

        /// <summary>
        /// show introduction step
        /// </summary>
        public void showIntroduction()
        {
            hideAll();
            var header = GetControlByName(headerName);
            SetProperty(header, "EnableVisible", true);
            SetProperty(header, "Label", "Einführung");
            var descBox = GetControlByName("FrameControl");
            SetProperty(descBox, "EnableVisible", true);
            
            var descLabel = GetControlByName(descTextName);
            SetProperty(descLabel,"EnableVisible" ,true);
            SetProperty(descLabel, "Label", "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.");
            index = -1;
            handleStepChange(7, null);
            SetProperty(this.activeLabel, "FontUnderline", FontUnderline.NONE);
            SetProperty(introduction, "FontUnderline", FontUnderline.BOLD);
            SetProperty(GetControlByName(evalButtonName), "EnableVisible", true);
        }



        /// <summary>
        /// hide all elements (buttons not)
        /// </summary>
        public void hideAll()
        {
            
            hide = true;
            //evalMember
            //SetProperty(GetControlByName(prioExplanationLabelName), "EnableVisible", false);
            SetProperty(GetControlByName(commentarResultLabelName), "EnableVisible", false);
            SetProperty(GetControlByName(testPointHeaderName), "EnableVisible", false);
            SetProperty(GetControlByName(testPointLabelName), "EnableVisible", false);
            SetProperty(GetControlByName(testPointsSkalaName), "EnableVisible", false);
            SetProperty(GetControlByName(criterionHeaderResultName), "EnableVisible", false);
            SetProperty(GetControlByName(testPointsSkalaName), "EnableVisible", false);
            SetProperty(GetControlByName(inspectionReportButtonName), "EnableVisible", false);
            SetProperty(GetControlByName(errorReportButtonName), "EnableVisible", false);
            SetProperty(GetControlByName(introductionLabelName), "EnableVisible", false);
            SetProperty(GetControlByName(resultLabelName), "EnableVisible", false);
            SetProperty(GetControlByName(evalHeaderName), "EnableVisible", false);
            SetProperty(GetControlByName(evalResultLabel), "EnableVisible", false);
            SetProperty(GetControlByName(inspectionReportButtonName), "EnableVisible", false);
            SetProperty(GetControlByName(testPointsSkalaName), "EnableVisible", false);
            SetProperty(GetControlByName(toCriterionButtonName), "EnableVisible", false);
            //SetProperty(GetControlByName(scrollContainerResultName), "EnableVisible", false);
            //SetProperty(GetControlByName(scrollContainerResult), "EnableVisible", false);
            SetProperty(GetControlByName(evalBoxName), "EnableVisible", false);
            var header = GetControlByName(headerName);
            SetProperty(header, "EnableVisible", false);
            var descLabel = GetControlByName(descTextName);
            SetProperty(descLabel, "EnableVisible", false);
            var helpLabel = GetControlByName(helpTextName);
            SetProperty(helpLabel, "EnableVisible", false);
            var prioritaet1 = GetControlByName(prioritaetName);
            SetProperty(prioritaet1, "EnableVisible", false);
            //Nicht sName sondern FrameControl
            var descBox = GetControlByName("FrameControl");
            SetProperty(descBox, "EnableVisible", false);
            //Nicht sName sondern FrameControl3
            var recomBox = GetControlByName("FrameControl3");
            SetProperty(recomBox, "EnableVisible", false);
            //Nicht sName sondern FrameControl2
            var helpBox = GetControlByName("FrameControl2");
            SetProperty(helpBox, "EnableVisible", false);


            SetProperty(GetControlByName("commentField"), "EnableVisible", false);
            SetProperty(GetControlByName("commentLabel"), "EnableVisible", false);
            //var prevButton = GetControlByName(prevButtonName);
            //SetProperty(prevButton, "EnableVisible", false);
            //var nextButton = GetControlByName(prevButtonName);
            //SetProperty(nextButton, "EnableVisible", false);
        }

        /// <summary>
        /// function to show all elements (with buttons)
        /// </summary>
        public void showAll()
        {
            hide = false;
            //evaluationMember
            SetProperty(GetControlByName(evalButtonName), "EnableVisible", true);

            SetProperty(introduction, "FontUnderline", FontUnderline.NONE);
            var header = GetControlByName(headerName);
            SetProperty(header, "EnableVisible", true);
            var descLabel = GetControlByName(descTextName);
            SetProperty(descLabel, "EnableVisible", true);
            var helpLabel = GetControlByName(helpTextName);
            SetProperty(helpLabel, "EnableVisible", true);
            var prioritaet1 = GetControlByName(prioritaetName);
            SetProperty(prioritaet1, "EnableVisible", true);
            //Nicht sName sondern FrameControl
            var descBox = GetControlByName("FrameControl");
            SetProperty(descBox, "EnableVisible", true);
            //Nicht sName sondern FrameControl3
            var recomBox = GetControlByName("FrameControl3");
            SetProperty(recomBox, "EnableVisible", true);
            //Nicht sName sondern FrameControl2
            var helpBox = GetControlByName("FrameControl2");
            SetProperty(helpBox, "EnableVisible", true);
            var prevButton = GetControlByName(prevButtonName);
            SetProperty(prevButton, "EnableVisible", true);
            var nextButton = GetControlByName(prevButtonName);
            SetProperty(nextButton, "EnableVisible", true);
        }

        /// <summary>
        /// underlines the right element which is active
        /// </summary>
        /// <param name="id"></param>
        public void changeActiveLabel(String id)
        {
            var newLabel = criteriaLabelMap[id.ToString()];
            SetProperty(this.activeLabel, "FontUnderline", FontUnderline.NONE);
            SetProperty(newLabel, "FontUnderline", FontUnderline.BOLD);
            this.activeLabel = newLabel;
        }

        public void changeActiveResultLabel(String id)
        {
            var newLabel = criteriaResultLabelMap[id.ToString()];
            SetProperty(this.activeResultLabel, "FontUnderline", FontUnderline.NONE);
            SetProperty(newLabel, "FontUnderline", FontUnderline.BOLD);
            this.activeResultLabel = newLabel;
        }

        /// <summary>
        /// Get the index of a element by key
        /// </summary>
        /// <param name="od">criteriaMap the </param>
        /// <param name="id"></param>
        /// <returns></returns>
        private int getIndexByKey(OrderedDictionary od, String id)
        {
            int index1 = 0;
            for (int i = 0; i < od.Count; i++)
            {
                if (od[i] is Criterion) { 
                if (((Criterion)od[i]).Id.Equals(id)) { index1 = i; break; }
                }
                else if (od[i] is Category)
                {
                    if (((Category)od[i]).Id.Equals(id)) { index1 = i; break; }
                }
            }
            return index1;
        }

        /// <summary>
        /// create variabletext
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private  String createVarText(Item i)
        {
            if (i.Var.Count == 0) return null;
            else
            {
                String varText = "";
                foreach (Variable va in i.Var)
                {
                    if (va.MediaType.Equals(chosenMediaType))
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
                        if (va.MediaType != null)
                        {
                            varText += " bei Medium: " + va.MediaType;
                        }


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
                            if (va.MediaType != null)
                            {
                                varText += " bei Medium: " + va.MediaType;
                            }


                            return varText;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// hide elements for ratingboxes
        /// </summary>
        public void hideRatingStep3()
        {
            for (int i = 0; i < maxCountOfRatingItems; i++)
            {
                SetProperty(GetControlByName("RatingBox" + i.ToString()), "EnableVisible", false );
                SetProperty(GetControlByName("RatingBox" + i.ToString()+"label1"), "EnableVisible", false );
                SetProperty(GetControlByName("RatingBox" + i.ToString() + "label2"), "EnableVisible", false);
                for (int s = 0; s < 5; s++)
                {
                    var a = GetControlByName("RatingBox" + i.ToString() + "_RATING_" + s);
                    SetProperty(a, "EnableVisible", false);

                }

            }
        }

        /// <summary>
        /// show elements for ratingboxes
        /// </summary>
        public void showRatingStep3()
        {
            for (int i = 0; i < maxCountOfRatingItems; i++)
            {
                SetProperty(GetControlByName("RatingBox" + i.ToString()), "EnableVisible", true);
                SetProperty(GetControlByName("RatingBox" + i.ToString() + "label1"), "EnableVisible", true);
                SetProperty(GetControlByName("RatingBox" + i.ToString() + "label2"), "EnableVisible", true);
                for (int s = 0; s < 5; s++)
                {
                    var a = GetControlByName("RatingBox" + i.ToString() + "_RATING_" + s);
                    SetProperty(a, "EnableVisible", true);

                }

            }
        }

        public void ShowCommentar(Criterion cr, int count)
        {
            

            var lastVisibleCheckBox = GetControlByName("CheckBox" + Convert.ToString(count));
            var commentLabel = GetControlByName("commentLabel");
            var commentField = GetControlByName("commentField");
            var yPos = GetProperty(lastVisibleCheckBox, "PositionY");
          
            SetProperty(commentLabel, "EnableVisible", true);
            SetProperty(commentLabel, "PositionY", (int)yPos + 8);
            SetProperty(commentField, "PositionY", (int)yPos + 16);

            SetProperty(commentField, "Text", cr.Comment);
            SetProperty(commentField, "EnableVisible", true);
        }

        public void ShowCommentarForRating(Criterion cr, int count){
            count--;
            var lastVisibleElement = GetControlByName("RatingBox" + Convert.ToString(count) );
            var commentLabel = GetControlByName("commentLabel");
            var commentField = GetControlByName("commentField");
            var yPos = GetProperty(lastVisibleElement, "PositionY");
           
            SetProperty(commentLabel, "EnableVisible", true);
            SetProperty(commentLabel, "PositionY", (int)yPos + 32);
            SetProperty(commentField, "PositionY", (int)yPos + 40);

            SetProperty(commentField, "Text", cr.Comment);
            SetProperty(commentField, "EnableVisible", true);
        }

        public void changeCommentar()
        {
            if (index > -1) { 
            var crNew = criteriaMap[index];
            if (crNew is Criterion)
            {
                ((Criterion)crNew).Comment = (String)GetProperty(GetControlByName("commentField"), "Text");
                
            }
            }
             
        }
        public void ShowEvaluation()
        {
            //SetProperty(GetControlByName(prioExplanationLabelName), "EnableVisible", true);
            SetProperty(GetControlByName(prevButtonName), "EnableVisible", false);
            SetProperty(GetControlByName(nextButtonName), "EnableVisible", false);
            SetProperty(GetControlByName(inspectionReportButtonName), "EnableVisible", true);
            SetProperty(GetControlByName(errorReportButtonName), "EnableVisible", true);
            SetProperty(GetControlByName(evalButtonName), "EnableVisible", false);
            SetProperty(GetControlByName(introductionLabelName), "EnableVisible", true);
            SetProperty(GetControlByName(resultLabelName), "EnableVisible", true);
            SetProperty(GetControlByName(evalBoxName), "EnableVisible", true);
            SetProperty(GetControlByName(evalHeaderName), "EnableVisible", true);
            SetProperty(GetControlByName(evalResultLabel), "EnableVisible", true);
            SetProperty(GetControlByName(evalResultLabel),"Label", eval.finalerg);
            SetProperty(GetControlByName(resultLabelName), "Label", "Bestandene Kriterien: " + eval.passCount + "/" + eval.criteriaCount);
            container = createScrollableContainerResult(100, 40, 255, 180, scrollContainerResultName);
            
            //util.Debug.GetAllProperties(container as XControl);
            
            //container.SetVisible(true);
           
        }

        public void showResults(Criterion cr,int step)
        {
            if (cr != null)
            {
                SetProperty(GetControlByName(introductionLabelName), "EnableVisible", false);
                changeActiveResultLabel(cr.Id.ToString());
                SetProperty(GetControlByName(toCriterionButtonName), "EnableVisible", true);
                SetProperty(GetControlByName(toCriterionButtonName), "HelpURL", cr.Id);
                util.Debug.GetAllProperties(GetControlByName(toCriterionButtonName));
                SetProperty(GetControlByName(toCriterionButtonName), "FocusOnClick", false);
                SetProperty(GetControlByName(testPointsSkalaName), "EnableVisible", true);
                SetProperty(GetControlByName(testPointLabelName), "EnableVisible", true);
                SetProperty(GetControlByName(criterionHeaderResultName), "EnableVisible", true);
                SetProperty(GetControlByName(testPointHeaderName), "EnableVisible", true);
                SetProperty(GetControlByName(commentarResultLabelName), "EnableVisible", true);
                commentarResultContainer.SetVisible(true);
                testPointsContainer.SetVisible(true);
                testPointsContainer.InnerPosY = 0;
                testPointsContainer.VerticalScrlBar.setValue(0);
                setTestPoints(testPointsContainer,cr);
                commentarResultContainer.InnerPosY = 0;
                commentarResultContainer.VerticalScrlBar.setValue(0);
                addCommentarLabel(commentarResultContainer, cr.Comment);
                
               
                if(cr.Res.resultType == ResultType.fail)
                {
                    SetProperty(GetControlByName(criterionHeaderResultName), "Label", "Das Kriterium '" + cr.Name + "' wurde nicht bestanden.");
                    SetProperty(GetControlByName(testPointHeaderName), "Label", "Nicht erfüllte Prüfpunkte:");
                }
                if (cr.Res.resultType == ResultType.fail && cr.Rec.Type == CriterionType.one)
                {
                    SetProperty(GetControlByName(criterionHeaderResultName), "Label", "Das Kriterium '" + cr.Name + "' wurde nicht bestanden. Es muss einer der beiden Prüfpunkte bestehen.");
                    SetProperty(GetControlByName(testPointHeaderName), "Label", "Nicht erfüllte Prüfpunkte:");
                }
                
                if (cr.Res.resultType == ResultType.passwithwarning&&cr.Rec.Type==CriterionType.rating)
                {
                    SetProperty(GetControlByName(testPointHeaderName), "Label", "Verbesserungswürdige Prüfpunkte");
                    SetProperty(GetControlByName(criterionHeaderResultName), "Label", "Das Kriterium '" + cr.Name + "' wurde bestanden, kann aber verbessert werden.");
                }
                if (cr.Res.resultType == ResultType.passwithwarning && cr.Rec.Type == CriterionType.count)
                {
                    SetProperty(GetControlByName(testPointHeaderName), "Label", "Nicht erfüllte Prüfpunkte:");
                    SetProperty(GetControlByName(criterionHeaderResultName), "Label", "Das Kriterium '" + cr.Name + "' wurde bestanden, kann aber verbessert werden.");
                }
                
            }
        }

        private void setTestPoints(ScrollableContainer testPointsContainer, Criterion cr)
        {
            foreach (XControl entry in testPointsMap.Values) 
            {
                entry.dispose();
            }
            

                testPointsMap.Clear();
              
            
                addFailTestPoints(testPointsContainer,cr);
            
           
        }
        private void addFailTestPoints(ScrollableContainer SC, Criterion cr)
        {
            if (cr.Rec.Type == CriterionType.all || cr.Rec.Type == CriterionType.count||cr.Rec.Type==CriterionType.one)
            {
                for (int i = 0; i < cr.Res.Rating.Count; i++)
                {
                    if (cr.Res.Rating[i] == 0)
                    {
                        var testPoint = CreateFixedLabel("-" + cr.Rec.Items[i].Desc + cr.Rec.Items[i].VarText, 0, 0, 0, 0, 10, null, "item" + i);
                        SetProperty(testPoint, "MultiLine", true);
                        var l = SC.AddElementToTheEndAndAdoptTheSize(testPoint as XControl, "item" + i, 5, 5);

                        testPointsMap.Add("item" + i, l);
                    }

                }
            }
            if (cr.Rec.Type == CriterionType.rating)
            {
                
                        var testPoint = CreateFixedLabel("-" + cr.Rec.Items[0].Desc + cr.Rec.Items[0].VarText+" (Ratingergebnis "+cr.Res.ratingerg* 100+"%)", 0, 0, 0, 0, 10, null, "item" + 0);
                        SetProperty(testPoint, "MultiLine", true);
                        var l = SC.AddElementToTheEndAndAdoptTheSize(testPoint as XControl, "item" + 0, 5, 5);

                        testPointsMap.Add("item" + 0, l);
                    

                }
            
            }
        
        public List<Category> getCategoriesFromMap()
        {
            List<Category> li = new List<Category>();
            foreach (Object c in criteriaMap.Values)
            {
                //if (c is Category) System.Console.WriteLine(((Category)c).Criteria[0].Res.Rating[0] +"test");
                if (c is Category) li.Add(c as Category);
            }
            return li;
        }
        
    }
}


  