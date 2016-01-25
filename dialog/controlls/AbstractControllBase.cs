using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tud.mci.tangram.dialog.controlls
{
    static class AbstractControllBase
    {

        static readonly Random rand = new Random(DateTime.Now.Millisecond);

        public static String GenerateUniqueName(String _base = "")
        {
            return _base + rand.Next(99999999).ToString();
        }

    }
}
