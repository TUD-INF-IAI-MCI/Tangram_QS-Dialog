using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tud.mci.tangram.qsdialog.models
{
    class Item
    {
        private String role;

        public String Role
        {
            get { return role; }
            set { role = value; }
        }


        private String desc;

        public String Desc
        {
            get { return desc; }
            set { desc = value; }
        }
        private List<Variable> var;

        private String varText;

        public String VarText
        {
            get { return varText; }
            set { varText = value; }
        }

        internal List<Variable> Var
        {
            get { return var; }
            set { var = value; }
        }

        public Item()
        {
            this.varText = "";
            this.var= new List<Variable>();

        }
    }
}
