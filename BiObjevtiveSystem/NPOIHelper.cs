using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace BiObjevtiveSystem
{
    class NPOIHelper
    {
        /// <summary>使用NPOI输出把arraylist的内容输出到Excel
        /// 
        /// </summary>
        /// <param name="solutionSet"></param>
        /// <param name="filename"></param>
        public static void outputExcel(ArrayList solutionSet,string filename)
        {
            HSSFWorkbook WorkBook = new HSSFWorkbook();
            ISheet Sheet = WorkBook.CreateSheet();

            for (int i = 0; i < solutionSet.Count; i++)
            {
                Solution Solution = (Solution)solutionSet[i];
                IRow Row = Sheet.CreateRow(i);
                Row.CreateCell(0).SetCellValue(Solution.ob1.ToString());
                Row.CreateCell(1).SetCellValue(Solution.ob2.ToString());
            }

            using (FileStream File = new FileStream(@filename, FileMode.Create))
            {
                WorkBook.Write(File);
                File.Close();
            }
        }
    }
}
