using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using tud.mci.tangram.EARL;
using tud.mci.tangram.qsdialog.models;
using tud.mci.tangram.models.dialogs;
using unoidl.com.sun.star.awt;
using unoidl.com.sun.star.view;



namespace tud.mci.tangram.qsdialog.models
{
    public class Evaluation
    {
        public Earl earl;
        public ResultType finalRes;
        public String finalerg;
        public int errorCount;
        public int warningCount;
        public int passCount;
        public int criteriaCount;
        public int prio1Pass;
        public int prio2Pass;
        public int prio3Pass;
        public int prio1Count;
        public int prio2Count;
        public int prio3Count;
        public int failprio1Count;
        public int failprio2Count;
        public int failprio3Count;

        public Evaluation() { }
       
        public void evaluate(System.Collections.ICollection caList, Parameters par, Earl ea)
        {
            passCount = 0;
            criteriaCount = 0;
            prio2Pass=0;
            prio3Pass=0;
            prio1Pass=0;
            prio1Count = 0;
            prio2Count = 0;
            prio3Count = 0;
            failprio1Count = 0;
            failprio2Count = 0;
            failprio3Count = 0;
            errorCount = 0;
            warningCount = 0;
            prio1Count = 0;
            this.earl = ea;
            //earl.createEmptyEarlGraph();
            String output = "";
            
            foreach (Category ca in caList)
            {
                //earl.createTestRequirement(ca);
                //hier wird der Kopf des EARL Berichtes geschrieben
                
                //ea.writeEarlRdf();
                
                foreach (Criterion element in ca.Criteria)
                {
                    //set to zero
                    element.EvaluationValue = 0;

                    criteriaCount++;
                    try
                    {
                        //foreach (float e in element.Res.Rating)
                        //{
                        //    System.Console.WriteLine(e);
                        //}
                        float evaluationValue;
                        
                        //hier wird der TestCase für das Element geschrieben das bewertet wurde
                        //earl.createTestCase(element);
                        
                        bool passed = evaluateCriterion(par, element, out evaluationValue);
                        earl.createCriterionResult(element, passed, evaluationValue);
                        
                        if (passed)
                        {

                            passCount++;
                        }
                        
                    }
                    catch (System.Exception ex)
                    {
                        output += "Das Kriterium '" + element.Name + "' wurde wegen eines internen Fahlers übersprungen \r\n";
                    }
                }
                
                createCategoryResult(ca);
            }
            evaluateCategories(caList);
            createFinalErgString();
            
            System.Console.WriteLine(finalerg);
        }

    

        private void createFinalErgString()
        {
            if (failprio1Count > 0)
            {
                finalerg = "Die Grafik wurde nicht ausreichend taktil umgesetzt, da Kriterien mit der Priorität 1 nicht erfüllt wurden.";
               
                return;
            }
            else if (warningCount > 0 && failprio1Count ==0 && failprio2Count == 0 && failprio3Count == 0)
            {
                finalerg = "Die Grafik wurde ausreichen taktil umgesetzt, es gibt aber Verbesserungsvorschläge.";
                
                return;
            }
            else if (errorCount>0)
            {
                finalerg = "Die Grafik wurde ausreichend taktil umgesetzt, es gibt aber Kriterien mit Priorität 2 oder 3, die nicht bestanden wurden.";
                return;
            }
            else
            {
                finalerg = "Die Grafik wurde sehr gut taktil umgesetzt.";
            }
            return;
          
        }

        public void evaluateCategories(System.Collections.ICollection caList)
        {
            //int actErrorCount = 0;
            //int actwarningCount = 0;
            foreach (Category ca in caList) { 
            foreach(Criterion cr in ca.Criteria){
                switch (cr.Res.resultType)
                {
                    case ResultType.fail:
                        //actErrorCount++;
                        errorCount++;
                        if (cr.Priority == 1)
                        {
                            prio1Count++;
                            failprio1Count++;
                        }
                        if (cr.Priority == 2)
                        {
                            prio2Count++;
                            failprio2Count++;
                        }
                        if (cr.Priority == 3)
                        {
                            prio3Count++;
                            failprio3Count++;
                        }
                        break;
                    case ResultType.pass:
                        
                        if (cr.Priority == 1)
                        {
                            prio1Count++;
                            prio1Pass++;
                        }
                        if (cr.Priority == 2)
                        {
                            prio2Count++;
                            prio2Pass++;
                        }
                        if (cr.Priority == 3)
                        {
                            prio3Pass++;
                            prio3Count++;
                        }
                        break;
                    case ResultType.passwithwarning:
                        
                        if (cr.Priority == 1) 
                        {
                            prio1Count++;
                            prio1Pass++;
                        }
                        if (cr.Priority == 2)
                        {
                            prio2Count++;
                            prio2Pass++;
                        }
                        if (cr.Priority == 3)
                        {
                            prio3Count++;
                            prio3Pass++;
                        }
                        warningCount++;
                        //actwarningCount++;
                        
                        break;
                }
            }
            }
        }
            //if (actwarningCount > 0)
            //{
            //    ca.resultType = ResultType.passwithwarning;
            //}
            //if (actErrorCount > 0)
            //{
            //    ca.resultType = ResultType.fail;
            //}
            //else
            //{
            //    ca.resultType = ResultType.pass;
            //}
            public void createCategoryResult(Category ca)
            {
            bool pass = true;
            ResultType resulttypeCategorie = ResultType.pass;
            foreach (Criterion cr in ca.Criteria)
            {
                if (cr.Res.resultType == ResultType.fail)
                {
                    pass = false;
                    resulttypeCategorie = ResultType.fail;
                    
                    break;
                }
                if (cr.Res.resultType == ResultType.passwithwarning)
                {
                    resulttypeCategorie = ResultType.pass;
                        
                }


            }
            ca.resultType = resulttypeCategorie;
            earl.createRequirementResult(ca, pass);
        }
        
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="par"></param>
        /// <param name="element"></param>
        /// <param name="evaluationValue"></param>
        /// <returns></returns>
        private bool evaluateCriterion(Parameters par, Criterion element, out float evaluationValue)
        {
            evaluationValue = 0;
            if (element != null && element.Rec != null && element.Rec.Type != CriterionType.unknown)
            {
                switch (element.Rec.Type)
                {
                    case CriterionType.all:
                        {
                            if (element.Rec.Items != null && element.Rec.Items.Count > 0)
                            {
                                int passed = 0;

                                for (int i = 0; i < element.Rec.Items.Count; i++)
                                {
                                    //hier wird test Result für einzelnes Item erstellt
                                    earl.createAllItemResult(element, i, (int)(element.Res.GetItemRating(i)));
                                    if (element.Res.GetItemRating(i) == 1)
                                    {
                                        element.EvaluationValue++;
                                        passed++;

                                    }
                                }
                                evaluationValue = (float)passed / (float)element.Rec.Items.Count;
                                if (evaluationValue != 1)
                                {
                                    element.Res.resultType = ResultType.fail;
                                    return false;
                                }
                                else
                                {
                                    element.Res.resultType = ResultType.pass;
                                    element.Res.Passed = true;
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    case CriterionType.one:
                        {
                            bool ergBool = false;
                            element.Res.resultType = ResultType.fail;
                            for (int i = 0; i < element.getCountOfItems(); i++)
                            {
                                earl.createOneItemResult(element, i, (int)element.Res.Rating[i]);
                                if (element.Res.GetItemRating(i) == 1)
                                {
                                    element.EvaluationValue++;
                                    evaluationValue = 1;
                                    ergBool = true;
                                    element.Res.Passed = true;
                                    element.Res.resultType = ResultType.pass;
                                }

                            }
                            return ergBool;
                            //if (element.Res.GetItemRating(0) == 0)
                            //{
                            //    return false;
                            //}
                            //evaluationValue = 1;
                            //return true;
                        }
                    case CriterionType.count:
                        {
                            int passed = 0;

                            for (int i = 0; i < element.Rec.Items.Count; i++)
                            {
                                earl.createCountItemResult(element, i, (int)element.Res.GetItemRating(i));
                                if (element.Res.GetItemRating(i) == 1)
                                {
                                    element.EvaluationValue++;
                                    passed++;
                                }
                            }
                            evaluationValue = (float)passed / (float)element.Rec.Items.Count;
                            if (evaluationValue < par.PassCount)
                            {
                                element.Res.resultType = ResultType.fail;
                                return false;
                            }
                            else if (evaluationValue == 1)
                            {
                                element.Res.resultType = ResultType.pass;
                                element.Res.Passed = true;
                                return true;
                            }
                            else
                            {
                                element.Res.resultType = ResultType.passwithwarning;
                                element.Res.Passed = true;
                                return true;
                            }
                        }
                    case CriterionType.rating:
                        {
                            int index = 0;
                            float sum = 0;
                            for (int i = 0; i < 5; i++)
                            {
                                if (element.Res.GetItemRating(i) == 1)
                                {
                                    sum = i;
                                }
                            }
                            if (element.Rec.Items.Count > 0)
                            {
                                evaluationValue = (float)sum / ((float)element.Res.Rating.Count - 1);
                                if (evaluationValue < par.PassRating)
                                {
                                    element.Res.resultType = ResultType.fail;
                                    earl.createRatingResultFail(element, evaluationValue, index);
                                    element.Res.ratingerg = evaluationValue;
                                    return false;
                                }
                                else if (evaluationValue == 1)
                                {
                                    element.Res.resultType = ResultType.pass;
                                    earl.createRatingResultPass(element, evaluationValue, index);
                                    element.Res.Passed = true;
                                    element.Res.ratingerg = evaluationValue;
                                    return true;
                                }
                                else
                                {
                                    element.Res.resultType = ResultType.passwithwarning;
                                    earl.createRatingResultPass(element, evaluationValue, index);
                                    element.Res.Passed = true;
                                    element.Res.ratingerg = evaluationValue;
                                    return true;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    default:
                        break;
                }
            }
            return false;
        }
    }

        public class Parameters
        {
            public double PassCount = 0.5;
            public double PassRating = 0.5;
            public int FailPrio1 = 1;
            public int FailPrio2 = 2;
            public int FailPrio3 = 3;
        }
    }


