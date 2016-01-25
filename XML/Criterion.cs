using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tud.mci.tangram.qsdialog.models
{
    public class Criterion : IQsEntry
    {
        private String comment = String.Empty;

        public String Comment
        {
            get { return comment; }
            set { comment = value; }
        }


        private String relation;

        public String Relation
        {
            get { return relation; }
            set { relation = value; }
        }
        private String mode;

        private int evaluationValue;

        public int EvaluationValue
        {
            get { return evaluationValue; }
            set { evaluationValue = value; }
        }
       

        private Category ca;
        public Category CA
        {
            get { return ca; }
            set { ca = value; }
        }

        public String Mode
        {
            get { return mode; }
            set { mode = value; }
        }
        private int priority;

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }
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
        private Recommendation rec;

        internal Recommendation Rec
        {
            get { return rec; }
            set { rec = value; }
        }

        //private Result res;
        public  Result Res;

        public Criterion(Recommendation rec, String id1, String desc1, String help1):this(rec)
        {
            this.id = id1;
            this.desc = desc1;
            this.help = help1;
            

        }
        /// <summary>
        /// get the count of items in the Recommendation
        /// </summary>
        /// <returns></returns>
        public int getCountOfItems()
        {    
             
             int i = this.Rec.Items.Count;
             return i;
        }
        //wrong constructor, not used only with empty rec by the Parser
        public Criterion(Recommendation rec)
        {
            Rec = rec;
            if (rec != null && rec.Items != null)
            {
                Res = new Result(rec.Type, rec.Items.Count);
            }
            else throw new ArgumentException("rec ist null / rec.Items ist null", "rec");
        }
        /// <summary>
        /// creates a List for the Results of a criterion
        /// </summary>
        public void setResult()
        {
            if (rec != null && rec.Items != null)
            { 
            this.Res = new Result(this.rec.Type, rec.Items.Count);
            }
            else throw new ArgumentException("rec ist null / rec.Items ist null", "rec");

        }
        
        /// <summary>
        /// Only use after Evaluation was made
        /// </summary>
        /// <returns></returns>
        //public List<Item> GetFailedITemsWithVarText()
        //{
        //    List<Item> failedItems = new List<Item>();

        //    switch(this.Res.Type)
        //        {
        //            case CriterionType.rating:
        //                if(this.Res.resultType== ResultType.fail){
        //                   );
        //                }
        //                break;
        //        }
            
        //    return null;
        //}
        //}
    
    }
    public struct Result
    {
        public readonly CriterionType Type;
        public List<float> Rating;
        private bool passed;
        public ResultType resultType;
        public float ratingerg;

        public bool Passed
        {
            get { return passed; }
            set { passed = value; }
        }

       
        public Result(CriterionType type)
        {
            this.ratingerg = 0;
            Type = type;
            Rating = new List<float>();
            passed = false;
            resultType = ResultType.fail;
            
        }

        public Result(CriterionType type, int count)
            : this(type)
        {
            resultType = ResultType.fail;
            passed = false;
            ratingerg = 0;
            if (type == CriterionType.rating)
            {
                for (int i = 0; i < 5; i++)
                {
                    Rating.Add(0);
                }
                //System.Console.WriteLine("rating");
            }
            else { 
            for (int i = 0; i < count; i++)
            {
                Rating.Add(0);
            }
            }
        }

        public Result(CriterionType type, List<float> rating)
            : this(type)
        {
            ratingerg=0;
            resultType = ResultType.fail;
            passed = false;
            Rating = rating;
           
        }
        
        public void SetItemRating(int i, float value)
        {
            if (value < 0 || value > 1) { throw new ArgumentException("value nicht zwischen 0 und 1", "value"); }
            if (i < 0) { throw new ArgumentException("i kleiner als 0", "i"); }
            if (i >= Rating.Count) { throw new ArgumentException("Index zu groß", "i"); }

            Rating[i] = value;
        }

        public float GetItemRating(int i)
        {
            if (i < 0) { throw new ArgumentException("i kleiner als 0", "i"); }
            if (i >= Rating.Count) { throw new ArgumentException("Index zu groß", "i"); }

            return Rating[i];
        }
        

    }
    public enum CriterionType
    {
        unknown, all, one, count, rating
    }
    public enum ResultType
    {
        fail, pass, passwithwarning
    }

}

