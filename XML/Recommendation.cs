using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tud.mci.tangram.qsdialog.models;

namespace tud.mci.tangram.qsdialog.models
{
    public class Recommendation
    {

        private CriterionType type;

        public CriterionType Type
        {
            get { return type; }
            set { type = value; }
        }
        List<Item> items;

        internal List<Item> Items
        {
            get { return items; }
            set { items = value; }
        }
        public Recommendation(CriterionType typ)
        {
            this.type = typ;
            this.items = new List<Item>();
        }

        public Recommendation()
        {
            // TODO: Complete member initialization
            this.items = new List<Item>();
            
        }
    }
}
