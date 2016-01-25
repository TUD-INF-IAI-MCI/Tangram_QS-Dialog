using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tud.mci.tangram.qsdialog.models
{
    class Variable
    {
        private MediaType mediaType;

        public MediaType MediaType
        {
            get { return mediaType; }
            set { mediaType = value; }
        }
        private String name;
        private String min;

        public String Min
        {
            get { return min; }
            set { min = value; }
        }
        private String max;

        public String Max
        {
            get { return max; }
            set { max = value; }
        }
       

        public String Name
        {
            get { return name; }
            set { name = value; }

        }
        private String value;

        public String Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        public Variable(){

    }

        
    }
    public enum MediaType
    {
        All, Tiger, Schwellpapier, Stiftplatte
    }
}
