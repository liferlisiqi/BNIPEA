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
        private int msUpperLimit;
        private int ttUpperLimit;
        int[,] time;//每个机器完成每项工作的时间参数
        private int[] timeMachine;
        private int timeTotal;//临时总时间，用于回溯的比较和减枝
        private int[] schedule;
        private int[] msParetoSchedule;//makespan min pareto调度
        private int[] ttParetoSchedule;//timetotal min pareto调度
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

        /// <summary>回溯搜索makespan更小的调度
        /// 
        /// </summary>
        /// <param name="dep"></param>
        void btSmallerMs(int dep)
        {
            if (dep == numWork)
            {
                ttUpperLimit = getTimeTotal();
                msUpperLimit = getMakespan();
                Array.Copy(schedule, msParetoSchedule, schedule.Length);
                return;

            }

            for (int i = 0; i < numMachine; i++)
            {
                timeTotal += time[i, dep];
                timeMachine[i] += time[i, dep];
                schedule[dep] = i + 1;

                //减枝
                if (getMakespan() + 0.00001 * timeTotal < msUpperLimit + 0.00001 * ttUpperLimit)
                {                               //sufficiently small positive constant Epslon = 0.00001
                    btSmallerMs(dep + 1);
                }
                timeTotal -= time[i, dep];
                timeMachine[i] -= time[i, dep];
            }
        }

        /// <summary>回溯搜索timetotal更小的调度
        /// 
        /// </summary>
        /// <param name="dep"></param>
        void btSmallerTt(int dep)
        {
            if (dep == numWork)
            {
                ttUpperLimit = getTimeTotal();
                msUpperLimit = getMakespan();
                Array.Copy(schedule, ttParetoSchedule, schedule.Length);
                return;
            }

            for (int i = 0; i < numMachine; i++)
            {
                timeTotal += time[i, dep];
                timeMachine[i] += time[i, dep];
                schedule[dep] = i + 1;

                //减枝
                if (getMakespan() * 0.00001 + timeTotal < msUpperLimit * 0.00001 + ttUpperLimit)
                {                               //sufficiently small positive constant Epslon = 0.00001
                    btSmallerTt(dep + 1);
                }
                timeTotal -= time[i, dep];
                timeMachine[i] -= time[i, dep];
            }
        }

        /// <summary>回溯搜索非支配调度
        /// 
        /// </summary>
        /// <param name="dep"></param>
        void btNdSchedule(int dep, int msUpper, int ttUpper, ArrayList ndSchedules)
        {
            if (dep == numWork)
            {
                ndSchedules.Add(new Schedule(getMakespan(), timeTotal, schedule));
                return;
            }

            for (int i = 0; i < numMachine; i++)
            {
                timeTotal += time[i, dep];
                timeMachine[i] += time[i, dep];
                schedule[dep] = i + 1;
                //int makespan = getMakespan();
                //减枝
                if (getMakespan() < msUpper && timeTotal < ttUpper)
                {
                    btNdSchedule(dep + 1, msUpper, ttUpper, ndSchedules);
                }
                timeTotal -= time[i, dep];
                timeMachine[i] -= time[i, dep];
            }
        }

        /// <summary>执行回溯算法,求makespan最小的pareto
        /// 
        /// </summary>
        public Schedule getMsPareto()
        {
            msUpperLimit = int.MaxValue;
            ttUpperLimit = 0;
            schedule = new int[numWork];
            msParetoSchedule = new int[numWork];
            btSmallerMs(0);
            return new Schedule(msUpperLimit, ttUpperLimit, msParetoSchedule);
        }

        /// <summary>执行回溯算法，求timetotal最先的pareto
        /// 
        /// </summary>
        /// <returns></returns>
        public Schedule getTtPareto()
        {
            msUpperLimit = 0;
            ttUpperLimit = int.MaxValue;
            schedule = new int[numWork];
            ttParetoSchedule = new int[numWork];
            btSmallerTt(0);
            return new Schedule(msUpperLimit, ttUpperLimit, ttParetoSchedule);
        }

        /// <summary>用回溯寻找所有非支配解
        /// 
        /// </summary>
        /// <returns></returns>
        public ArrayList getNDSchedules(int msUpper, int ttUpper)
        {
            schedule = new int[numWork];
            ArrayList ndSchedules = new ArrayList();
            btNdSchedule(0, msUpper, ttUpper, ndSchedules);
            return ndSchedules;
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
