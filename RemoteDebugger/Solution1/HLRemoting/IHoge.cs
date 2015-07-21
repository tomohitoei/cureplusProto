using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace HLRemoting
{
    public interface IHoge
    {
        ReturnValues Func(string message, int count);
    }

    [Serializable()]public class ReturnValues
    {
        public string param1 = string.Empty;
        public bool abort = false;

        public MyTextRenderer r = null;
    }

    public interface JumpTargetSelecter
    {
        string SelectTarget(System.Collections.Generic.List<string> labels);
    }
}
