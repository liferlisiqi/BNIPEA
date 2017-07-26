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
        /// <summary>找到ob1最小的Pareto最优解
        ///
        /// </summary>
        /// <param name="solutions"></param>
        /// <returns></returns>
        public static Solution min1Pareto(ArrayList solutions)
        {
            Solution pareto = new Solution(1000, 1000);
            foreach (Solution i in solutions)
                pareto = i.z1 < pareto.z1 ? i : pareto;
            return pareto;
        }

        /// <summary>找到ob1最小的Pareto最优解，该重载只用于原始Epslon约束法
        /// 
        /// </summary>
        /// <param name="solutions"></param>
        /// <returns></returns>
        public static Solution min1Pareto(ArrayList solutions, Solution min1Pareto)
        {
            Solution Pareto = new Solution(1000, 1000);
            foreach (Solution i in solutions)
                if (i.ob2 < min1Pareto.ob2 && i.z1 < Pareto.z1)
                    Pareto = i;
            return Pareto;
        }


        /// <summary>找到ob2最小的Pareto最优解
        /// 
        /// </summary>
        /// <param name="solutions"></param>
        /// <returns></returns>
        public static Solution min2Pareto(ArrayList solutions)
        {
            Solution pareto = new Solution(1000, 1000);
            foreach (Solution i in solutions)
                pareto = i.z2 < pareto.z2 ? i : pareto;
            return pareto;
        }

        /// <summary>没有理想点，找到理想Pareto
        /// 
        /// </summary>
        /// <param name="solutions"></param>
        /// <returns></returns>
        public static Solution idealPareto(ArrayList solutions)
        {
            Solution idealPoint = new Solution(1000, 1000);
            Solution idealPareto = new Solution();
            double nearDis = double.MaxValue;
            foreach (Solution i in solutions)
            {
                if (idealPoint.ob1 > i.ob1) idealPoint.ob1 = i.ob1;
                if (idealPoint.ob2 > i.ob2) idealPoint.ob2 = i.ob2;
            }
            foreach (Solution i in solutions)
            {
                double dis = Math.Pow(idealPoint.ob1 - i.ob1, 2)
                    + Math.Pow(idealPoint.ob2 - i.ob2, 2);
                if (dis < nearDis)
                {
                    idealPareto = i;
                    nearDis = dis;
                }
            }
            return idealPareto;
        }

        /// <summary>已有理想点,找到理想Pareto
        /// 
        /// </summary>
        /// <param name="solutions"></param>
        /// <param name="idealPoint"></param>
        /// <returns></returns>
        public static Solution idealPareto(ArrayList solutions, Solution idealPoint)
        {
            Solution idealPareto = new Solution();
            double nearDis = double.MaxValue;
            foreach (Solution i in solutions)
            {
                double dis = Math.Pow(idealPoint.ob1 - i.ob1, 2)
                    + Math.Pow(idealPoint.ob2 - i.ob2, 2);
                if (dis < nearDis)
                {
                    idealPareto = i;
                    nearDis = dis;
                }
            }
            return idealPareto;
        }

        /// <summary>寻找不被给定Pareto支配的解
        /// 
        /// </summary>
        /// <param name="solutions"></param>
        /// <param name="Pareto"></param>
        /// <returns></returns>
        public static ArrayList ndSolutions(ArrayList solutions, Solution Pareto)
        {
            ArrayList ndSolutions = new ArrayList();
            foreach (Solution i in solutions)
            {
                if (!Pareto.dominate(i) && !Pareto.equal(i))
                    ndSolutions.Add(i);
            }
            return ndSolutions;
        }
    }
}
