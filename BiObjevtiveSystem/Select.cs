using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BiObjevtiveSystem
{
    class Select
    {
        /// <summary>挑选不被ob1最大的Pareto支配的解
        /// 
        /// </summary>
        /// <param name="solutionSet"></param>
        /// <param name="ob1MaxPareto"></param>
        /// <returns></returns>
        public static ArrayList nondominatedByOb1MaxPareto(ArrayList solutionSet, Solution ob1MaxPareto)
        {
            ArrayList nondominatedSolutionSet = new ArrayList();

            foreach (Solution i in solutionSet)
            {
                if (i.ob2 > ob1MaxPareto.ob2)
                {
                    nondominatedSolutionSet.Add(i);
                }
            }

            return nondominatedSolutionSet;
        }

        /// <summary>挑选不被ob2最大的Pareto支配的解
        /// 
        /// </summary>
        /// <param name="solutionSet"></param>
        /// <param name="ob2MaxPareto"></param>
        /// <returns></returns>
        public static ArrayList nondominatedByOb2MaxPareto(ArrayList solutionSet, Solution ob2MaxPareto)
        {
            ArrayList nondominatedSolutionSet = new ArrayList();

            foreach (Solution i in solutionSet)
            {
                if (i.ob1 > ob2MaxPareto.ob1)
                {
                    nondominatedSolutionSet.Add(i);
                }
            }

            return nondominatedSolutionSet;
        }

        /// <summary>挑选不被距理想点最近的Pareto支配的解
        /// 
        /// </summary>
        /// <param name="solutionSet"></param>
        /// <param name="neatestPareto"></param>
        /// <returns></returns>
        public static ArrayList nondominatedByNearestPareto(ArrayList solutionSet, Solution neatestPareto)
        {
            ArrayList nondominatedSolutionSet = new ArrayList();
            foreach (Solution i in solutionSet)
            {
                if (i.ob1 > neatestPareto.ob1 || i.ob2 > neatestPareto.ob2)
                {
                    nondominatedSolutionSet.Add(i);
                }
            }
            return nondominatedSolutionSet;
        }

    }
}
