//本系统用于求解双目标非线性的所有非支配解，两个目标均为求min
//

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
        ArrayList allSolution = new ArrayList();
        public Form1()
        {
            InitializeComponent();
        }

        private void ExportExcel(System.Data.DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return;
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                return;
            }
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range range;
            long totalCount = dt.Rows.Count;
            long rowRead = 0;
            float percent = 0;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                range.Interior.ColorIndex = 15;
                range.Font.Bold = true;
            }
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = dt.Rows[r][i].ToString();
                }
                rowRead++;
                percent = ((float)(100 * rowRead)) / totalCount;
            }
            xlApp.Visible = true;
        }

        private void ExportExcel(ArrayList arl)
        {
            if (arl == null || arl.Count == 0) return;
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                return;
            }
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range range;
            long totalCount = arl.Count;
            long rowRead = 0;
            float percent = 0;
            //for (int i = 0; i < 2; i++)
            //{
            //    worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
            //    range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
            //    range.Interior.ColorIndex = 15;
            //    range.Font.Bold = true;
            //}
            for (int r = 0; r < arl.Count; r++)
            {
                Schedule shcedule = (Schedule)arl[r];
                worksheet.Cells[r + 1, 1] = shcedule.makespan;
                worksheet.Cells[r + 1, 2] = shcedule.timetotal;
                rowRead++;
                percent = ((float)(100 * rowRead)) / totalCount;
            }
            xlApp.Visible = true;
        }
        private void drawPoints(ArrayList solutionSet)
        {
            Int32 x, y;
            Graphics g = panel1.CreateGraphics();
            g.Clear(Color.Black);
            Bitmap p = new Bitmap(1, 1);
            Graphics h = Graphics.FromImage(p);
            h.Clear(Color.White);

            for (int i = 0; i < solutionSet.Count; i++)
            {
                Solution Temp_Class = (Solution)solutionSet[i];
                x = (int)(Temp_Class.ob1 * 2.5);
                y = 505 - (int)(Temp_Class.ob2 * 300);
                g.DrawImage(p, x, y);
            }
        }

        /// <summary>对于双目标广义指派问题，用递归生成所有解，然后再找所有pareto解
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateData_Click(object sender, EventArgs e)
        {
            Model model = TxtHelper.TxtIn(3, 8, 1);
            BackTrackUnrelated btu = new BackTrackUnrelated(model);
            DateTime begin = System.DateTime.Now;
            //Schedule msMinPareto = btu.getMsPareto();
            //Schedule ttMinPareto = btu.getTtPareto();
            //ArrayList ndSchedules = btu.getNDSchedules(ttMinPareto.makespan, msMinPareto.timetotal);
            ArrayList allSchedules = btu.getAllSchedules();
            DateTime end = System.DateTime.Now;
            //ExportExcel(allSchedules);
            geneDataTextBox.Text = (end - begin).TotalMilliseconds.ToString();
            foreach (Schedule i in allSchedules)
            {
                Solution solution = new Solution(150 - i.makespan, 150 - i.timetotal);
                allSolution.Add(solution);
            }
            MessageBox.Show("ok");
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
                Solution solution = new Solution();
                solution.ob1 = 200 - Convert.ToDouble(AllData.Rows[j][0].ToString());
                solution.ob2 = 1.5 - Convert.ToDouble(AllData.Rows[j][1].ToString());
                allSolution.Add(solution);
            }
            DateTime End_Time = System.DateTime.Now;
            textBox1.Text = (End_Time - Begin_Time).TotalMilliseconds.ToString();
            //drawPoints(allSolution);
            //NPOIHelper.outputExcel(allSolution, "D:/源码/多目标精确算法/多目标benchmark/AP/3-10-1-all.xls");
            //ExportExcel(AllData);

            MessageBox.Show("ok");
        }

        /// <summary>原始Epslon约束法
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Epslon_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList paretoSet = new ArrayList();
            Solution ob1MaxPareto = new Solution();

            while (true)
            {
                ob1MaxPareto = Find.min1Pareto(allSolution, ob1MaxPareto);
                if (ob1MaxPareto.ob2 == 0) break;
                paretoSet.Add(ob1MaxPareto);
            }

            //drawPoints(paretoSet);
            DateTime endTime = System.DateTime.Now;
            textBox2.Text = (endTime - beginTime).TotalMilliseconds.ToString();
            Console.WriteLine("1: " + paretoSet.Count);
        }

        /// <summary>先CUT被两个极点支配的解，在使用原始Eplson约束法
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwoPoleCUT_Epslon_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList restSolutionSet = allSolution;
            ArrayList paretoSet = new ArrayList();

            Solution ob1MaxPareto = Find.min1Pareto(restSolutionSet);
            restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb1MaxPareto(restSolutionSet, ob1MaxPareto);
            paretoSet.Add(ob1MaxPareto);

            Solution ob2MaxPareot = Find.min2Pareto(restSolutionSet);
            restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb2MaxPareto(restSolutionSet, ob2MaxPareot);
            paretoSet.Add(ob2MaxPareot);

            ob1MaxPareto = new Solution();
            while (true)
            {
                ob1MaxPareto = Find.min1Pareto(restSolutionSet, ob1MaxPareto);
                if (ob1MaxPareto.ob2 == 0) break;
                paretoSet.Add(ob1MaxPareto);
            }

            DateTime endTime = System.DateTime.Now;
            textBox3.Text = (endTime - beginTime).TotalMilliseconds.ToString();
            Console.WriteLine("2: " + paretoSet.Count);
        }

        /// <summary>一路EplsonCUT
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EpslonCUT_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList paretoSet = new ArrayList();
            Solution ob1MaxPareto = new Solution();
            ArrayList restSolutionSet = allSolution;

            while (restSolutionSet.Count != 0)
            {
                ob1MaxPareto = Find.min1Pareto(restSolutionSet);
                restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb1MaxPareto(restSolutionSet, ob1MaxPareto);
                paretoSet.Add(ob1MaxPareto);
            }

            DateTime endTime = System.DateTime.Now;
            textBox4.Text = (endTime - beginTime).TotalMilliseconds.ToString();
            Console.WriteLine("3: " + paretoSet.Count);
        }

        /// <summary>先两极点CUT，再一路EplsonCUT
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwoPoleCUT_EplsonCUT_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList paretoSet = new ArrayList();
            Solution ob1MaxPareto = new Solution();
            ArrayList restSolutionSet = allSolution;

            Solution ob2MaxPareto = Find.min2Pareto(restSolutionSet);
            restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb2MaxPareto(restSolutionSet, ob2MaxPareto);
            paretoSet.Add(ob2MaxPareto);

            while (restSolutionSet.Count != 0)
            {
                ob1MaxPareto = Find.min1Pareto(restSolutionSet);
                restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb1MaxPareto(restSolutionSet, ob1MaxPareto);
                paretoSet.Add(ob1MaxPareto);
            }

            DateTime endTime = System.DateTime.Now;
            textBox5.Text = (endTime - beginTime).TotalMilliseconds.ToString();
            Console.WriteLine("4: " + paretoSet.Count);
        }

        /// <summary>先用最近点CUT，再一路EplsonCUT
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdeaPointCUT_EpslonCUT_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList paretoSet = new ArrayList();
            ArrayList restSolutionSet = allSolution;

            Solution nearestPareto = Find.idealPoint(restSolutionSet);
            paretoSet.Add(restSolutionSet);
            restSolutionSet = BiObjevtiveSystem.Select.nondominatedByNearestPareto(restSolutionSet, nearestPareto);

            while (restSolutionSet.Count != 0)
            {
                Solution ob1MaxPareto = Find.min1Pareto(restSolutionSet);
                restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb1MaxPareto(restSolutionSet, ob1MaxPareto);
                paretoSet.Add(ob1MaxPareto);
            }
            //NPOIHelper(IdeaSet, "D:/源码/多目标精确算法/多目标benchmark/AP/3-10-1.xls");
            DateTime endTime = System.DateTime.Now;
            textBox6.Text = (endTime - beginTime).TotalMilliseconds.ToString();
            Console.WriteLine("5: " + paretoSet.Count);
        }

        /// <summary>先用两极点CUT，再用最近点CUT，再一路EplsonCUT
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwoCUT_IdeaCUT_EpslonCUT_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList paretoSet = new ArrayList();
            ArrayList restSolutionSet = allSolution;

            Parallel.Invoke(() =>
                {
                    Find.min1Pareto(restSolutionSet);
                },
                () =>
                {
                    Find.min2Pareto(restSolutionSet);
                },
                () =>
                {
                    Find.idealPoint(restSolutionSet);
                }
                    );
            DateTime endTime = System.DateTime.Now;
            Console.WriteLine("并行：" + (endTime - beginTime).TotalMilliseconds.ToString());


            DateTime beginTime1 = System.DateTime.Now;

            DateTime beginTime2 = System.DateTime.Now;
            Solution ob1MaxPareto = Find.min1Pareto(restSolutionSet);
            DateTime endTime2 = System.DateTime.Now;
            Console.WriteLine("1："+(endTime2 - beginTime2).TotalMilliseconds.ToString());

            //restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb1MaxPareto(restSolutionSet, ob1MaxPareto);
            //paretoSet.Add(ob1MaxPareto);
            DateTime beginTime3 = System.DateTime.Now;
            Solution ob2MaxPareto = Find.min2Pareto(restSolutionSet);
            DateTime endTime3 = System.DateTime.Now;
            Console.WriteLine("2：" + (endTime3 - beginTime3).TotalMilliseconds.ToString());
            //restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb2MaxPareto(restSolutionSet, ob2MaxPareto);
            //paretoSet.Add(ob2MaxPareto);
            DateTime beginTime4 = System.DateTime.Now;
            Solution nearestPareto = Find.idealPoint(restSolutionSet);
            DateTime endTime4 = System.DateTime.Now;
            Console.WriteLine("理：" + (endTime4 - beginTime4).TotalMilliseconds.ToString());
            //paretoSet.Add(nearestPareto);
            //restSolutionSet = BiObjevtiveSystem.Select.nondominatedByNearestPareto(restSolutionSet, nearestPareto);

            //ob1MaxPareto = new Solution();
            //while (restSolutionSet.Count != 0)
            //{
            //    ob1MaxPareto = Find.ob1MaxSolution(restSolutionSet);
            //    restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb1MaxPareto(restSolutionSet, ob1MaxPareto);
            //    paretoSet.Add(ob1MaxPareto);
            //}
            //NPOIHelper(IdeaSet, "D:/源码/多目标精确算法/多目标benchmark/AP/3-10-1.xls");
            DateTime endTime1 = System.DateTime.Now;
            Console.WriteLine("串行：" + (endTime1 - beginTime1).TotalMilliseconds.ToString());
            //textBox7.Text = (endTime - beginTime).TotalMilliseconds.ToString();
            //Console.WriteLine("6: " + paretoSet.Count);
        }

    }
}
