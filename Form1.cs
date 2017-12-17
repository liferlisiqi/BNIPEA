using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using System.Data;

namespace BNIPEA
{
    public partial class Form1 : Form
    {
        ArrayList allSolutions = new ArrayList();
        public Form1()
        {
            InitializeComponent();
        }

        private void readSubData(String tablename, int lo, int hi)
        {
            SqlConnection conn = new SqlConnection("Data Source=703B\\SQL2005;Initial Catalog=MNGAP;Integrated Security=True");
            DataTable dt = new DataTable();
            string sql = "select totaltime,timeCV from " + tablename + " where ID > " + lo + " and ID <= " + hi;
            try
            {
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(new SqlCommand(sql, conn));
                sda.Fill(dt);
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

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Solution solution = new Solution(
                   Math.Floor(Convert.ToDouble(dt.Rows[i][0].ToString()) * 10000) / 10000,
                   Math.Floor(Convert.ToDouble(dt.Rows[i][1].ToString()) * 10000) / 10000);
                allSolutions.Add(solution);
            }
        }

        private void read_BTN_Click(object sender, EventArgs e)
        {
            allSolutions.Clear();
            string M = M_ComBox.Text;
            string N = N_ComBox.Text;
            string ins = ins_ComBox.Text;
            readSubData("[" + M + "-" + N + "-" + ins + "]", 0, 10000000);
            //readSubData("[5-18-5]", 5000000, 10000000);
            //readSubData(10000000, 15000000);
            this.N_Text.Text = allSolutions.Count.ToString();
        }

        private void E_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList ParetoSet = new ArrayList();
            Solution min1Pareto = new Solution(1000, 1000);

            while (true)
            {
                min1Pareto = Find.min1Pareto(allSolutions, min1Pareto);
                if (min1Pareto.ob2 == 1000) break;
                ParetoSet.Add(min1Pareto);
            }

            DateTime endTime = System.DateTime.Now;
            E_T_Text.Text = (endTime - beginTime).TotalSeconds.ToString();
            E_P_Text.Text = ParetoSet.Count.ToString();
        }

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

            PC_LS_Text.Text = restSolutions.Count.ToString();

            Solution Pareto = new Solution(1000, 1000);
            while (true)
            {
                Pareto = Find.min1Pareto(restSolutions, Pareto);
                if (Pareto.ob2 == 1000) break;
                ParetoSet.Add(Pareto);
            }

            DateTime endTime = System.DateTime.Now;
            PC_T_Text.Text = (endTime - beginTime).TotalSeconds.ToString();
            PC_P_Text.Text = ParetoSet.Count.ToString();
        }

        private void IC_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList ParetoSet = new ArrayList();
            ArrayList restSolutions = allSolutions;

            Solution nearestPareto = Find.idealPareto(restSolutions);
            ParetoSet.Add(restSolutions);
            restSolutions = Find.ndSolutions(restSolutions, nearestPareto);
            IC_LS_Text.Text = restSolutions.Count.ToString();

            Solution min1Pareto = new Solution(1000, 1000);
            while (true)
            {
                min1Pareto = Find.min1Pareto(restSolutions, min1Pareto);
                if (min1Pareto.ob2 == 1000) break;
                ParetoSet.Add(min1Pareto);
            }

            DateTime endTime = System.DateTime.Now;
            IC_T_Text.Text = (endTime - beginTime).TotalSeconds.ToString() + "";
            IC_P_Text.Text = ParetoSet.Count + "";
        }

        private void PIC_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList ParetoSet = new ArrayList();
            ArrayList restSolutions = allSolutions;

            Solution min1Pareto = Find.min1Pareto(restSolutions);
            restSolutions = Find.ndSolutions(restSolutions, min1Pareto);
            ParetoSet.Add(min1Pareto);

            Solution min2Pareto = Find.min2Pareto(restSolutions);
            restSolutions = Find.ndSolutions(restSolutions, min2Pareto);
            ParetoSet.Add(min2Pareto);

            Solution nearestPareto = Find.idealPareto(restSolutions);
            ParetoSet.Add(restSolutions);
            restSolutions = Find.ndSolutions(restSolutions, nearestPareto);
            PIC_LS_Text.Text = restSolutions.Count.ToString();

            min1Pareto = new Solution(1000, 1000);
            while (true)
            {
                min1Pareto = Find.min1Pareto(restSolutions, min1Pareto);
                if (min1Pareto.ob2 == 1000) break;
                ParetoSet.Add(min1Pareto);
            }

            DateTime endTime = System.DateTime.Now;
            PIC_T_Text.Text = (endTime - beginTime).TotalSeconds.ToString() + "";
            PIC_P_Text.Text = ParetoSet.Count + "";    
        }

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
            EC_T_Text.Text = (endTime - beginTime).TotalSeconds.ToString();
            EC_P_Text.Text = ParetoSet.Count + "";
        }

        private void PEC_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList ParetoSet = new ArrayList();
            ArrayList restSolutions = allSolutions;

            Solution min1Pareto = Find.min1Pareto(restSolutions);
            restSolutions = Find.ndSolutions(restSolutions, min1Pareto);
            ParetoSet.Add(min1Pareto);

            Solution min2Pareto = Find.min2Pareto(restSolutions);
            restSolutions = Find.ndSolutions(restSolutions, min2Pareto);
            ParetoSet.Add(min2Pareto);
            PEC_LS_Text.Text = restSolutions.Count.ToString();

            min1Pareto = new Solution();
            while (restSolutions.Count != 0)
            {
                min1Pareto = Find.min1Pareto(restSolutions);
                restSolutions = Find.ndSolutions(restSolutions, min1Pareto);
                ParetoSet.Add(min1Pareto);
            }

            DateTime endTime = System.DateTime.Now;
            PEC_T_Text.Text = (endTime - beginTime).TotalSeconds.ToString();
            PEC_P_Text.Text = ParetoSet.Count.ToString();
        }

        private void IEC_Click(object sender, EventArgs e)
        {
            DateTime beginTime = System.DateTime.Now;
            ArrayList ParetoSet = new ArrayList();
            ArrayList restSolutions = allSolutions;

            Solution nearestPareto = Find.idealPareto(restSolutions);
            ParetoSet.Add(restSolutions);
            restSolutions = Find.ndSolutions(restSolutions, nearestPareto);
            IEC_LS_Text.Text = restSolutions.Count.ToString();

            while (restSolutions.Count != 0)
            {
                Solution ob1MaxPareto = Find.min1Pareto(restSolutions);
                restSolutions = Find.ndSolutions(restSolutions, ob1MaxPareto);
                ParetoSet.Add(ob1MaxPareto);
            }

            DateTime endTime = System.DateTime.Now;
            IEC_T_Text.Text = (endTime - beginTime).TotalSeconds.ToString();
            IEC_P_Text.Text = ParetoSet.Count.ToString();
        }

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
            PIEC_LS_Text.Text = restSolutions.Count.ToString();

            min1Pareto = new Solution();
            while (restSolutions.Count != 0)
            {
                min1Pareto = Find.min1Pareto(restSolutions);
                restSolutions = Find.ndSolutions(restSolutions, min1Pareto);
                ParetoSet.Add(min1Pareto);
            }
            DateTime endTime = System.DateTime.Now;
            PIEC_T_Text.Text = (endTime - beginTime).TotalSeconds.ToString();
            PIEC_P_Text.Text = ParetoSet.Count.ToString();
        }
    }
}
