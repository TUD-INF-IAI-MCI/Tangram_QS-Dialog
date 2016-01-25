using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using unoidl.com.sun.star.lang;
using unoidl.com.sun.star.view;
using tud.mci.tangram.models;
using unoidl.com.sun.star.beans;
using unoidl.com.sun.star.awt;
using unoidl.com.sun.star.frame;
using unoidl.com.sun.star.text;

namespace tud.mci.tangram.qsdialog.Dialogs
{

    public partial class UnoDialogSample
    {
        #region Execution
        /// <summary>
        /// Executes the dialog with embedded example snippets.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="com.sun.star.script.BasicErrorException">com.sun.star.script.BasicErrorException</exception>
        public short ExecuteDialogWithembeddedExampleSnippets()
        {
            if (MXWindowPeer == null)
            {
                createWindowPeer();
            }
            MXDialog = MXDialogControl as XDialog;
            MXComponent = MXDialogControl as XComponent;

            initalize();

            return MXDialog.execute();
        }

        public short ExecuteDialog()
        {
            if (MXWindowPeer == null)
            {
                createWindowPeer();
            }
            MXDialog = MXDialogControl as XDialog;
            MXComponent = MXDialogControl as XComponent;
            return MXDialog.execute();
        }

        public void EndDialogExecute()
        {
            MXDialog.endExecute();
        }

        #endregion

        #region utils

        public void calculateDialogPosition(XWindow _xWindow)
        {
            Rectangle aFramePosSize = ((unoidl.com.sun.star.frame.XModel)MXModel).getCurrentController().getFrame().getComponentWindow().getPosSize();
            Rectangle CurPosSize = _xWindow.getPosSize();
            int WindowHeight = aFramePosSize.Height;
            int WindowWidth = aFramePosSize.Width;
            int DialogWidth = CurPosSize.Width;
            int DialogHeight = CurPosSize.Height;
            int iXPos = ((WindowWidth / 2) - (DialogWidth / 2));
            int iYPos = ((WindowHeight / 2) - (DialogHeight / 2));
            _xWindow.setPosSize(iXPos, iYPos, DialogWidth, DialogHeight, PosSize.POS);
        }


        public XWindowPeer createNonModalWindowPeer()
        {
            try
            {
                XToolkit xToolkit = (XToolkit)MXMcf
                        .createInstanceWithContext("com.sun.star.awt.Toolkit",
                             MXContext);

            WindowDescriptor aDescriptor = new WindowDescriptor();
            aDescriptor.Type = WindowClass.TOP;
            aDescriptor.WindowServiceName = "window";
            aDescriptor.ParentIndex = -1;
            //aDescriptor.Parent = xToolkit.getDesktopWindow();
            aDescriptor.Parent = null;
            aDescriptor.Bounds = new Rectangle(100, 200, 300, 400);

            aDescriptor.WindowAttributes = WindowAttribute.BORDER
                  | WindowAttribute.MOVEABLE 
                  | WindowAttribute.SIZEABLE
                  | WindowAttribute.CLOSEABLE;

            XWindowPeer xPeer = xToolkit.createWindow(aDescriptor);

            XWindow xWindow = (XWindow)xPeer;
            xWindow.setVisible(false);
            MXDialogControl.createPeer(xToolkit, xPeer);

            MXWindowPeer = MXDialogControl.getPeer();

            return MXWindowPeer;

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

        /**
         * create a peer for this
         * dialog, using the given
         * peer as a parent.
         * @param parentPeer
         * @return
         * @throws java.lang.Exception
         */
        public XWindowPeer createWindowPeer(XWindowPeer _xWindowParentPeer)
        {
            try
            {
                if (_xWindowParentPeer == null)
                {
                    XWindow xWindow = (XWindow)MXDlgContainer;
                    xWindow.setVisible(false);
                    Object tk = MXMcf.createInstanceWithContext("com.sun.star.awt.Toolkit", MXContext);
                    XToolkit xToolkit = (XToolkit)tk;
                    //MXReschedule = (XReschedule)xToolkit;
                    MXDialogControl.createPeer(xToolkit, _xWindowParentPeer);
                    MXWindowPeer = MXDialogControl.getPeer();
                    return MXWindowPeer;
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
            return null;
        }

        /**
         * Creates a peer for this
         * dialog, using the active OO frame
         * as the parent window.
         * @return
         * @throws java.lang.Exception
         */
        public XWindowPeer createWindowPeer()
        {
            return createNonModalWindowPeer();
            return createWindowPeer(null);
        }

        public XFrame getCurrentFrame()
        {
            XFrame xRetFrame = null;
            try
            {
                Object oDesktop = MXMcf.createInstanceWithContext("com.sun.star.frame.Desktop", MXContext);
                XDesktop xDesktop = (XDesktop)oDesktop;
                xRetFrame = xDesktop.getCurrentFrame();
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
            return xRetFrame;
        }

        public void convertUnits()
        {
            //    iXPixelFactor = (int) (100000/xDevice.getInfo().PixelPerMeterX);
        }

        #endregion
     
        #region external EventListener creation

        //TODO:
        public XSelectionChangeListener GetTreeItemSelectioChangeListener()
        {
            //return new RoadmapItemStateChangeListener(m_xMSFDialogModel);
            return new TreeSelectionChangeListener();
            //return null;
        }

        #endregion

        #region external Event Listener classes - only for debugging

        class TreeSelectionChangeListener : XSelectionChangeListener
        {

            public void selectionChanged(EventObject aEvent)
            {
                System.Diagnostics.Debug.WriteLine("\t\t\tSELCTION CHANGED IN TREE");
            }

            public void disposing(EventObject Source)
            {

            }
        }

        #endregion
    }
}
