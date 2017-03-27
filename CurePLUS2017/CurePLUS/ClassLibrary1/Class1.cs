using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1
    {
    }

    public class hoge
    {
        public void huga(Dictionary<string,object> param)
        {
            Console.WriteLine(param["name"]);
            param["p1"] = (int)param["p1"] + 1;
        }
    }
}
