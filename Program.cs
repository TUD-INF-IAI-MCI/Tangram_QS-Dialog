using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tud.mci.tangram.qsdialog.Dialogs;
using tud.mci.tangram.util;
using tud.mci.tangram.Examples.dialogs;
using System.Windows.Forms;

namespace tud.mci.tangram.qsdialog
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {

                

                /******************* Dialog sample ************************/
                //var context = OO.GetContext();
                //var factory = OO.GetMultiComponentFactory();
                
                //var dialog = new UnoDialogSample(context, factory);
                //dialog.ShowDialog();


                //NEW DIAOLOG START
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(
                    new tud.mci.tangram.qsdialog.QsDialog(tud.mci.tangram.qsdialog.models.Parser.GetBaseDir() + @"\Test\XMLFile1.xml")
                    );


            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Exception occurred: \n" + ex);
                Console.ReadLine();
            }
        }
    }
}
