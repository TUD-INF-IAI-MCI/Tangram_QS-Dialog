using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using unoidl.com.sun.star.lang;
using tud.mci.tangram.util;
using unoidl.com.sun.star.beans;
using tud.mci.tangram.models;
using unoidl.com.sun.star.awt;

namespace tud.mci.tangram.dialog.controlls
{
    static class ProgressBarControll 
    {

        /// <summary>
        /// Creates a progress bar control.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="_nPosX">The X position.</param>
        /// <param name="_nPosY">The Y position.</param>
        /// <param name="_nWidth">Width of the element.</param>
        /// <param name="_nHeight">Height of the element.</param>
        /// <param name="_nProgressMax">The max value.</param>
        /// <param name="_nProgress">The current progress value.</param>
        /// <param name="visible">if set to <c>true</c> the progress bar will be visble while creation.
        /// ATTENTION: from the scratch it is set to <c>true</c>. If you want to hide it while creation 
        /// set this to <c>false</c> and change the 'EnableVisible' property to true after creation.</param>
        /// <param name="xMSF">A XMultiServiceFactory for controll creation.</param>
        /// <returns>a XProgressBar or <c>null</c></returns>
        public static XProgressBar CreateProgressBar(String name, int _nPosX, int _nPosY, int _nWidth, int _nHeight, int _nProgressMax, int _nProgress, bool visible = true, XMultiServiceFactory xMSF = null)
        {
            try
            {
                // check service factory 
                xMSF = xMSF != null ? xMSF : OO.GetMultiServiceFactory();

                //try to generate unique name
                name = AbstractControllBase.GenerateUniqueName(String.IsNullOrWhiteSpace(name) ? "ProgressBar_" : name + "_");

                // create a control model
                Object oPBModel = xMSF.createInstance(OO.Services.AWT_CONTROL_PROGRESS_BAR_MODEL);
                // create a control
                Object xPBControl = xMSF.createInstance(OO.Services.AWT_CONTROL_PROGRESS_BAR);

                if (xPBControl == null && oPBModel != null)
                {
                    string defaultControllName = OoUtils.GetStringProperty(oPBModel, "DefaultControl");
                    if (!String.IsNullOrWhiteSpace(defaultControllName))
                    {
                        xPBControl = xMSF.createInstance(defaultControllName);
                    }
                }

                if (oPBModel != null && xPBControl != null && xPBControl is XControl && oPBModel is XControlModel)
                {
                    ((XControl)xPBControl).setModel(((XControlModel)oPBModel));

                    XMultiPropertySet xPBModelMPSet = oPBModel as XMultiPropertySet;

                    if (xPBModelMPSet != null)
                    {
                        // Set the properties at the model - keep in mind to pass the property names in alphabetical order!
                        xPBModelMPSet.setPropertyValues(
                                new String[] { "EnableVisible", "Height", "Name", "PositionX", "PositionY", "ProgressValue", "ProgressValueMax", "ProgressValueMin", "Width" },
                                Any.Get(new Object[] { visible, _nHeight, name, _nPosX, _nPosY, _nProgress, _nProgressMax, 0, _nWidth }));
                    }
                    return xPBControl as XProgressBar;
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Control creation exception: " + ex);
            }

            return null;

        }

    }
}
