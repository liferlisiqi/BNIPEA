using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiObjevtiveSystem
{
    class TxtHelper
    {
        public static Model TxtIn(int m, int n, int ins)
        {
            Model Model = new Model();
            Model.m = m;
            Model.n = n;
            Model.c = new int[m, n];

            string[] Lines = System.IO.File.ReadAllLines(@"D:\源码\多目标精确算法\多目标benchmark\GAP\GAP"
                + "_m-" + m.ToString() + "_n-" + n.ToString() + "_ins-" + ins.ToString() + ".txt");
            for (int i = 2; i < 2 + m; i++)
            {
                if (Lines[i] != "," && Lines[i] != "")
                {
                    Lines[i] = Lines[i].Replace("[", "").Replace("]", "").Replace(" ", "");
                    string[] strs = Lines[i].Split(new char[] { ',' });
                    for (int j = 0; j < Model.n; j++)
                    {
                        Model.c[i - 2, j] = Convert.ToInt32(strs[j]);
                        Console.Write(Model.c[i - 2, j] + ",");
                    }
                    Console.WriteLine();
                }
            }
            return Model;
        }
    }
}
