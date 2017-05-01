using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace BiObjevtiveSystem
{
    class BackTrackUnrelated
    {
        #region//参数定义
        private int numWork;
        private int numMachine;
        int[,] time;//每个机器完成每项工作的时间参数
        private int[] timeMachine;
        private int timeTotal;//临时总时间，用于回溯的比较和减枝
        private int[] schedule;
        #endregion

        /// <summary>构造函数，给BTU初始化参数
        /// 
        /// </summary>
        /// <param name="model"></param>
        public BackTrackUnrelated(Model model)
        {
            numWork = model.n;
            numMachine = model.m;
            time = model.c;
            timeMachine = new int[numMachine];
        }

        /// <summary>打印一个解
        /// 
        /// </summary>
        /// <param name="makespan"></param>
        private void printSchedule(int makespan, int timeTotal)
        {
            Console.WriteLine();
            Console.WriteLine("makespan:  " + makespan);
            Console.WriteLine("timetotal:  " + timeTotal);
            Console.Write("each job costs: ");
            for (int i = 0; i < numWork; i++)
            {
                Console.Write(time[schedule[i] - 1, i] + " ");
            }
            Console.WriteLine();
            Console.Write("schedule: ");
            for (int i = 0; i < numWork; i++)
                Console.Write(schedule[i] + " ");
            Console.WriteLine();
        }

        /// <summary>计算makespan
        /// 
        /// </summary>
        /// <returns></returns>
        private int getMakespan()
        {
            int makespan = 0;
            foreach (int i in timeMachine)
                makespan = Math.Max(makespan, i);
            return makespan;
        }

        /// <summary>计算总时间
        /// 
        /// </summary>
        /// <returns></returns>
        private int getTimeTotal()
        {
            int timeTotal = 0;
            foreach (int i in timeMachine)
                timeTotal += i;
            return timeTotal;
        }

        /// <summary>回溯搜索所有schedule
        /// 
        /// </summary>
        /// <param name="dep"></param>
        void btAll(int dep, ArrayList allSchedules)
        {
            if (dep == numWork)
            {
                allSchedules.Add(new Schedule(getMakespan(), timeTotal, schedule));
                //printSchedule(getMakespan(), timeTotal);
                return;
            }

            for (int i = 0; i < numMachine; i++)
            {
                timeTotal += time[i, dep];
                timeMachine[i] += time[i, dep];
                schedule[dep] = i + 1;

                //穷举
                btAll(dep + 1, allSchedules);

                timeTotal -= time[i, dep];
                timeMachine[i] -= time[i, dep];
            }
        }    

        /// <summary>回溯生成所有调度
        /// 
        /// </summary>
        /// <returns></returns>
        public ArrayList getAllSchedules()
        {
            ArrayList allSchedules = new ArrayList();
            schedule = new int[numWork];
            btAll(0, allSchedules);
            return allSchedules;
        }
    }
}
