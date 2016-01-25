using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using unoidl.com.sun.star.lang;
using unoidl.com.sun.star.awt;
using unoidl.com.sun.star.beans;
using unoidl.com.sun.star.util;
using tud.mci.tangram.qsdialog.models;
using tud.mci.tangram.models;
using System.Xml;
using System.IO;



namespace tud.mci.tangram.qsdialog.Dialogs
{

    public partial class UnoDialogSample
    {

        #region EventListener implementations

        #region XTextListener - makes it possible to receive text change events.

        /// <summary>
        /// is invoked when the text has changed. 
        /// </summary>
        /// <param name="textEvent">The text event.</param>
        public void textChanged(TextEvent textEvent)
        {
            try
            {
                //var textBox = textEvent.Source;
                //String textBoxName = (String)GetProperty(textBox,"Name");
                //if(textBoxName.Equals("commentField")){


                //var crNew = criteriaMap[index];
                //if (crNew is Criterion)
                //{
                //    ((Criterion)crNew).Comment = GetProperty(textBox, "Text") as String;
                //    util.Debug.GetAllProperties(textEvent.Source);
                //}
                //}
                // get the control that has fired the event,
                XControl xControl = (XControl)textEvent.Source;
                XControlModel xControlModel = xControl.getModel();
                XPropertySet xPSet = (XPropertySet)xControlModel;
                String sName = (String)xPSet.getPropertyValue("Name").Value;
                // just in case the listener has been added to several controls,
                // we make sure we refer to the right one
                if (sName.Equals("TextField1"))
                {
                    String sText = (String)xPSet.getPropertyValue("Text").Value;
                    System.Diagnostics.Debug.WriteLine(sText);
                    // insert your code here to validate the text of the control...
                }
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
        }

        #endregion

        #region XSpinListener - makes it possible to receive spin events.

        /// <summary>
        /// is invoked when the spin field is spun up. 
        /// </summary>
        /// <param name="spinEvent">The spin event.</param>
        public void up(SpinEvent spinEvent)
        {
            try
            {
                // get the control that has fired the event,
                XControl xControl = (XControl)spinEvent.Source;
                XControlModel xControlModel = xControl.getModel();
                XPropertySet xPSet = (XPropertySet)xControlModel;
                String sName = (String)xPSet.getPropertyValue("Name").Value;
                // just in case the listener has been added to several controls,
                // we make sure we refer to the right one
                if (sName.Equals("FormattedField1"))
                {
                    double fvalue = (double)(xPSet.getPropertyValue("EffectiveValue")).Value;
                    System.Diagnostics.Debug.WriteLine("Controlvalue:  " + fvalue);
                    // insert your code here to validate the value of the control...
                }
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
        }
        /// <summary>
        /// is invoked when the spin field is spun down. 
        /// </summary>
        /// <param name="spinEvent">The spin event.</param>
        public void down(SpinEvent spinEvent)
        {
        }
        /// <summary>
        /// is invoked when the spin field is set to the upper value. 
        /// </summary>
        /// <param name="spinEvent">The spin event.</param>
        public void last(SpinEvent spinEvent)
        {
        }
        /// <summary>
        /// is invoked when the spin field is set to the lower value. 
        /// </summary>
        /// <param name="spinEvent">The spin event.</param>
        public void first(SpinEvent spinEvent)
        {
        }
        public void disposing(EventObject rEventObject)
        {
        }

        #endregion

        #region XActionListener - makes it possible to receive action events.

        /// <summary>
        /// is invoked when an action is performed.
        /// </summary>
        /// <param name="rEvent">The r event.</param>
        public void actionPerformed(ActionEvent rEvent)
        {

            if (rEvent != null)
            {
                if (rEvent.Source != null)
                {
                    changeCommentar();
                    var recomBox = GetControlByName("FrameControl3");
                    var helpBox = GetControlByName("FrameControl2");
                    var a = rEvent.Source;
                    String buttonName = (String)GetProperty(a, "Name");
                    switch (buttonName)
                    {
                        case "nextButtonLabel":
                            SetProperty(GetControlByName(prevButtonName), "EnableVisible", true);
                            if (index < criteriaMap.Count - 1)
                            {
                                if (hide) showAll();

                                index++;
                                var crNew = criteriaMap[index];


                                if (crNew is Category)
                                {
                                    //XControl crLabel = criteriaLabelMap[(crNew as Category).Id] as XControl;

                                    changeContentCategory((crNew as Category).Id);
                                    // changeContent(crNew as Category)
                                }
                                else
                                {
                                    XControl crLabel = criteriaLabelMap[(crNew as Criterion).Id] as XControl;
                                    var Step = GetProperty(crLabel, "HelpURL");
                                    SetProperty(recomBox, "EnableVisible", true);
                                    SetProperty(helpBox, "EnableVisible", true);
                                    changeContent((crNew as Criterion).Id, Convert.ToInt16(Step));
                                }

                            }
                            else
                            {
                                //handleStepChange(5, null);
                                SetProperty(rEvent.Source, "EnableVisible", false);
                            }

                            break;

                        case "prevButtonLabel":
                            SetProperty(GetControlByName(nextButtonName), "EnableVisible", true);
                            if (index > 0)
                            {
                                if (hide) showAll();

                                index--;
                                var crNew = criteriaMap[index];
                                if (crNew is Category)
                                {
                                    changeContentCategory((crNew as Category).Id);
                                }
                                else
                                {
                                    SetProperty(recomBox, "EnableVisible", true);
                                    SetProperty(helpBox, "EnableVisible", true);
                                    XControl crLabel = criteriaLabelMap[(crNew as Criterion).Id] as XControl;
                                    var Step = GetProperty(crLabel, "HelpURL");
                                    changeContent((crNew as Criterion).Id, Convert.ToInt16(Step));
                                }
                                //Criterion crNew = criteriaMap[index] as Criterion;
                                //XControl crLabel = criteriaLabelMap[crNew.Id] as XControl;
                                //var Step = GetProperty(crLabel, "HelpURL");
                                //changeContent(crNew.Id, Convert.ToInt16(Step));

                            }
                            
                            else
                            {
                                SetProperty(rEvent.Source, "EnableVisible", false);

                                index--;
                                showIntroduction();

                            }
                            break;
                        case evalButtonName:
                            try
                            {
                                hideAll();

                                handleStepChange(5, null);



                                earl.createEmptyEarlGraph(chosenMediaType);
                                eval.evaluate(getCategoriesFromMap(), new tud.mci.tangram.qsdialog.models.Parameters(), earl);
                                this.earl = eval.earl;
                                earl.writeEarlRdf();
                                //earl.testFunction();

                                //TODO: make this via dialog
                                // write to Doc
                                //var errWrDoc = new tud.mci.tangram.exports.WriterErrorExport(getCategoriesFromMap(), eval);
                                //errWrDoc.WriteDocument("", "");
                                //var errWrDoc = new tud.mci.tangram.exports.WriterInspectionReport(getCategoriesFromMap(), eval);
                                //errWrDoc.WriteDocument("", "");

                                ShowEvaluation();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("exception!!!!"+ ex);
                               
                            }
                            break;
                        case earlLoadButtonName:

                            XmlDocument xDoc = new XmlDocument();
                            //string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                            string fullPath = typeof(UnoDialogSample).Assembly.Location;
                            string baseDir = Path.GetDirectoryName(fullPath);
                            if (File.Exists(baseDir + @"\document.rdf")) { 
                            xDoc.Load(baseDir + @"\document.rdf");
                            earlReader.updateDialog(this.criteriaMap, xDoc, this);
                            }

                            
                            break;

                        case toCriterionButtonName:
                            MouseEvent mousEvent = new MouseEvent();
                            mousEvent.Source = criteriaLabelMap[GetProperty(rEvent.Source, "HelpURL")];
                            mouseReleased(mousEvent);
                            break;
                            //mouseReleased();
                        default:
                            System.Diagnostics.Debug.WriteLine("Button '" + buttonName + "' pressed");
                            break;

                        case inspectionReportButtonName:
                            var inspRep = new tud.mci.tangram.exports.WriterInspectionReport(getCategoriesFromMap(), eval);
                            inspRep.WriteDocument("", "");
                            break;

                        case errorReportButtonName:
                        var errWrDoc = new tud.mci.tangram.exports.WriterErrorExport(getCategoriesFromMap(), eval);
                        errWrDoc.WriteDocument("", "");
                        break;
                    }
                    
                }


                try
                {
                    // get the control that has fired the event,
                    XControl xControl = (XControl)rEvent.Source;
                    XControlModel xControlModel = xControl.getModel();
                    XPropertySet xPSet = (XPropertySet)xControlModel;
                    String sName = (String)xPSet.getPropertyValue("Name").Value;
                    // just in case the listener has been added to several controls,
                    // we make sure we refer to the right one
                    if (sName.Equals("CommandButton1"))
                    {
                        //...
                    }
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
            }
        }



        #endregion

        #region XFocusListener - makes it possible to receive keyboard focus events.
        //The window which has the keyboard focus is the window which gets the keyboard events.

        /// <summary>
        /// is invoked when a window loses the keyboard focus. 
        /// </summary>
        /// <param name="_focusEvent">The _focus event.</param>
        public void focusLost(FocusEvent _focusEvent)
        {
            short nFocusFlags = _focusEvent.FocusFlags;
            int nFocusChangeReason = nFocusFlags & FocusChangeReason.TAB;
            if (nFocusChangeReason == FocusChangeReason.TAB)
            {
                // get the window of the Window that has gained the Focus...
                // Note that the xWindow is just a representation of the controlwindow
                // but not of the control itself
                XWindow xWindow = (XWindow)_focusEvent.NextFocus;
            }
        }

        /// <summary>
        /// is invoked when a window gains the keyboard focus.
        /// </summary>
        /// <param name="focusEvent">The focus event.</param>
        public void focusGained(FocusEvent focusEvent)
        {
        }

        #endregion

        #region XMouseListener - makes it possible to receive events from the mouse in a certain window.

        /// <summary>
        /// is invoked when a mouse button has been released on a window. 
        /// </summary>
        /// <param name="mouseEvent">The mouse event.</param>
        public void mouseReleased(MouseEvent mouseEvent)
        {
            //here the step magic happens
            if (mouseEvent != null && mouseEvent.Source != null)
            {
                //util.Debug.GetAllServicesOfObject(mouseEvent.Source);
                //var ps = GetPropertysetOfControl(mouseEvent.Source);
                //util.Debug.GetAllProperties(mouseEvent.Source);
                //this.index = (int)GetProperty(mouseEvent.Source, "Index");
                //var tagObj2 = GetProperty(mouseEvent.Source, "Tag");
                var helpText = GetProperty(mouseEvent.Source, "HelpText");
                var tagObj = GetProperty(mouseEvent.Source, "HelpURL");
                var kriterienTag = GetProperty(mouseEvent.Source, "Label");
                if (tagObj != null && kriterienTag.ToString() != "Kriterien" && !helpText.ToString().StartsWith("result"))
                {

                    int step = 0;
                    try
                    {

                        if (hide) showAll();
                        changeCommentar();
                        SetProperty(GetControlByName(prevButtonName), "EnableVisible", true);
                        //set the right index
                        var crID = GetProperty(mouseEvent.Source, "HelpText");

                        this.index = getIndexByKey(criteriaMap, crID as String);
                        var helpBox1 = GetControlByName("FrameControl");
                        var descBox1 = GetControlByName("FrameControl2");
                        var recomBox1 = GetControlByName("FrameControl3");

                        SetProperty(helpBox1, "EnableVisible", true);
                        SetProperty(descBox1, "EnableVisible", true);
                        SetProperty(recomBox1, "EnableVisible", true);
                        //step = Int32.Parse(tagObj.ToString());
                        if (criteriaMap[crID] is Category)
                        {
                            changeContentCategory(crID.ToString());
                        }
                        else if (crID.ToString().Equals("I"))
                        {
                            showIntroduction();
                        }
                        else
                        {
                            step = Int32.Parse(tagObj.ToString());
                            changeContent(crID, step);

                        }
                        var prevButton = GetControlByName(prevButtonName);
                        var nextButton = GetControlByName(nextButtonName);
                        if (index == criteriaMap.Count - 1)
                        {

                            SetProperty(nextButton, "EnableVisible", false);
                            return;
                        }

                        if (index == -1)
                        {

                            SetProperty(prevButton, "EnableVisible", false);
                            return;
                        }
                        SetProperty(nextButton, "EnableVisible", true);
                        SetProperty(prevButton, "EnableVisible", true);


                    }
                    catch { return; }
                }
                if (tagObj != null && kriterienTag.ToString() != "Kriterien" && helpText.ToString().StartsWith("result"))

                {
                    String crID = helpText.ToString();
                    char[] MyChar = {'r','e','s','u','l','t'};
                    crID = crID.TrimStart(MyChar);
                    int step1 = Int32.Parse(tagObj.ToString());
                    
                    showResults(criteriaMap[crID] as Criterion,step1);   


                }

            }

        }
        /// <summary>
        ///     is invoked when a mouse button has been pressed on a window. 
        ///     Since mouse presses are usually also used to indicate requests for popup 
        ///     menus (also known as context menus) on objects, you might receive two 
        ///     events for a single mouse press: For example, if, on your operating system, 
        ///     pressing the right mouse button indicates the request for a context menu, 
        ///     then you will receive one call to mousePressed indicating the mouse click, 
        ///     and another one indicating the context menu request. For the latter, the 
        ///     MouseEvent::PopupTrigger member of the event will be set to true.
        /// </summary>
        /// <param name="mouseEvent">The mouse event.</param>
        public void mousePressed(MouseEvent mouseEvent)
        {
        }
        /// <summary>
        /// is invoked when the mouse exits a window.
        /// </summary>
        /// <param name="mouseEvent">The mouse event.</param>
        public void mouseExited(MouseEvent mouseEvent)
        {
        }
        /// <summary>
        /// is invoked when the mouse enters a window. 
        /// </summary>
        /// <param name="_mouseEvent">The _mouse event.</param>
        public void mouseEntered(MouseEvent _mouseEvent)
        {
            try
            {
                // retrieve the control that the event has been invoked at...
                XControl xControl = (XControl)_mouseEvent.Source;
                Object tk = MXMcf.createInstanceWithContext("com.sun.star.awt.Toolkit", MXContext);
                XToolkit xToolkit = (XToolkit)tk;
                // create the peer of the control by passing the windowpeer of the parent
                // in this case the windowpeer of the control
                xControl.createPeer(xToolkit, MXWindowPeer);
                // create a pointer object "in the open countryside" and set the type accordingly...
                Object oPointer = this.MXMcf.createInstanceWithContext("com.sun.star.awt.Pointer", this.MXContext);
                XPointer xPointer = (XPointer)oPointer;
                xPointer.setType(unoidl.com.sun.star.awt.SystemPointer.REFHAND);
                // finally set the created pointer at the windowpeer of the control
                xControl.getPeer().setPointer(xPointer);
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
        }

        #endregion

        #region XItemListener - makes it possible to receive events from a component when the state of an item changes.

        /// <summary>
        /// is invoked when an item changes its state. 
        /// </summary>
        /// <param name="itemEvent">The item event.</param>
        public void itemStateChanged(ItemEvent itemEvent)
        {
            try
            {
                if (itemEvent.Source is XCheckBox)
                {

                    #region checkbox
                    #region checkbox for Step 1
                    Criterion cr = (Criterion)criteriaMap[index];
                    String recomType = cr.Rec.Type.ToString();
                    System.Console.WriteLine(recomType);
                    switch (recomType)
                    {
                        case "all":
                            String checkboxName = GetProperty(itemEvent.Source, "Name") as String;
                            int checkBoxIndex = Convert.ToInt16(checkboxName.Replace("CheckBox", ""));


                            int checkBoxResult = (itemEvent.Source as XCheckBox).getState();
                            cr.Res.SetItemRating(checkBoxIndex, checkBoxResult);
                            break;

                        case "count":
                            String checkboxStep3Name = GetProperty(itemEvent.Source, "Name") as String;
                            int checkBoxStep3Index = Convert.ToInt16(checkboxStep3Name.Replace("CheckBoxStep3", ""));


                            int checkBoxStep3Result = (itemEvent.Source as XCheckBox).getState();
                            cr.Res.SetItemRating(checkBoxStep3Index, checkBoxStep3Result);
                            break;

                    }
                    #endregion

                    // retrieve the control that the event has been invoked at...
                    XCheckBox xCheckBox = (XCheckBox)itemEvent.Source;
                    // retrieve the control that we want to disable or enable
                    XControl xControl = (XControl)MXDlgContainer.getControl("CommandButton1");
                    if (xControl != null)
                    {
                        XPropertySet xModelPropertySet = (XPropertySet)xControl.getModel();
                        short nState = xCheckBox.getState();
                        bool bdoEnable = true;
                        switch (nState)
                        {
                            case 1:     // checked
                                bdoEnable = true;
                                break;
                            case 0:     // not checked
                            case 2:     // don't know
                                bdoEnable = false;
                                break;
                        }

                        // Alternatively we could have done it also this way:
                        // bdoEnable = (nState == 1);
                        xModelPropertySet.setPropertyValue("Enabled", Any.Get(bdoEnable));

                    }


                    #endregion

                }
                else
                {
                    if (itemEvent.Source is XRadioButton)
                    {
                        //write results for rating box
                        //TODO: write only for one rating box
                        //TODO: write this for more than one , RatingBox0, RatingBox1 , etc 
                        String name = GetProperty(itemEvent.Source, "Name") as String;
                        //util.Debug.GetAllProperties(itemEvent.Source);
                        if (name.Equals("All"))
                        {
                            chosenMediaType = MediaType.All;
                            (itemEvent.Source as XRadioButton).setState(true);
                        }
                        if (name.Equals("Schwellpapier"))
                        {
                            chosenMediaType = MediaType.Schwellpapier;
                            (itemEvent.Source as XRadioButton).setState(true);
                        }
                        if (name.Equals("Tiger"))
                        {
                            chosenMediaType = MediaType.Tiger;
                            (itemEvent.Source as XRadioButton).setState(true);
                        }
                        if (name.Equals("Stiftplatte"))
                        {
                            chosenMediaType = MediaType.Stiftplatte;
                            (itemEvent.Source as XRadioButton).setState(true);
                        }
                        if (name.Contains("RATING"))
                        {
                            //TODO
                            //int ratingBoxNumber = Convert.ToInt16(name.Replace("RatingBox", "");
                            int rating = Convert.ToInt16(name.Replace("RatingBox0_RATING_", ""));
                            Criterion cr = (Criterion)criteriaMap[index];
                            for (int i = 0; i < 5; i++)
                            {
                                if (i == rating)
                                {
                                    cr.Res.SetItemRating(rating, 1);
                                }
                                else cr.Res.SetItemRating(i, 0);
                            }
                            //System.Console.WriteLine("rating");
                        }
                        if (name.Contains("RadioButton"))
                        {
                            int radioBoxValue = Convert.ToInt16(name.Replace("RadioButton", ""));
                            Criterion cr = (Criterion)criteriaMap[index];
                            for (int i = 0; i < cr.getCountOfItems(); i++)
                            {
                                if (i == radioBoxValue) cr.Res.Rating[radioBoxValue] = 1;
                                else { cr.Res.Rating[i] = 0; }
                            }

                        }

                    }
                    //util.Debug.GetAllInterfacesOfObject(itemEvent.Source);
                    //util.Debug.GetAllServicesOfObject(itemEvent.Source);

                    if (util.OoUtils.ElementSupportsService(itemEvent.Source, "com.sun.star.awt.UnoControlRoadmap"))
                    {
                        var ps = GetPropertysetOfControl(itemEvent.Source as XControl);
                        util.Debug.GetAllProperties(ps);

                        handleStepChange(itemEvent.ItemId, itemEvent.Source);
                    }

                }



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
        }

        private void handleStepChange(int step, Object source)
        {
            //get the dialog model
            if (MXDialogControl != null)
            {
                
                //System.Console.WriteLine(((Criterion)criteriaMap[index]).Rec.Type);
                //MXDialogControl
                
                SetStepProperty(MXModel, step);
                if (testPointsContainer != null)
                {
                    testPointsContainer.SetVisible(false);
                    
                }
                if (container != null)
                {
                    container.Dispose();
                }
                if (commentarResultContainer != null)
                {
                    commentarResultContainer.SetVisible(false);
                }

                System.Console.WriteLine("HANDLE The Step " + step);
                
                //foreach (Criterion cr in allcriteria)
                //{
                //    var a = 
                //    if(cr.Id.Equals())
                //}
                //if (step == 1)
                //{

                //    //set help text helpTextName_Step1
                //    var hc = GetControlByName(helpTextName);
                //    if (hc != null)
                //    {
                //        SetProperty(hc, "Label", "Schmu" + DateTime.Now.ToString());
                //    }

                //    System.Console.WriteLine("das ist eine Step 1 Zeile und es müsste eigentlich der Header geändert werden");
                //}



                //var header = GetControlByName(headerName);

                //FIXME: only for fixing
                //util.Debug.GetAllServicesOfObject(header);

                //var ps = GetPropertysetOfControl(header as XControl);
                //util.Debug.GetAllProperties(ps);

                //SetProperty(header, "Label", step.ToString());

                //foreach(Category ca in categories){
                //   SetProperty(roadMap)


                //WICHTIG
                //SetProperty(source, "Text", step.ToString());


                //TODO: get CR Object (zugehöriges Kriterium)
                //TODO: Titel setzen --> Feld finden (zugehöriger step)
                // Desc
                // Hilfe
                // Bewertung
                // evt. zurückbutton ein- und ausblenden

            }
        }

        #endregion

        #region XAdjustmentListener - makes it possible to receive adjustment events.

        /// <summary>
        /// is invoked when the adjustment has changed.
        /// </summary>
        /// <param name="_adjustmentEvent">The _adjustment event.</param>
        public void adjustmentValueChanged(AdjustmentEvent _adjustmentEvent)
        {
            switch (_adjustmentEvent.Type)
            {
                case AdjustmentType.ADJUST_ABS: // adjustment is originated by dragging the thumb.  
                    System.Diagnostics.Debug.WriteLine("The event has been triggered by dragging the thumb...");
                    break;
                case AdjustmentType.ADJUST_LINE: // adjustment is originated by a line jump. 
                    // A line jump can, for example, be caused by a click on one of the pointer buttons.
                    System.Diagnostics.Debug.WriteLine("The event has been triggered by a single line move..");
                    break;
                case AdjustmentType.ADJUST_PAGE: // adjustment is originated by a page jump.  
                    // A page jump can, for example, be caused by a click in the background area of the scrollbar (neither one of the pointer buttons, nor the thumb).
                    System.Diagnostics.Debug.WriteLine("The event has been triggered by a block move...");
                    break;
            }
            System.Diagnostics.Debug.WriteLine("The value of the scrollbar is: " + _adjustmentEvent.Value);
        }

        #endregion

        #region XKeyHandler - This key handler is similar to XKeyListener but allows the consumption of key events.

        /*
         * If a key event is consumed by one handler both the following handlers, 
         * with respect to the list of key handlers of the broadcaster, and a 
         * following handling by the broadcaster will not take place. 
         */

        /// <summary>
        /// This function is called by the broadcaster, an XExtendedToolkit for 
        /// instance, after a key has been pressed but before it is released. 
        /// The return value decides about whether other handlers will be called 
        /// and a handling by the broadcaster will take place. 
        /// Consume the event if the action performed by the implementation is 
        /// mutually exclusive with the default action of the broadcaster or, 
        /// when known, with that of other handlers.
        /// Consuming this event does not prevent the pending key-release event 
        /// from beeing broadcasted.
        /// 
        /// When false is returned the other handlers are called and a following 
        /// handling of the event by the broadcaster takes place. Otherwise, when 
        /// true is returned, no other handler will be called and the broadcaster 
        /// will take no further actions regarding the event. 
        /// </summary>
        /// <param name="keyEvent">The key event informs about the pressed key. </param>
        public void keyReleased(KeyEvent keyEvent)
        {
            int i = keyEvent.KeyChar;
            int n = keyEvent.KeyCode;
            int m = keyEvent.KeyFunc;
        }
        /// <summary>
        /// This function is called by the broadcaster, an XExtendedToolkit for 
        /// instance, after a key has been pressed and released. The return 
        /// value decides about whether other handlers will be called and a 
        /// handling by the broadcaster will take place. 
        /// Consume the event if the action performed by the implementation is 
        /// mutualy exclusive with the default action of the broadcaster or, 
        /// when known, with that of other handlers.
        /// 
        /// When false is returned the other handlers are called and a following 
        /// handling of the event by the broadcaster takes place. Otherwise, 
        /// when true is returned, no other handler will be called and the 
        /// broadcaster will take no further actions regarding the event. 
        /// </summary>
        /// <param name="keyEvent">The key event.</param>
        public void keyPressed(KeyEvent keyEvent)
        {
        }

        #endregion

        #region XSelectionChangeListener - makes it possible to receive an event when the current selection changes.

        /// <summary>
        /// is called when the selection changes. 
        /// You can get the new selection via XSelectionSupplier from 
        /// ::com::sun::star::lang::EventObject::Source. 
        /// </summary>
        /// <param name="aEvent">A event.</param>
        public void selectionChanged(EventObject aEvent)
        {

        }

        #endregion

        #endregion
    }
}
