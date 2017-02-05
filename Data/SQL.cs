using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace Data
{
    class SQL
    {
        /// <summary>
        /// 功能：读取SQL Server数据库中表的目标一、目标二的数据，实例化为SolutionClass类，保存在arraylist中返回
        /// 参数：”Source“：stringl类型，数据源的表名称
        /// 返回值：“SolutionSet”:arraylist类型，solution集
        /// 其他：
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public ArrayList SQL_Input(string Source)
        {
            ArrayList arlSolutionSet = new ArrayList();
            DataTable dtaSolutionset = new DataTable();

            SqlConnection conn = new SqlConnection("Data Source=USER-20160720BD;integrated security=SSPI;Initial Catalog=MO-example");
            string str = "select ob1,ob2 from " + Source + " order by newid()";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(str, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dtaSolutionset);
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



            return arlSolutionSet;
        }
    }
}
