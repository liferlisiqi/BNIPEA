//本系统用于求解双目标非线性整数规划问题的所有Pareto最优解，两个目标均为求min

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using Microsoft.Office.Interop.Excel;
using System.Data;

namespace BiObjevtiveSystem
{
    public partial class Form1 : Form
    {
        ArrayList allSolutions = new ArrayList();
        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>读数据
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadData_Click(object sender, EventArgs e)
        {
            DateTime Begin_Time = System.DateTime.Now;
            SqlConnection conn = new SqlConnection("Data Source=USER-20160720BD;" +
                "Initial Catalog=MNGAPbenchmark;Integrated Security=True");
            System.Data.DataTable AllData = new System.Data.DataTable();
            string sql = "select totaltime,timeCV from [5-15-1]";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(AllData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            for (int j = 0; j < AllData.Rows.Count; j++)
            {
                Solution solution = new Solution(
                    Convert.ToDouble(AllData.Rows[j][0].ToString()), 
                    Convert.ToDouble(AllData.Rows[j][1].ToString()));
                allSolutions.Add(solution);
            }
            DateTime End_Time = System.DateTime.Now;
            textBox1.Text = (End_Time - Begin_Time).TotalSeconds.ToString();
            //NPOIHelper.outputExcel(allSolution, "D:/源码/多目标精确算法/多目标benchmark/AP/3-10-1-all.xls");
        }

        /// <summary>原始Epslon约束法
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void epslon_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList ParetoSet = new ArrayList();
            Solution min1Pareto = new Solution(1000,1000);

            while (true)
            {
                min1Pareto = Find.min1Pareto(allSolutions, min1Pareto);
                if (min1Pareto.ob2 == 1000) break;
                ParetoSet.Add(min1Pareto);
            }

            DateTime endTime = System.DateTime.Now;
            textBox2.Text = (endTime - beginTime).TotalSeconds.ToString();
            Console.WriteLine("epslon: " + ParetoSet.Count);
        }

        /// <summary>eplson剪切
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EC_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList ParetoSet = new ArrayList();
            Solution Pareto = new Solution();
            ArrayList restSolutions = allSolutions;

            while (restSolutions.Count != 0)
            {
                Pareto = Find.min1Pareto(restSolutions);
                restSolutions = Find.ndSolutions(restSolutions, Pareto);
                ParetoSet.Add(Pareto);
            }

            DateTime endTime = System.DateTime.Now;
            textBox4.Text = (endTime - beginTime).TotalSeconds.ToString();
            Console.WriteLine("EC: " + ParetoSet.Count);
        }

        /// <summary>两极点Pareto剪切，原始Eplson约束法
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PC_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList restSolutions = allSolutions;
            ArrayList ParetoSet = new ArrayList();

            Solution min1Pareto = Find.min1Pareto(restSolutions);
            restSolutions = Find.ndSolutions(restSolutions, min1Pareto);
            ParetoSet.Add(min1Pareto);

            Solution min2Pareto = Find.min2Pareto(restSolutions);
            restSolutions = Find.ndSolutions(restSolutions, min2Pareto);
            ParetoSet.Add(min2Pareto);

            min1Pareto = new Solution(1000, 1000);
            while (true)
            {
                min1Pareto = Find.min1Pareto(restSolutions, min1Pareto);
                if (min1Pareto.ob2 == 1000) break;
                ParetoSet.Add(min1Pareto);
            }

            DateTime endTime = System.DateTime.Now;
            textBox3.Text = (endTime - beginTime).TotalSeconds.ToString();
            Console.WriteLine("PC: " + ParetoSet.Count);
        }   

        /// <summary>两极点Pareto剪切，eplson剪切
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PEC_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList ParetoSet = new ArrayList();
            Solution min1Pareto = new Solution(1000,100);
            ArrayList restSolutions = allSolutions;

            Solution min2Pareto = Find.min2Pareto(restSolutions);
            restSolutions = Find.ndSolutions(restSolutions, min2Pareto);
            ParetoSet.Add(min2Pareto);

            while (restSolutions.Count != 0)
            {
                min1Pareto = Find.min1Pareto(restSolutions);
                restSolutions = Find.ndSolutions(restSolutions, min1Pareto);
                ParetoSet.Add(min1Pareto);
            }

            DateTime endTime = System.DateTime.Now;
            textBox5.Text = (endTime - beginTime).TotalSeconds.ToString();
            Console.WriteLine("PEC: " + ParetoSet.Count);
        }

        /// <summary>理想Pareto剪切，eplson剪切
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IEC_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList ParetoSet = new ArrayList();
            ArrayList restSolutions = allSolutions;

            Solution nearestPareto = Find.idealPareto(restSolutions);
            ParetoSet.Add(restSolutions);
            restSolutions = Find.ndSolutions(restSolutions, nearestPareto);

            while (restSolutions.Count != 0)
            {
                Solution ob1MaxPareto = Find.min1Pareto(restSolutions);
                restSolutions = Find.ndSolutions(restSolutions, ob1MaxPareto);
                ParetoSet.Add(ob1MaxPareto);
            }
            //NPOIHelper(IdeaSet, "D:/源码/多目标精确算法/多目标benchmark/AP/3-10-1.xls");
            DateTime endTime = System.DateTime.Now;
            textBox6.Text = (endTime - beginTime).TotalSeconds.ToString();
            Console.WriteLine("IEC: " + ParetoSet.Count);
        }

        /// <summary>两极点Pareto剪切，理想Pareto剪切，eplson剪切
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PIEC_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList ParetoSet = new ArrayList();
            ArrayList restSolutions = allSolutions;

            //并行寻找两个极点Pareto，和理想Pareto
            //Parallel.Invoke(() =>
            //    {
            //        Find.min1Pareto(restSolutions);
            //    },
            //    () =>
            //    {
            //        Find.min2Pareto(restSolutions);
            //    },
            //    () =>
            //    {
            //        Find.idealPareto(restSolutions);
            //    }
            //        );
            

            Solution min1Pareto = Find.min1Pareto(restSolutions);
            restSolutions = Find.ndSolutions(restSolutions, min1Pareto);
            ParetoSet.Add(min1Pareto);
            Solution min2Pareto = Find.min2Pareto(restSolutions);
            restSolutions = Find.ndSolutions(restSolutions, min2Pareto);
            ParetoSet.Add(min2Pareto);
            Solution idealPareto = Find.idealPareto(restSolutions);
            ParetoSet.Add(idealPareto);
            restSolutions = Find.ndSolutions(restSolutions, idealPareto);

            min1Pareto = new Solution();
            while (restSolutions.Count != 0)
            {
                min1Pareto = Find.min1Pareto(restSolutions);
                restSolutions = Find.ndSolutions(restSolutions, min1Pareto);
                ParetoSet.Add(min1Pareto);
            }
            DateTime endTime = System.DateTime.Now;
            //NPOIHelper(IdeaSet, "D:/源码/多目标精确算法/多目标benchmark/AP/3-10-1.xls");
            textBox7.Text = (endTime - beginTime).TotalSeconds.ToString();
            Console.WriteLine("PIEC: " + ParetoSet.Count);
        }

    }
}
