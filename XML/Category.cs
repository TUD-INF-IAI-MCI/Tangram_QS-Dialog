using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tud.mci.tangram.qsdialog.models;

namespace tud.mci.tangram.qsdialog.models
{
    public interface IQsEntry
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        String Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the detailed description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        String Desc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the help text with detailed hints or further instructions. Can be HTML.
        /// </summary>
        /// <value>
        /// The help text in HTML.
        /// </value>
        String Help
        {
            get;
            set;
        }
    }

    public class Category : IQsEntry
    {
        public ResultType resultType;
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        private String id;

        public String Id
        {
            get { return id; }
            set { id = value; }
        }
        private String desc;

        public String Desc
        {
            get { return desc; }
            set { desc = value; }
        }
        private String help;

        public String Help
        {
            get { return help; }
            set { help = value; }
        }
        private List<Criterion> criteria;

        public List<Criterion> Criteria
        {
            get { return criteria; }
            set { criteria = value; }
        }

        public Category(String id1, String desc1, String help1)
        {
            resultType = ResultType.fail;
            this.id = id1;
            this.desc = desc1;
            this.help = help1;
            this.criteria = new List<Criterion>();


        }

        public Category()
        {
            resultType = ResultType.fail;
            // TODO: Complete member initialization
        }
        public int CountPassCategories()
        {int i = 0;
            foreach(Criterion cr in criteria)
            {
                if(cr.Res.resultType == ResultType.pass || cr.Res.resultType == ResultType.passwithwarning)
                {
                    i++;
                }
            }
            return i;
        }
        
    }
}

