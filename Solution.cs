using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNIPEA
{
    class Solution
    {
        public double ob1;
        public double ob2;
        public double z1;
        public double z2;
        public double sum;

        /// <summary>构造函数，无参
        /// 
        /// </summary>
        public Solution()
        { }

        /// <summary>构造函数,ob1,ob2
        /// 
        /// </summary>
        /// <param name="ob1"></param>
        /// <param name="ob2"></param>
        public Solution(double ob1, double ob2)
        {
            this.ob1 = ob1;
            this.ob2 = ob2;
            this.z1 = ob1 + 0.000000001 * ob2;
            this.z2 = ob2 + 0.000000001 * ob1;
            this.sum = ob1 + ob2;
        }

        /// <summary>判断a是否支配b
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool dominate(Solution b)
        {
            return (this.ob1 <= b.ob1 && this.ob2 <= b.ob2 && this.sum < b.sum);
        }

        /// <summary>判断a与b是否相等
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool equal(Solution b)
        {
            return (this.ob1 == b.ob1 && this.ob2 == b.ob2);
        }
    }
}
