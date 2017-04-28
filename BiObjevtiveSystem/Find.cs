using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BiObjevtiveSystem
{
    class Find
    {
        /// <summary>找到ob1最大的解，同时也是一个Pareto最优解
        ///
        /// </summary>
        /// <param name="solutionSet"></param>
        /// <returns></returns>
        public static Solution ob1MaxSolution(ArrayList solutionSet)
        {
            Solution Pareto = new Solution();
            ArrayList ob1MaxSolutionSet = new ArrayList();

            foreach (Solution i in solutionSet)
            {
                if (Pareto.ob1 < i.ob1)
                {
                    Pareto = i;
                    ob1MaxSolutionSet.Clear();
                    ob1MaxSolutionSet.Add(Pareto);
                }
                if (Pareto.ob1 == i.ob1)
                {
                    ob1MaxSolutionSet.Add(i);
                }
            }
            foreach (Solution i in ob1MaxSolutionSet)
            {
                if (Pareto.ob2 <= i.ob2)
                {
                    Pareto = i;
                }
            }

            return Pareto;
        }

        /// <summary>找到ob1最大的解，同时也是一个Pareto最优解，但该方法只用于原始Epslon约束法
        /// 
        /// </summary>
        /// <param name="solutionSet"></param>
        /// <returns></returns>
        public static Solution ob1MaxSolution(ArrayList solutionSet, Solution aheadob1MaxPareto)
        {
            Solution Pareto = new Solution();
            ArrayList ob1MaxSolutionSet = new ArrayList();

            foreach (Solution i in solutionSet)
            {
                if(aheadob1MaxPareto.ob2 < i.ob2)
                {
                    if (Pareto.ob1 < i.ob1)
                    {
                        Pareto = i;
                        ob1MaxSolutionSet.Clear();
                        ob1MaxSolutionSet.Add(Pareto);
                    }
                    if (Pareto.ob1 == i.ob1)
                    {
                        ob1MaxSolutionSet.Add(i);
                    }
                }
                
            }
            foreach (Solution i in ob1MaxSolutionSet)
            {
                if (Pareto.ob2 <= i.ob2)
                {
                    Pareto = i;
                }
            }

            return Pareto;
        }


        /// <summary>找到ob2最大的解，同时也是一个Pareto最优解
        /// 
        /// </summary>
        /// <param name="solutionSet"></param>
        /// <returns></returns>
        public static Solution ob2MaxSolution(ArrayList solutionSet)
        {
            Solution Pareto = new Solution();
            ArrayList ob2MaxSolutionSet = new ArrayList();

            foreach (Solution i in solutionSet)
            {
                if (Pareto.ob2 < i.ob2)
                {
                    Pareto = i;
                    ob2MaxSolutionSet.Clear();
                    ob2MaxSolutionSet.Add(Pareto);
                }
                if (Pareto.ob2 == i.ob2)
                {
                    ob2MaxSolutionSet.Add(i);
                }
            }
            foreach (Solution i in ob2MaxSolutionSet)
            {
                if (Pareto.ob1 <= i.ob1)
                {
                    Pareto = i;
                }
            }

            return Pareto;
        }

        /// <summary>找到距理想点最近的解，同时也是一个Pareto最优解
        /// 
        /// </summary>
        /// <param name="solutionSet"></param>
        /// <param name="ob1MaxValue"></param>
        /// <param name="ob2MaxValue"></param>
        /// <returns></returns>
        public static Solution nearestPointToIdealPoint(ArrayList solutionSet)
        {
            Solution nearestPoint = new Solution();
            double nearestDistance = new double();
            double ob1MaxValue = 0;
            double ob2MaxValue = 0;

            foreach (Solution i in solutionSet)
            {
                if (ob1MaxValue < i.ob1) ob1MaxValue = i.ob1;
                if (ob2MaxValue < i.ob2) ob2MaxValue = i.ob2;
            }

            nearestDistance = double.MaxValue;

            foreach (Solution i in solutionSet)
            {
                double distance = (ob1MaxValue - i.ob1) * (ob1MaxValue - i.ob1) + (ob2MaxValue - i.ob2) * (ob2MaxValue - i.ob2);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPoint = i;
                }
            }
            return nearestPoint;
        }


    }
}
