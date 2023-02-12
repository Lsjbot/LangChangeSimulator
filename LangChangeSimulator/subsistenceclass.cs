using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangChangeSimulator
{
    [Serializable()]
    public class subsistenceclass
    {
        public static Dictionary<string, string> subsistencedict = new Dictionary<string, string>()
        {{"hunter-gatherer","" },{"horticulture","horticulture" },{"agriculture","agriculture" },{"herding","husbandry" } };

        public string name = parameterclass.p.get("defaultsubsistence"); //default subsistence mode
        
    }
}
