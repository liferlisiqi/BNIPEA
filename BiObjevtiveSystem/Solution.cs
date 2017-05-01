using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiObjevtiveSystem
{
    class Solution
    {
        public double ob1;
        public double ob2;
        public double ob3;
        public string solution;

        /// <summary>构造函数，无参
        /// 
        /// </summary>
        public Solution()
        { }

        /// <summary>构造函数,ob1,ob2
        /// 
        /// </summary>
        /// <param name="ob1Value"></param>
        /// <param name="ob2Value"></param>
        public Solution(double ob1Value, double ob2Value)
        {
            this.ob1 = ob1Value;
            this.ob2 = ob2Value;
        }
    }
}
