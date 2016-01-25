using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using unoidl.com.sun.star.beans;
using unoidl.com.sun.star.awt;
using unoidl.com.sun.star.view;
using unoidl.com.sun.star.awt.tree;
using unoidl.com.sun.star.lang;
using unoidl.com.sun.star.container;
using unoidl.com.sun.star.util;
using unoidl.com.sun.star.ucb;
using tud.mci.tangram.models;
using tud.mci.tangram.models.dialogs;
using tud.mci.tangram.util;
using tud.mci.tangram.qsdialog.models;
using tud.mci.tangram.dialog.controlls;
using System.Threading;
using System.Collections.Specialized;

namespace tud.mci.tangram.qsdialog.Dialogs
{

    public partial class UnoDialogSample
    {
        /* * directly AVAILABE MODELS on this dialog:
 * 
    com.sun.star.awt.UnoControlEditModel
    com.sun.star.awt.UnoControlFormattedFieldModel
    com.sun.star.awt.UnoControlFileControlModel
    com.sun.star.awt.UnoControlButtonModel
    com.sun.star.awt.UnoControlImageControlModel
    com.sun.star.awt.UnoControlRadioButtonModel
    com.sun.star.awt.UnoControlCheckBoxModel
    com.sun.star.awt.UnoControlFixedTextModel
    com.sun.star.awt.UnoControlGroupBoxModel
    com.sun.star.awt.UnoControlListBoxModel
    com.sun.star.awt.UnoControlComboBoxModel
    com.sun.star.awt.UnoControlDateFieldModel
    com.sun.star.awt.UnoControlTimeFieldModel
    com.sun.star.awt.UnoControlNumericFieldModel
    com.sun.star.awt.UnoControlCurrencyFieldModel
    com.sun.star.awt.UnoControlPatternFieldModel
    com.sun.star.awt.UnoControlProgressBarModel
    com.sun.star.awt.UnoControlScrollBarModel
    com.sun.star.awt.UnoControlFixedLineModel
    com.sun.star.awt.UnoControlRoadmapModel
    com.sun.star.awt.tree.TreeControlModel
    com.sun.star.awt.grid.UnoControlGridModel
    com.sun.star.awt.tab.UnoControlTabPageContainerModel
    com.sun.star.awt.tab.UnoControlTabPageModel
 */

        #region Controll element creation and insertion

        public Object insertGroupBox(int _nPosX, int _nPosY, int _nHeight, int _nWidth, string label = "~", String sName = "")
        {
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "FrameControl");

                // create a controlmodel at the multiservicefactory of the dialog model...
                Object oGBModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_GROUP_BOX_MODEL, MXContext);
                XMultiPropertySet xGBModelMPSet = (XMultiPropertySet)oGBModel;

                // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                xGBModelMPSet.setPropertyValues(
                        new String[] { "Height", "Name", "PositionX", "PositionY", "Width" },
                       tud.mci.tangram.models.Any.Get(new Object[] { _nHeight, sName, _nPosX, _nPosY, _nWidth }));

                // The controlmodel is not really available until inserted to the Dialog container
                MXDlgModelNameContainer.insertByName(sName, Any.Get(oGBModel));

                // The following property may also be set with XMultiPropertySet but we
                // use the XPropertySet interface merely for reasons of demonstration
                XPropertySet xGBPSet = (XPropertySet)oGBModel;
                xGBPSet.setPropertyValue("Label", Any.Get(label));

                return oGBModel;
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

            return null;
        }

        public XTextComponent insertEditField(XTextListener _xTextListener, XFocusListener _xFocusListener, int _nPosX, int _nPosY, int _nWidth, String sName = "")
        {
            XTextComponent xTextComponent = null;
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "EDIT");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);


                // create a controlmodel at the multiservicefactory of the dialog model...
                Object oTFModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_EDIT_MODEL, this.MXContext);
                XMultiPropertySet xTFModelMPSet = (XMultiPropertySet)oTFModel;

                // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                xTFModelMPSet.setPropertyValues(
                        new String[] { "Height", "Name", "PositionX", "PositionY", "Text", "Width" },
                       Any.Get(new Object[] { 12, sName, _nPosX, _nPosY, "MyText", _nWidth }));

                // The controlmodel is not really available until inserted to the Dialog container
                MXDlgModelNameContainer.insertByName(sName, Any.Get(oTFModel));
                XPropertySet xTFModelPSet = (XPropertySet)oTFModel;

                // The following property may also be set with XMultiPropertySet but we
                // use the XPropertySet interface merely for reasons of demonstration
                xTFModelPSet.setPropertyValue("EchoChar", Any.Get((short)'*'));
                XControl xTFControl = GetControlByName(sName);

                // add a textlistener that is notified on each change of the controlvalue...
                xTextComponent = (XTextComponent)xTFControl;
                XWindow xTFWindow = (XWindow)xTFControl;
                xTFWindow.addFocusListener(_xFocusListener);
                xTextComponent.addTextListener(_xTextListener);
                xTFWindow.addKeyListener(this);
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
            return xTextComponent;
        }

        public XPropertySet insertTimeField(int _nPosX, int _nPosY, int _nWidth, int _nTime, int _nTimeMin, int _nTimeMax, String sName = "")
        {
            XPropertySet xTFModelPSet = null;
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "TIME_FIELD");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                // create a controlmodel at the multiservicefactory of the dialog model...
                Object oTFModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_TIME_FIELD_MODEL, MXContext);
                XMultiPropertySet xTFModelMPSet = (XMultiPropertySet)oTFModel;

                // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                xTFModelMPSet.setPropertyValues(
                        new String[] { "Height", "Name", "PositionX", "PositionY", "Spin", "Width" },
                        Any.Get(new Object[] { 12, sName, _nPosX, _nPosY, true, _nWidth }));

                // The controlmodel is not really available until inserted to the Dialog container
                MXDlgModelNameContainer.insertByName(sName, Any.Get(oTFModel));
                xTFModelPSet = (XPropertySet)oTFModel;

                // The following properties may also be set with XMultiPropertySet but we
                // use the XPropertySet interface merely for reasons of demonstration
                xTFModelPSet.setPropertyValue("TimeFormat", Any.Get((short)5));
                xTFModelPSet.setPropertyValue("TimeMin", Any.Get(_nTimeMin));
                xTFModelPSet.setPropertyValue("TimeMax", Any.Get(_nTimeMax));
                xTFModelPSet.setPropertyValue("Time", Any.Get(_nTime));
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
            return xTFModelPSet;
        }

        public XPropertySet insertDateField(XSpinListener _xSpinListener, int _nPosX, int _nPosY, int _nWidth, String sName = "")
        {
            XPropertySet xDFModelPSet = null;
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "DATE_FIELD");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                // create a controlmodel at the multiservicefactory of the dialog model...
                Object oDFModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_DATE_FIELD_MODEL, MXContext);
                XMultiPropertySet xDFModelMPSet = (XMultiPropertySet)oDFModel;

                // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                xDFModelMPSet.setPropertyValues(
                        new String[] { "Dropdown", "Height", "Name", "PositionX", "PositionY", "Width" },
                        Any.Get(new Object[] { true, 12, sName, _nPosX, _nPosY, _nWidth }));

                // The controlmodel is not really available until inserted to the Dialog container
                MXDlgModelNameContainer.insertByName(sName, Any.Get(oDFModel));
                xDFModelPSet = (XPropertySet)oDFModel;

                // The following properties may also be set with XMultiPropertySet but we
                // use the XPropertySet interface merely for reasons of demonstration
                xDFModelPSet.setPropertyValue("DateFormat", Any.Get((short)7));
                xDFModelPSet.setPropertyValue("DateMin", Any.Get(20070401));
                xDFModelPSet.setPropertyValue("DateMax", Any.Get(20070501));
                xDFModelPSet.setPropertyValue("Date", Any.Get(20000415));
                Object oDFControl = GetControlByName(sName);

                // add a SpinListener that is notified on each change of the controlvalue...
                XSpinField xSpinField = (XSpinField)oDFControl;
                xSpinField.addSpinListener(_xSpinListener);
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
            return xDFModelPSet;
        }

        public XPropertySet insertPatternField(int _nPosX, int _nPosY, int _nWidth, String sName = "")
        {
            XPropertySet xPFModelPSet = null;
            try
            {
                // create a unique nameid by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "PATTERNFIELD");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                // create a controlmodel at the multiservicefactory of the dialog model...
                Object oPFModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_PATTERNFIELD_MODEL, MXContext);
                XMultiPropertySet xPFModelMPSet = (XMultiPropertySet)oPFModel;

                // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                xPFModelMPSet.setPropertyValues(
                        new String[] { "Height", "Name", "PositionX", "PositionY", "Width" },
                        Any.Get(new Object[] { 12, sName, _nPosX, _nPosY, _nWidth }));

                // The controlmodel is not really available until inserted to the Dialog container
                MXDlgModelNameContainer.insertByName(sName, Any.Get(oPFModel));
                xPFModelPSet = (XPropertySet)oPFModel;

                // The following properties may also be set with XMultiPropertySet but we
                // use the XPropertySet interface merely for reasons of demonstration
                xPFModelPSet.setPropertyValue("LiteralMask", Any.Get("__.05.2007"));
                // Allow only numbers for the first two digits...
                xPFModelPSet.setPropertyValue("EditMask", Any.Get("NNLLLLLLLL"));
                // verify the user input immediately...
                xPFModelPSet.setPropertyValue("StrictFormat", Any.Get(true));
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
            return xPFModelPSet;
        }

        public XPropertySet insertNumericField(int _nPosX, int _nPosY, int _nWidth,
                double _fValueMin, double _fValueMax, double _fValue,
                double _fValueStep, short _nDecimalAccuracy, String sName = "")
        {
            XPropertySet xNFModelPSet = null;
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "NUMMERIC_FIELD");
                sName = createUniqueName(MXDlgModelNameContainer, sName);

                // create a controlmodel at the multiservicefactory of the dialog model...
                Object oNFModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_NUMMERIC_FIELD_MODEL, MXContext);
                XMultiPropertySet xNFModelMPSet = (XMultiPropertySet)oNFModel;
                // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                xNFModelMPSet.setPropertyValues(
                        new String[] { "Height", "Name", "PositionX", "PositionY", "Spin", "StrictFormat", "Width" },
                       Any.Get(new Object[] { 12, sName, _nPosX, _nPosY, true, true, _nWidth }));

                // The controlmodel is not really available until inserted to the Dialog container
                MXDlgModelNameContainer.insertByName(sName, Any.Get(oNFModel));
                xNFModelPSet = (XPropertySet)oNFModel;
                // The following properties may also be set with XMultiPropertySet but we
                // use the XPropertySet interface merely for reasons of demonstration
                xNFModelPSet.setPropertyValue("ValueMin", Any.Get(_fValueMin));
                xNFModelPSet.setPropertyValue("ValueMax", Any.Get(_fValueMax));
                xNFModelPSet.setPropertyValue("Value", Any.Get(_fValue));
                xNFModelPSet.setPropertyValue("ValueStep", Any.Get(_fValueStep));
                xNFModelPSet.setPropertyValue("ShowThousandsSeparator", Any.Get(true));
                xNFModelPSet.setPropertyValue("DecimalAccuracy", Any.Get(_nDecimalAccuracy));
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
            return xNFModelPSet;
        }

        public XCheckBox insertCheckBox(XItemListener _xItemListener, int _nPosX, int _nPosY, int _nWidth, String sName = "")
        {
            XCheckBox xCheckBox = null;
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "CHECKBOX");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                // create a controlmodel at the multiservicefactory of the dialog model...
                Object oCBModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_CHECKBOX_MODEL, MXContext);

                // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                XMultiPropertySet xCBMPSet = (XMultiPropertySet)oCBModel;
                xCBMPSet.setPropertyValues(
                        new String[] { "Height", "Label", "Name", "PositionX", "PositionY", "Width" },
                        Any.Get(new Object[] { 8, "~Include page number", sName, _nPosX, _nPosY, _nWidth }));

                // The following property may also be set with XMultiPropertySet but we
                // use the XPropertySet interface merely for reasons of demonstration
                XPropertySet xCBModelPSet = (XPropertySet)xCBMPSet;
                xCBModelPSet.setPropertyValue("TriState", Any.Get(true));
                xCBModelPSet.setPropertyValue("State", Any.Get((short)1));

                // add the model to the NameContainer of the dialog model
                MXDlgModelNameContainer.insertByName(sName, Any.Get(oCBModel));
                XControl xCBControl = GetControlByName(sName);
                xCheckBox = (XCheckBox)xCBControl;
                // An ActionListener will be notified on the activation of the button...
                xCheckBox.addItemListener(_xItemListener);
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
            return xCheckBox;
        }

        /// <summary>
        /// Inserts the radio button.
        /// </summary>
        /// <param name="_nPosX">The _n pos X.</param>
        /// <param name="_nPosY">The _n pos Y.</param>
        /// <param name="_nWidth">Width of the _n.</param>
        /// <param name="label">The label. Mark the access key with the '~' char in front</param>
        /// <param name="sName">Name of the s.</param>
        public XRadioButton insertRadioButton(XItemListener _itemListener, int _nPosX, int _nPosY, int _nWidth, String label, String sName = "")
        {
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "RADIOBUTTON");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                // create a controlmodel at the multiservicefactory of the dialog model...
                Object oRBModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_RADIOBUTTON_MODEL, MXContext);
                XMultiPropertySet xRBMPSet = (XMultiPropertySet)oRBModel;
                // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                xRBMPSet.setPropertyValues(
                        new String[] { "Height", "Label", "Name", "PositionX", "PositionY", "State", "Width", "Enabled", "Tabstop", "TabIndex" },
                        Any.Get(new Object[] { 8, label, sName, _nPosX, _nPosY, ((short)0), _nWidth, true, true, ((short)(0)) }));
                // add the model to the NameContainer of the dialog model
                MXDlgModelNameContainer.insertByName(sName, Any.Get(oRBModel));
                XControl xRBControl = GetControlByName(sName);
                if (xRBControl != null && xRBControl is XRadioButton && _itemListener != null)
                {
                    ((XRadioButton)xRBControl).addItemListener(_itemListener);
                }

                return xRBControl as XRadioButton;
            }
            catch (System.Exception ex) { }
            return null;
        }
        public void insertRatingGroup(XItemListener _itemListener, int _nPosX, int _nPosY, int _nWidth, int step, String label, String sName = "")
        {
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "RATING_GROUP");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                var gb = insertGroupBox(_nPosX, _nPosY, 30, _nWidth, label, sName);
                SetStepProperty(gb, step);
                var l1= InsertFixedLabel("schlecht", _nPosX + 2, _nPosY + 10, 50, sName+ "label1");
                var l2= InsertFixedLabel("gut", _nPosX + 63, _nPosY + 10, 50, sName+"label2"); 
                SetStepProperty(l1, step);
                SetStepProperty(l2, step);

                for (int i = 0; i < 5; i++)
                {
                    var  a =  insertRadioButton(_itemListener, _nPosX + 5 + (i * 15), _nPosY + 20, 10, String.Empty, sName + "_RATING_" + i);
                    SetStepProperty(a, step);
                    
                }
            }
            catch (System.Exception ex) { }
            //return null;
        }
        
        /************************************************************************/
        /*          TODO: fix the insertion of list elements. Doesn't work !!   */
        /************************************************************************/
        public XListBox insertListBox(int _nPosX, int _nPosY, int _nWidth, int _nStep, String[] _sStringItemList, String sName = "")
        {
            XListBox xListBox = null;
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "LISTBOX");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                // create a controlmodel at the multiservicefactory of the dialog model...
                Object oListBoxModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_LISTBOX_MODEL, MXContext);
                XMultiPropertySet xLBModelMPSet = (XMultiPropertySet)oListBoxModel;
                // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                xLBModelMPSet.setPropertyValues(
                        new String[] { "Dropdown", "Height", "Name", "PositionX", "PositionY", "Step", "StringItemList", "Width" },
                        Any.Get(new Object[] { true, 12, sName, _nPosX, _nPosY, _nStep, _sStringItemList, _nWidth }));
                // The following property may also be set with XMultiPropertySet but we
                // use the XPropertySet interface merely for reasons of demonstration
                XPropertySet xLBModelPSet = (XPropertySet)xLBModelMPSet;
                xLBModelPSet.setPropertyValue("MultiSelection", Any.Get(true));
                short[] nSelItems = new short[] { (short)1, (short)3 };
                //TODO: See if this works
                xLBModelPSet.setPropertyValue("SelectedItems", Any.GetAsOne(nSelItems));

                //uno.Any sI = new uno.Any();


                //xLBModelPSet.setPropertyValue("SelectedItems", new uno.Any(typeof(short[]).GetType(), nSelItems));
                // add the model to the NameContainer of the dialog model
                MXDlgModelNameContainer.insertByName(sName, Any.Get(xLBModelMPSet));
                XControl xControl = GetControlByName(sName);
                // retrieve a ListBox that is more convenient to work with than the Model of the ListBox...
                xListBox = (XListBox)xControl;
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
            return xListBox;
        }

        public XComboBox insertComboBox(int _nPosX, int _nPosY, int _nWidth, String sName = "")
        {
            XComboBox xComboBox = null;
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "COMBOBOX");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                String[] sStringItemList = new String[] { "First Entry", "Second Entry", "Third Entry", "Fourth Entry" };

                // create a controlmodel at the multiservicefactory of the dialog model...
                Object oComboBoxModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_COMBOBOX_MODEL, MXContext);
                XMultiPropertySet xCbBModelMPSet = (XMultiPropertySet)oComboBoxModel;
                // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                xCbBModelMPSet.setPropertyValues(
                        new String[] { "Dropdown", "Height", "Name", "PositionX", "PositionY", "StringItemList", "Width" },
                        Any.Get(new Object[] { true, 12, sName, _nPosX, _nPosY, sStringItemList, _nWidth }));

                // The following property may also be set with XMultiPropertySet but we
                // use the XPropertySet interface merely for reasons of demonstration
                XPropertySet xCbBModelPSet = (XPropertySet)xCbBModelMPSet;
                xCbBModelPSet.setPropertyValue("MaxTextLen", Any.Get((short)10));
                xCbBModelPSet.setPropertyValue("ReadOnly", Any.Get(true));

                // add the model to the NameContainer of the dialog model
                MXDlgModelNameContainer.insertByName(sName, Any.Get(xCbBModelMPSet));
                XControl xControl = GetControlByName(sName);

                // retrieve a ListBox that is more convenient to work with than the Model of the ListBox...
                xComboBox = (XComboBox)xControl;
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
            return xComboBox;
        }

        public XPropertySet insertFormattedField(XSpinListener _xSpinListener, int _nPosX, int _nPosY, int _nWidth, String sName = "")
        {
            XPropertySet xFFModelPSet = null;
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "FORMATTED_FIELD");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                // create a controlmodel at the multiservicefactory of the dialog model...
                Object oFFModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_FORMATTED_FIELD_MODEL, MXContext);
                XMultiPropertySet xFFModelMPSet = (XMultiPropertySet)oFFModel;
                // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                xFFModelMPSet.setPropertyValues(
                        new String[] { "EffectiveValue", "Height", "Name", "PositionX", "PositionY", "StrictFormat", "Spin", "Width" },
                        Any.Get(new Object[] { (double)12348, 12, sName, _nPosX, _nPosY, true, true, _nWidth }));

                xFFModelPSet = (XPropertySet)oFFModel;
                // to define a numberformat you always need a locale...
                unoidl.com.sun.star.lang.Locale aLocale = new unoidl.com.sun.star.lang.Locale();
                aLocale.Country = "US";
                aLocale.Language = "en";
                // this Format is only compliant to the english locale!
                String sFormatString = "NNNNMMMM DD, YYYY";

                // a NumberFormatsSupplier has to be created first "in the open countryside"...
                Object oNumberFormatsSupplier = MXMcf.createInstanceWithContext(OO.Services.UTIL_FORMAT_NUMBER, MXContext);
                XNumberFormatsSupplier xNumberFormatsSupplier = (XNumberFormatsSupplier)oNumberFormatsSupplier;
                XNumberFormats xNumberFormats = xNumberFormatsSupplier.getNumberFormats();
                // is the numberformat already defined?
                int nFormatKey = xNumberFormats.queryKey(sFormatString, aLocale, true);
                if (nFormatKey == -1)
                {
                    // if not then add it to the NumberFormatsSupplier
                    nFormatKey = xNumberFormats.addNew(sFormatString, aLocale);
                }

                // The following property may also be set with XMultiPropertySet but we
                // use the XPropertySet interface merely for reasons of demonstration
                xFFModelPSet.setPropertyValue("FormatsSupplier", Any.Get(xNumberFormatsSupplier));
                xFFModelPSet.setPropertyValue("FormatKey", Any.Get(nFormatKey));
                xFFModelPSet.setPropertyValue("FormatKey", Any.Get(nFormatKey));

                // The controlmodel is not really available until inserted to the Dialog container
                MXDlgModelNameContainer.insertByName(sName, Any.Get(oFFModel));

                // finally we add a Spin-Listener to the control
                XControl xFFControl = GetControlByName(sName);
                // add a SpinListener that is notified on each change of the controlvalue...
                XSpinField xSpinField = (XSpinField)xFFControl;
                xSpinField.addSpinListener(_xSpinListener);

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
            return xFFModelPSet;
        }

        public XTextComponent insertFileControl(XTextListener _xTextListener, int _nPosX, int _nPosY, int _nWidth, String sName = "")
        {
            XTextComponent xTextComponent = null;
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "FILECONTROL");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                // retrieve the configured Work path...
                Object oPathSettings = MXMcf.createInstanceWithContext(OO.Services.UTIL_PATH_SETTINGS, MXContext);
                XPropertySet xPropertySet = (XPropertySet)oPathSettings;
                String sWorkUrl = (String)xPropertySet.getPropertyValue("Work").Value;

                // convert the Url to a system path that is "human readable"...
                Object oFCProvider = MXMcf.createInstanceWithContext(OO.Services.UTIL_FILE_CONTENT_PRVIDER, MXContext);
                XFileIdentifierConverter xFileIdentifierConverter = (XFileIdentifierConverter)oFCProvider;
                String sSystemWorkPath = xFileIdentifierConverter.getSystemPathFromFileURL(sWorkUrl);

                // create a controlmodel at the multiservicefactory of the dialog model...
                Object oFCModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_FILE_MODEL, MXContext);

                // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                XMultiPropertySet xFCModelMPSet = (XMultiPropertySet)oFCModel;
                xFCModelMPSet.setPropertyValues(
                        new String[] { "Height", "Name", "PositionX", "PositionY", "Text", "Width" },
                        Any.Get(new Object[] { 14, sName, _nPosX, _nPosY, sSystemWorkPath, _nWidth }));

                // The controlmodel is not really available until inserted to the Dialog container
                MXDlgModelNameContainer.insertByName(sName, Any.Get(oFCModel));
                XPropertySet xFCModelPSet = (XPropertySet)oFCModel;

                // add a textlistener that is notified on each change of the controlvalue...
                XControl xFCControl = GetControlByName(sName);
                xTextComponent = (XTextComponent)xFCControl;
                XWindow xFCWindow = (XWindow)xFCControl;
                xTextComponent.addTextListener(_xTextListener);
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
            return xTextComponent;
        }

        public XButton insertButton(XActionListener _xActionListener, int _nPosX, int _nPosY, int _nWidth, String _sLabel, short _nPushButtonType, String sName = "")
        {
            XButton xButton = null;
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "BUTTON");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                // create a controlmodel at the multiservicefactory of the dialog model...
                Object oButtonModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_BUTTON_MODEL, MXContext);
                XMultiPropertySet xButtonMPSet = (XMultiPropertySet)oButtonModel;
                // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                xButtonMPSet.setPropertyValues(
                        new String[] { "Height", "Label", "Name", "PositionX", "PositionY", "PushButtonType", "Width" },
                        Any.Get(new Object[] { 14, _sLabel, sName, _nPosX, _nPosY, _nPushButtonType, _nWidth }));

                // add the model to the NameContainer of the dialog model
                MXDlgModelNameContainer.insertByName(sName, Any.Get(oButtonModel));
                XControl xButtonControl = GetControlByName(sName);
                xButton = (XButton)xButtonControl;
                // An ActionListener will be notified on the activation of the button...
                xButton.addActionListener(_xActionListener);
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
            return xButton;
        }

        #region Roadmap

        // Globally available object variables of the roadmapmodel
        XPropertySet m_xRMPSet;
        XSingleServiceFactory m_xSSFRoadmap;
        XIndexContainer m_xRMIndexCont;

        public Object addRoadmap(XItemListener _xItemListener, String sName = "")
        {
            XPropertySet xDialogModelPropertySet = null;
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "ROADMAP");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                xDialogModelPropertySet = (XPropertySet)MXContext;
                // Similar to the office assistants the roadmap is adjusted to the height of the dialog
                // where a certain space is left at the bottom for the buttons...
                int nDialogHeight = (int)(xDialogModelPropertySet.getPropertyValue("Height").Value);

                // instantiate the roadmapmodel...
                Object oRoadmapModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_ROADMAP_MODEL, MXContext);

                // define the properties of the roadmapmodel
                XMultiPropertySet xRMMPSet = (XMultiPropertySet)oRoadmapModel;
                xRMMPSet.setPropertyValues(
                        new String[] { "Complete", "Height", "Name", "PositionX", "PositionY", "Text", "Width" },
                        Any.Get(new Object[] { false, (nDialogHeight - 26), sName, 0, 0, "Steps", 85 }));
                m_xRMPSet = (XPropertySet)oRoadmapModel;

                // add the roadmapmodel to the dialog container..
                MXDlgModelNameContainer.insertByName(sName, Any.Get(oRoadmapModel));

                // the roadmapmodel is a SingleServiceFactory to instantiate the roadmapitems...
                m_xSSFRoadmap = (XSingleServiceFactory)oRoadmapModel;
                m_xRMIndexCont = (XIndexContainer)oRoadmapModel;


                //util.Debug.GetAllProperties(oRoadmapModel);
                //util.Debug.GetAllServicesOfObject(oRoadmapModel);

                // add the itemlistener to the control...
                XControl xRMControl = GetControlByName(sName);
                XItemEventBroadcaster xRMBroadcaster = (XItemEventBroadcaster)xRMControl;
                xRMBroadcaster.addItemListener(_xItemListener);
                return oRoadmapModel;
            }
            catch (System.Exception)
            {
                //jexception.printStackTrace(System.out);
            }
            return null;
        }

        /**
         *To fully understand the example one has to be aware that the passed ???Index??? parameter
         * refers to the position of the roadmap item in the roadmapmodel container
         * whereas the variable ???_ID??? directly references to a certain step of dialog.
         */
        public void insertRoadmapItem(int Index, bool _bEnabled, String _sLabel, int _ID)
        {
            try
            {
                // a roadmap is a SingleServiceFactory that can only create roadmapitems that are the only possible
                // element types of the container
                Object oRoadmapItem = m_xSSFRoadmap.createInstance();
                XPropertySet xRMItemPSet = (XPropertySet)oRoadmapItem;
                xRMItemPSet.setPropertyValue("Label", Any.Get(_sLabel));
                // sometimes steps are supposed to be set disabled depending on the program logic...
                xRMItemPSet.setPropertyValue("Enabled", Any.Get(_bEnabled));
                // in this context the "ID" is meant to refer to a step of the dialog
                xRMItemPSet.setPropertyValue("ID", Any.Get(_ID));
                m_xRMIndexCont.insertByIndex(Index, Any.Get(oRoadmapItem));

                //util.Debug.GetAllProperties(oRoadmapItem);
                //util.Debug.GetAllInterfacesOfObject(oRoadmapItem);

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

        #region Tree

        // Globally available object variables of the treemodel
        XMultiPropertySet m_xTreePSet;
        XMutableTreeDataModel oMutTreeModel;

        public void addTree(XSelectionChangeListener _xSelectionListener = null, String sName = "")
        {

            /*Selection
             *
             *   If you are interested in knowing when the selection changes implement a 
             *   ::com::sun::star::view::XSelectionChangeListener and add the instance 
             *   with the method ::com::sun::star::view::XSelectionSupplier::addSelectionChangeListener(). 
             *   You than will be notified for any selection change.
             *
             *  If you are interested in detecting either double-click events or when a user 
             *  clicks on a node, regardless of whether or not it was selected, you can get 
             *  the ::com::sun::star::awt::XWindow and add yourself as 
             *  a ::com::sun::star::awt::XMouseClickHandler. 
             *  You can use the method XTreeControl::getNodeForLocation() to retrieve the 
             *  node that was under the mouse at the time the event was fired. 
             */

            XPropertySet xDialogModelPropertySet = null;
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "TREE");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                xDialogModelPropertySet = (XPropertySet)MXMsfDialogModel;

                // Similar to the office assistants the tree is adjusted to the height of the dialog
                // where a certain space is left at the bottom for the buttons...
                int nDialogHeight = (int)(xDialogModelPropertySet.getPropertyValue("Height").Value);

                // instantiate the treemodel...
                Object oTreeModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_TREE_MODEL, MXContext);

                // define the properties of the treemodel
                m_xTreePSet = (XMultiPropertySet)oTreeModel;
                m_xTreePSet.setPropertyValues(
                                        new String[] { "Height", "Name", "PositionX", "PositionY", "Width" },
                                        Any.Get(new Object[] { (nDialogHeight - 26), sName, 0, 0, 85 }));
                m_xRMPSet = (XPropertySet)oTreeModel;

                // add the treemodel to the dialog container..
                MXDlgModelNameContainer.insertByName(sName, Any.Get(oTreeModel));

                oMutTreeModel = OO.GetMultiServiceFactory().createInstance(OO.Services.AWT_CONTROL_MUTABLETREE_MODEL) as XMutableTreeDataModel;

                //set the new DdataModel
                XPropertySet xTItemPSet = oTreeModel as XPropertySet;
                if (xTItemPSet != null)
                    xTItemPSet.setPropertyValue("DataModel", Any.Get(oMutTreeModel));

                // add the itemlistener to the control...
                XControl xTMControl = GetControlByName(sName);

                XSelectionSupplier selSupp = xTMControl as XSelectionSupplier;
                if (selSupp != null)
                {
                    selSupp.addSelectionChangeListener(_xSelectionListener);
                }

            }
            catch (System.Exception)
            {
                //jexception.printStackTrace(System.out);
            }
        }

        private XMutableTreeNode AddTreeRootNode(String name) { return AddTreeRootNode(oMutTreeModel, name); }
        /// <summary>
        /// Adds the root node to the tree.
        /// ATTENTION: THere can only be ONE root node!
        /// </summary>
        /// <param name="oMutTreeModel">The XMutableTreeDataModel.</param>
        /// <param name="name">The name of the new root node.</param>
        /// <returns>the created root node</returns>
        private XMutableTreeNode AddTreeRootNode(Object oMutTreeModel, String name)
        {
            //add a root node
            if (oMutTreeModel != null && oMutTreeModel is XMutableTreeDataModel)
            {
                var root = ((XMutableTreeDataModel)oMutTreeModel).createNode(Any.Get(name), false);
                if (root != null && root is XMutableTreeNode)
                {
                    ((XMutableTreeDataModel)oMutTreeModel).setRoot(root);
                    return root;
                }
            }
            return null;
        }

        private XMutableTreeNode createChildTreeNode(String name, XMutableTreeNode parent = null) { return createChildTreeNode(oMutTreeModel, name, parent); }
        private XMutableTreeNode createChildTreeNode(Object oMutTreeModel, String name, XMutableTreeNode parent = null)
        {
            if (oMutTreeModel != null && oMutTreeModel is XMutableTreeDataModel)
            {
                var node = ((XMutableTreeDataModel)oMutTreeModel).createNode(Any.Get(name), false);
                if (parent == null) parent = ((XMutableTreeDataModel)oMutTreeModel).getRoot() as XMutableTreeNode;
                if (parent != null)
                {
                    parent.appendChild(node);
                    return node;
                }
            }
            return null;
        }
        #endregion

        #region ScrollBars
        private const int defaultScrollBarwidth = 10;

        /// <summary>
        /// Inserts a vertical scroll bar.
        /// </summary>
        /// <param name="_nPosX">The X position.</param>
        /// <param name="_nPosY">The Y position.</param>
        /// <param name="_nHeight">Height of the Scrollbar.</param>
        /// <param name="sName">Name of the XControl - can be empty.</param>
        private XScrollBar insertVerticalScrollBar(XAdjustmentListener _xAdjustmentListener, int _nPosX, int _nPosY, int _nHeight, String sName = "")
        {
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "VERTICAL_SCROLLBAR");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                return insertScrollBar(_xAdjustmentListener, _nPosX, _nPosY, _nHeight, defaultScrollBarwidth, unoidl.com.sun.star.awt.ScrollBarOrientation.VERTICAL, sName);

            }
            catch { }
            return null;

        }

        /// <summary>
        /// Inserts s horizontal scroll bar.
        /// </summary>
        /// <param name="_nPosX">The X position.</param>
        /// <param name="_nPosY">The Y position.</param>
        /// <param name="_nWidth">Width of the Scrollbar.</param>
        /// <param name="sName">Name of the XControl - can be empty.</param>
        private XScrollBar insertHorizontalScrollBar(XAdjustmentListener _xAdjustmentListener, int _nPosX, int _nPosY, int _nWidth, String sName = "")
        {
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "HORIZONTAL_SCROLLBAR");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);
                return insertScrollBar(_xAdjustmentListener, _nPosX, _nPosY, defaultScrollBarwidth, _nWidth, unoidl.com.sun.star.awt.ScrollBarOrientation.HORIZONTAL, sName);
            }
            catch { }
            return null;
        }


        /// <summary>
        /// Inserts the scroll bar.
        /// </summary>
        /// <param name="_nPosX">The X position.</param>
        /// <param name="_nPosY">The Y position.</param>
        /// <param name="_nHeight">Height of the Scrollbar.</param>
        /// <param name="_nWidth">Width of the Scrollbar.</param>
        /// <param name="sName">Name of the XControl - can be empty.</param>
        private XScrollBar insertScrollBar(XAdjustmentListener _xAdjustmentListener, int _nPosX, int _nPosY, int _nHeight, int _nWidth, int orientation = unoidl.com.sun.star.awt.ScrollBarOrientation.VERTICAL, String sName = "", bool liveScroll = true)
        {
            try
            {
                // create a unique name by means of an own implementation...
                if (String.IsNullOrWhiteSpace(sName)) sName = createUniqueName(MXDlgModelNameContainer, "SCROLLBAR");
                else sName = createUniqueName(MXDlgModelNameContainer, sName);

                // create a controlmodel at the multiservicefactory of the dialog model...
                Object oSBModel = MXMcf.createInstanceWithContext(OO.Services.AWT_CONTROL_SCROLLBAR_MODEL, MXContext);
                XMultiPropertySet xRBMPSet = (XMultiPropertySet)oSBModel;
                // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                xRBMPSet.setPropertyValues(
                        new String[] { "Height", "LiveScroll", "Name", "Orientation", "PositionX", "PositionY", "Width" },
                        Any.Get(new Object[] { _nHeight, liveScroll, sName, orientation, _nPosX, _nPosY, _nWidth }));
                // add the model to the NameContainer of the dialog model
                MXDlgModelNameContainer.insertByName(sName, Any.Get(oSBModel));
                XControl xSBControl = GetControlByName(sName);

                //System.Diagnostics.Debug.WriteLine("Scrollbar XControl ______");
                //util.Debug.GetAllProperties(xSBControl);
                //util.Debug.GetAllInterfacesOfObject(xSBControl);

                if (xSBControl != null && xSBControl is XScrollBar && _xAdjustmentListener != null)
                {
                    ((XScrollBar)xSBControl).addAdjustmentListener(_xAdjustmentListener);
                }

                return xSBControl as XScrollBar;

            }
            catch { }

            return null;
        }
        #endregion

        #endregion
        private ScrollableContainer createScrollableCommentarResultContainer(int _nPosX, int _nPosY, int _nWidth, int _nHeight, String sName)
        {

            ScrollableContainer SC
                = new ScrollableContainer(
                    MXDlgContainer,
                    MXDlgModelNameContainer,
                    MXContext,
                    MXMcf
                    );
            SC.CreateScrollableContainer(_nPosX, _nPosY, _nWidth, _nHeight, sName);

            //addFailTestPoints(SC);
            //addResults(SC);
            return SC;
        }



        private ScrollableContainer createScrollableContainerTestPoints(int _nPosX, int _nPosY, int _nWidth, int _nHeight, String sName)
        {

            ScrollableContainer SC
                = new ScrollableContainer(
                    MXDlgContainer,
                    MXDlgModelNameContainer,
                    MXContext,
                    MXMcf
                    );
            SC.CreateScrollableContainer(_nPosX, _nPosY, _nWidth, _nHeight, sName);
            
            //addFailTestPoints(SC);
            //addResults(SC);
            return SC;
        }

        private void addCommentarLabel(ScrollableContainer SC,String commentar)
        {
            if (commentResult != null) { 
                commentResult.dispose();
            }
                var commentResultLabel = CreateFixedLabel(commentar,0,0,0,0,20,null,"comment");
                SetProperty(commentResultLabel, "MultiLine", true);
                commentResult = SC.AddElementToTheEndAndAdoptTheSize(commentResultLabel as XControl, "comment", 5, 5,350);
            
        }
        


        #region Scrollable Container Test

        private ScrollableContainer createScrollableContainerResult(int _nPosX, int _nPosY, int _nWidth, int _nHeight, String sName)
        {

            ScrollableContainer SC
                = new ScrollableContainer(
                    MXDlgContainer,
                    MXDlgModelNameContainer,
                    MXContext,
                    MXMcf
                    );
            SC.CreateScrollableContainer(_nPosX, _nPosY, _nWidth, _nHeight, sName);

            addResults(SC);


            return SC;
        }

        private void addResults(ScrollableContainer SC)
        {
            criteriaResultLabelMap = new OrderedDictionary();
            var catHeader = CreateFixedLabel("Kategorien", 0, 0, 0, 0, 5, null, "headerresultsscrollbar");
            SC.AddElementToTheEndAndAdoptTheSize(catHeader as XControl, "categorie_HEADER", h1_leftMargin,5,SC.Width-190,20);
            SetProperty(catHeader, "FontHeight", h1_fontHieght);
            SetProperty(catHeader, "FontWeight", h1_fontWeight);
            SetProperty(catHeader, "MultiLine", true);
            //util.Debug.GetAllProperties(catHeader);
            var critHeader = CreateFixedLabel("Kriterien erfüllt", 0, 0, 0, 0, 5, null, "header_criterion");
            SC.AddElement(critHeader as XControl, "criterion_Header", SC.Width - 175, 10,100, 15);
            SetProperty(critHeader, "FontHeight", h2_fontHieght);
            SetProperty(critHeader, "FontWeight", h1_fontWeight);
                int caCount = 0;
                foreach (Category ca in getCategoriesFromMap())
                {
                    
                    /********************************************
                     * 
                     *          ScrollableContainer Test
                     * 
                     * ******************************************/


                 



                        var catLable = CreateFixedLabel(++caCount + ". " +ca.Name,  0, 0, 0, 0, 5, null, ca.Id + "result");
                        //SetProperty(introduction, "Tag", s.ToString()); //TODO: add the tag for the introduction
                        SetProperty(catLable, "FontHeight", h2_fontHieght);
                        SetProperty(catLable, "FontWeight", h2_fontWeight);
                        //SetProperty(catLable, "HelpText", ca.Id + "result");
                        SetProperty(catLable, "MultiLine", true);


                        //TODO: function to leave the with as ist was set
                        var placesControll = SC.AddElementToTheEndAndAdoptTheSize(catLable as XControl, "criterion_LABEL", h1_leftMargin, 7, SC.Width-190);





                        /********************************************************/
                        /*              PROGRESS BAR TEST                       */
                        /********************************************************/

                        Rectangle pos = new Rectangle(0, 0, 150, 15);
                        if (placesControll != null && placesControll is XWindow2)
                        {
                            Rectangle lablePos = ((XWindow2)placesControll).getPosSize();

                            pos.Y = lablePos.Y;
                            pos.X = SC.Width - pos.Width - 30;
                        }



                        var rating = ProgressBarControll.CreateProgressBar("pb", 0, 0, 0, 0, ca.Criteria.Count, ca.CountPassCategories(), false);

                        var ratingLabel = CreateFixedLabel(ca.CountPassCategories() + "/" + ca.Criteria.Count, 0, 0, 0, 0, 0, null);
                        
                        //util.OoUtils.SetProperty(rating, "ProgressValue", 2);


                        //util.OoUtils.SetProperty(rating, "FillColor", OoUtils.getColor(100, 0, 0));
                        //util.OoUtils.SetProperty(rating, "BackgroundColor", OoUtils.getColor(0, 100, 0));
                        //util.OoUtils.SetProperty(rating, "BorderColor", OoUtils.getColor(0, 0, 100));

                        //util.Debug.GetAllProperties(rating);

                        var placedRating = SC.AddElement(rating as XControl, OoUtils.GetStringProperty(rating, "Name"), pos.X, pos.Y, pos.Width, pos.Height);
                        OoUtils.SetBooleanProperty(placedRating, "EnableVisible", true);

                        var placedRatingLabel = SC.AddElement(ratingLabel as XControl, OoUtils.GetStringProperty(ratingLabel, "Name"), SC.Width - 20 - 3, pos.Y, 20, 15);
                        if (placedRating != null && placedRating is XWindow2)
                        {
                            ((XWindow2)placedRating).setPosSize(pos.X, pos.Y, pos.Width, pos.Height, PosSize.POSSIZE);
                        }
                        bool failLabeExsist = false;
                        bool warningLabelExsist = false;
                        int crCount = 1;
                        Dictionary<int, Criterion> warning = new Dictionary<int,Criterion>();
                        foreach (Criterion cr in ca.Criteria)
                        {

                            if (cr.Res.resultType == ResultType.fail)
                            {
                                if (failLabeExsist == false)
                                {
                                    var failLabel = CreateFixedLabel("Nicht erfüllte Kriterien:", 0, 0, 0, 0, 5, null,"");
                                    SetProperty(failLabel, "FontHeight", 12);
                                    SetProperty(failLabel, "FontWeight", 150);
                                    SetProperty(failLabel, "FontSlant", FontSlant.ITALIC);
                                    SetProperty(failLabel, "FontUnderline", FontUnderline.SINGLE);
                                    SC.AddElementToTheEndAndAdoptTheSize(failLabel as XControl, "", h3_leftMargin+3, 2);
                                    failLabeExsist = true;
                                }
                                var crLabel = CreateFixedLabel("- " + caCount + "." + crCount + ". " + cr.Name + " (Priorität:" + cr.Priority + ")", 0, 0, 0, 0, 5, this, "result" +cr.Id);

                                SetProperty(crLabel, "MultiLine", true);
                                var l = SC.AddElementToTheEndAndAdoptTheSize(crLabel as XControl, "result"+cr.Id, h4_lefMargin+10, 5);
                                criteriaResultLabelMap.Add(cr.Id,l);
                                SetProperty(crLabel, "HelpText", "result" + cr.Id);
                                SetProperty(crLabel, "HelpURL", "10");
                               
                             

                            }
                            if (cr.Res.resultType == ResultType.passwithwarning)
                            {
                                warning.Add(crCount, cr);
                            }
                          


                            /********************************************************/
                            /*              PROGRESS BAR TEST   END                 */
                            /********************************************************/
                            
                            
                            crCount++;
                        }
                        foreach (KeyValuePair<int, Criterion> entry in warning) 
                        {
                            if (warningLabelExsist == false) 
                            {
                                var failLabel2 = CreateFixedLabel("Verbesserungswürdige Kriterien:", 0, 0, 0, 0, 5, null,"");
                                    SetProperty(failLabel2, "FontHeight", 12);
                                    SetProperty(failLabel2, "FontWeight", 150);
                                    SetProperty(failLabel2, "FontSlant", FontSlant.ITALIC);
                                    SetProperty(failLabel2, "FontUnderline", FontUnderline.SINGLE);
                                    SC.AddElementToTheEndAndAdoptTheSize(failLabel2 as XControl, "", h3_leftMargin, 5);
                                    warningLabelExsist = true;
                            }
                            var crLabel = CreateFixedLabel("- "+caCount + "." + entry.Key + ". " + entry.Value.Name  + " (Priorität:" + entry.Value.Priority + ")", 0, 0, 0, 0, 5, this, "result"+ entry.Value);

                            SetProperty(crLabel, "MultiLine", true);
                            var l =  SC.AddElementToTheEndAndAdoptTheSize(crLabel as XControl,"result"+ entry.Value.Id, h4_lefMargin+5, 5);
                            SetProperty(crLabel, "HelpText", "result"+entry.Value.Id);
                            SetProperty(crLabel, "HelpURL",   "10");
                            criteriaResultLabelMap.Add(entry.Value.Id,l);
                        }
                       
                        



                        //if (ca.resultType == ResultType.pass)
                        //{
                        //    var catLable = CreateFixedLabel(++caCount + ". " + ca.Name, 0, 0, 0, 0, 5, null, ca.Id + "result");
                        //    //SetProperty(introduction, "Tag", s.ToString()); //TODO: add the tag for the introduction
                        //    SetProperty(catLable, "FontHeight", h2_fontHieght);
                        //    SetProperty(catLable, "FontWeight", h2_fontWeight);
                        //    //SetProperty(catLable, "HelpText", ca.Id + "result");
                        //    SetProperty(catLable, "MultiLine", true);

                        //    SC.addElementToTheEndAndAdoptTheSize(catLable as XControl, "criterion_LABEL", h1_leftMargin, 7);
                        //}

                        ////util.Debug.GetAllProperties(catLable);
                        ////var progressbar = InsertProgressBar(0,0,100,ca.Criteria.Count,ca.Id+"progressbar");
                        ////SC.addElementAndAdoptTheSize(progressbar as XControl, "progressBar", 100, 100);
                        //if (ca.resultType == ResultType.fail)
                        //{
                        //    var catLable2 = CreateFixedLabel(++caCount + ". " + ca.Name, 0, 0, 0, 0, 5, null, ca.Id + "result");
                        //    //SetProperty(introduction, "Tag", s.ToString()); //TODO: add the tag for the introduction
                        //    SetProperty(catLable2, "FontHeight", h2_fontHieght);
                        //    SetProperty(catLable2, "FontWeight", h2_fontWeight);
                        //    //SetProperty(catLable2, "HelpText", ca.Id + "result");
                        //    SetProperty(catLable2, "MultiLine", true);

                        //    SC.addElementToTheEndAndAdoptTheSize(catLable2 as XControl, "criterion_LABEL", h1_leftMargin, 7);
                        //    bool failLabelexsist = false;
                        //    bool warningLabel = false;
                        //    int crCount = 1;
                        //    foreach (Criterion cr in ca.Criteria)
                        //    {

                        //        if (cr.Res.resultType == ResultType.fail)
                        //        {
                        //            if (failLabelexsist == false)
                        //            {
                        //                var failLabel = CreateFixedLabel("Nicht erfüllte Kriterien", 0, 0, 0, 0, 5, null, cr.Id + "result");
                        //                SetProperty(failLabel, "FontHeight", 10);
                        //                SetProperty(failLabel, "FontWeight", 1000);
                        //                SC.addElementToTheEndAndAdoptTheSize(failLabel as XControl, "", h3_leftMargin, 5);
                        //                failLabelexsist = true;
                        //            }
                        //            var crLabel = CreateFixedLabel(caCount + "." + crCount + ". " + cr.Name + " (Priorität:" + cr.Priority + ")", 0, 0, 0, 0, 5, this, cr.Id+"result");

                        //            SetProperty(crLabel, "MultiLine", true);
                        //            SC.addElementToTheEndAndAdoptTheSize(crLabel as XControl, "", h4_lefMargin, 5);
                        //            SetProperty(crLabel, "HelpText", cr.Id);


                        //        }

                        //        if (cr.Res.resultType == ResultType.passwithwarning)
                        //        {

                        //        }
                        //        crCount++;
                        //    }


                        //}

                    }
                
            }
        

        private ScrollableContainer createScrollableContainer(int _nPosX, int _nPosY, int _nWidth, int _nHeight, String sName = "")
        {

            ScrollableContainer SC
                = new ScrollableContainer(
                    MXDlgContainer,
                    MXDlgModelNameContainer,
                    MXContext, 
                    MXMcf
                    );
            SC.CreateScrollableContainer(_nPosX, _nPosY, _nWidth, _nHeight, sName);
            addNavList(SC);
            return SC;
        }


        const short h1_fontHieght = 11;
        const int h1_leftMargin = 5;
        const short h1_fontWeight = 1010; //weight in %
        const short h2_fontHieght = 9;
        const int h2_leftMargin = 10;
        const short h2_fontWeight = 101; //weight in %
        const int h3_leftMargin = 17;
        const int h4_lefMargin = 25;

        private void addNavList(tud.mci.tangram.models.dialogs.ScrollableContainer sc)
        {

            #region Einführung

            introduction = CreateFixedLabel("Einführung", 0, 0, 0, 0, 0, this, "introduction_LABEL");
            SetProperty(introduction, "HelpURL", "7");
            SetProperty(introduction, "HelpText", "I");
            //SetProperty(introduction, "Tag", s.ToString()); //TODO: add the tag for the introduction
            SetProperty(introduction, "FontHeight", h1_fontHieght);
            SetProperty(introduction, "FontWeight", h1_fontWeight);
            sc.AddElementToTheEndAndAdoptTheSize(introduction as XControl, "introduction_LABEL", h1_leftMargin, 7);
         
            #region Einführung underline
            XControl line1 = CreateHorizontalFixedLine("", 0, 0, 0, 2, 0, "introduction_LABEL_line");
            //TODO: how to set the color
            line1 = sc.AddElementToTheEndAndAdoptTheSize(
                line1,
                "introduction_LABEL_line", 5, 2, sc.Width-5, 2);
            #endregion

            #endregion

            var criterionLable = CreateFixedLabel("Kriterien", 0, 0, 0, 0, 0, this, "criterion_LABEL");
            
            ////SetProperty(criterionLable, "Enable", false);
            ////SetProperty(introduction, "Tag", s.ToString()); //TODO: add the tag for the introduction
            SetProperty(criterionLable, "FontHeight", h1_fontHieght);
            SetProperty(criterionLable, "FontWeight", h1_fontWeight);
            sc.AddElementToTheEndAndAdoptTheSize(criterionLable as XControl, "criterion_LABEL", h1_leftMargin, 7);


            int c = 0;
            foreach (tud.mci.tangram.qsdialog.models.Category ca in categories)
            {

                var catLable = CreateFixedLabel(++c + ". " + ca.Name, 0, 0, 0, 0, 0, this, ca.Id);
                //SetProperty(introduction, "Tag", s.ToString()); //TODO: add the tag for the introduction
                SetProperty(catLable, "FontHeight", h2_fontHieght);
                SetProperty(catLable, "FontWeight", h2_fontWeight);
                SetProperty(catLable, "HelpText", ca.Id);
                SetProperty(catLable, "MultiLine", true);
                //SetProperty(catLable, "HelpURL", 6);
                Object t = null;
                if (catLable is XControl) 
                { 
                t = sc.AddElementToTheEndAndAdoptTheSize(catLable as XControl, ca.Id, h2_leftMargin, 7);
                }
                criteriaLabelMap.Add(ca.Id, t);
                //Debug.GetAllProperties(catLable);

                //insertRoadmapItem(1, false, ca.Name, 1);
                //List<Criterion> criteria = ca.Criteria;
                //categorie adden
                criteriaMap.Add(ca.Id, ca);
                int i = 0;
                foreach (Criterion cr in ca.Criteria)
                {
                    
                    criteriaMap.Add(cr.Id, cr);

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


                    var tLabel = CreateFixedLabel(c+"."+ ++i + ". " + cr.Name, 0, 0, 0, 0, 0, this, cr.Id);
                    SetProperty(tLabel, "HelpURL", s.ToString());
                    SetProperty(tLabel, "HelpText", cr.Id);
                    SetProperty(tLabel, "MultiLine", true);
                    Object l = null;

                    if (tLabel is XControl)
                    {
                        //sc.addElementAndAdoptTheSize(tLabel as XControl, cr.Id, 5, 20 + (i++ * 20));
                        l = sc.AddElementToTheEndAndAdoptTheSize(tLabel as XControl, cr.Id, h3_leftMargin, 5);
                    
                    }

                    criteriaLabelMap.Add(cr.Id, l);

                    //var t = InsertFixedLabel(cr.Name, 10, 20 + (i++ * 10), 100, 10, 0, this, cr.Id);
                    //SetProperty(t, "HelpURL", s.ToString());

                    //SetProperty(t, "HelpURL", Convert.ToString(index++));
                    //util.Debug.GetAllProperties(t);


                }
            }

            #region Auswertung

            #region Auswertung underline
            XControl line2 = CreateHorizontalFixedLine("", 0, 0, 0, 2, 0, "eval_LABEL_line");
            //TODO: how to set the color

            line2 = sc.AddElementToTheEndAndAdoptTheSize(
                line2,
                "eval_LABEL_line", 5, 10, sc.Width - 5, 2);
            #endregion

            var eval = CreateFixedLabel("Auswertung", 0, 0, 0, 0, 0, this, "eval_LABEL");
            //SetProperty(introduction, "Tag", s.ToString()); //TODO: add the tag for the introduction
            SetProperty(eval, "FontHeight", h1_fontHieght);
            SetProperty(eval, "FontWeight", h1_fontWeight);
            sc.AddElementToTheEndAndAdoptTheSize(eval as XControl, "eval_LABEL", h1_leftMargin, 7, sc.Width - h1_leftMargin, 40);


            

            #endregion


        }


        #endregion


       
    }
}
