//本系统用于求解双目标非线性的所有非支配解，两个目标均为求min

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

        protected void ExportExcel(System.Data.DataTable dt)
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

        /// <summary>读数据
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadData_Click(object sender, EventArgs e)
        {
            DateTime Begin_Time = System.DateTime.Now;
            SqlConnection conn = new SqlConnection("Data Source=USER-20160720BD;Initial Catalog=MO-benchmark-AP;Integrated Security=True");
            System.Data.DataTable AllData = new System.Data.DataTable();
            string sql = "select sum,cv from [3-8-11]";
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
                solution.ob1 = 200- Convert.ToDouble(AllData.Rows[j][0].ToString());
                solution.ob2 = 1.5-Convert.ToDouble(AllData.Rows[j][1].ToString());
                allSolution.Add(solution);
            }
            DateTime End_Time = System.DateTime.Now;
            textBox1.Text = (End_Time - Begin_Time).TotalSeconds.ToString();
            drawPoints(allSolution);
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
                ob1MaxPareto = Find.ob1MaxSolution(allSolution, ob1MaxPareto);
                if (ob1MaxPareto.ob2 == 0) break;
                paretoSet.Add(ob1MaxPareto);
            }

            drawPoints(paretoSet);
            DateTime endTime = System.DateTime.Now;
            textBox2.Text = (endTime - beginTime).TotalSeconds.ToString();
            Console.WriteLine("1: "+paretoSet.Count);
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

            Solution ob1MaxPareto = Find.ob1MaxSolution(restSolutionSet);
            restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb1MaxPareto(restSolutionSet, ob1MaxPareto);
            paretoSet.Add(ob1MaxPareto);

            Solution ob2MaxPareot = Find.ob2MaxSolution(restSolutionSet);
            restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb2MaxPareto(restSolutionSet, ob2MaxPareot);
            paretoSet.Add(ob2MaxPareot);

            ob1MaxPareto = new Solution();
            while (true)
            {
                ob1MaxPareto = Find.ob1MaxSolution(restSolutionSet, ob1MaxPareto);
                if (ob1MaxPareto.ob2 == 0) break;
                paretoSet.Add(ob1MaxPareto);
            }

            DateTime endTime = System.DateTime.Now;
            textBox3.Text = (endTime - beginTime).TotalSeconds.ToString();
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
                ob1MaxPareto = Find.ob1MaxSolution(restSolutionSet);
                restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb1MaxPareto(restSolutionSet, ob1MaxPareto);
                paretoSet.Add(ob1MaxPareto);
            }

            DateTime endTime = System.DateTime.Now;
            textBox4.Text = (endTime - beginTime).TotalSeconds.ToString();
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

            Solution ob2MaxPareto = Find.ob2MaxSolution(restSolutionSet);
            restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb2MaxPareto(restSolutionSet,ob2MaxPareto);
            paretoSet.Add(ob2MaxPareto);

            while (restSolutionSet.Count != 0)
            {
                ob1MaxPareto = Find.ob1MaxSolution(restSolutionSet);
                restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb1MaxPareto(restSolutionSet, ob1MaxPareto);
                paretoSet.Add(ob1MaxPareto);
            }

            DateTime endTime = System.DateTime.Now;
            textBox5.Text = (endTime - beginTime).TotalSeconds.ToString();
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

            Solution nearestPareto = Find.nearestPointToIdealPoint(restSolutionSet);
            paretoSet.Add(restSolutionSet);
            restSolutionSet = BiObjevtiveSystem.Select.nondominatedByNearestPareto(restSolutionSet, nearestPareto);

            while (restSolutionSet.Count != 0)
            {
                Solution ob1MaxPareto = Find.ob1MaxSolution(restSolutionSet);
                restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb1MaxPareto(restSolutionSet, ob1MaxPareto);
                paretoSet.Add(ob1MaxPareto);
            }
            //NPOIHelper(IdeaSet, "D:/源码/多目标精确算法/多目标benchmark/AP/3-10-1.xls");
            DateTime endTime = System.DateTime.Now;
            textBox6.Text = (endTime - beginTime).TotalSeconds.ToString();
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

            Solution ob1MaxPareto = Find.ob1MaxSolution(restSolutionSet);
            restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb1MaxPareto(restSolutionSet, ob1MaxPareto);
            paretoSet.Add(ob1MaxPareto);

            Solution ob2MaxPareto = Find.ob2MaxSolution(restSolutionSet);
            restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb2MaxPareto(restSolutionSet, ob2MaxPareto);
            paretoSet.Add(ob2MaxPareto);

            Solution nearestPareto = Find.nearestPointToIdealPoint(restSolutionSet);
            paretoSet.Add(nearestPareto);
            restSolutionSet = BiObjevtiveSystem.Select.nondominatedByNearestPareto(restSolutionSet, nearestPareto);

            ob1MaxPareto = new Solution();
            while (restSolutionSet.Count != 0)
            {
                ob1MaxPareto = Find.ob1MaxSolution(restSolutionSet);
                restSolutionSet = BiObjevtiveSystem.Select.nondominatedByOb1MaxPareto(restSolutionSet, ob1MaxPareto);
                paretoSet.Add(ob1MaxPareto);
            }
            //NPOIHelper(IdeaSet, "D:/源码/多目标精确算法/多目标benchmark/AP/3-10-1.xls");
            DateTime endTime = System.DateTime.Now;
            textBox7.Text = (endTime - beginTime).TotalSeconds.ToString();
            Console.WriteLine("6: " + paretoSet.Count);
        }
        


    }
}
