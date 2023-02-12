using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LangChangeSimulator
{
    class walsclass
    {
        public static Dictionary<int, Dictionary<int, double>> walsmatrix = new Dictionary<int, Dictionary<int, double>>();
        public static Dictionary<int, List<string>> walsmorphemes = new Dictionary<int, List<string>>(); //Dictionary<featurevalue,List<morpheme meanings>>

        public Dictionary<int, int> features = new Dictionary<int, int>(); //dict<feature,value>

        public static void initWALS()
        {

        }

    }
}
