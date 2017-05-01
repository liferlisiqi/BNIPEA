using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiObjevtiveSystem
{
    class Schedule
    {
        #region//成员变量
        public int makespan;
        public int timetotal;
        public int[] schedule;
        #endregion

        /// <summary>构造函数，不对任何成员变量初始化
        /// 
        /// </summary>
        public Schedule()
        { }

        /// <summary>构造函数,对三个成员变量初始化
        /// 
        /// </summary>
        /// <param name="makespan"></param>
        /// <param name="timetotal"></param>
        /// <param name="schedule"></param>
        public Schedule(int makespan, int timetotal, int[] schedule)
        {
            this.makespan = makespan;
            this.timetotal = timetotal;
            this.schedule = schedule;
        }
    }
}
